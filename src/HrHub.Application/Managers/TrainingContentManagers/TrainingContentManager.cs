using AutoMapper;
using FluentValidation.Results;
using HrHub.Abstraction.Attributes;
using HrHub.Abstraction.Consts;
using HrHub.Abstraction.Contracts.Dtos.TrainingDtos;
using HrHub.Abstraction.Data.EfCore.Repository;
using HrHub.Abstraction.Extensions;
using HrHub.Abstraction.Result;
using HrHub.Abstraction.Settings;
using HrHub.Abstraction.StatusCodes;
using HrHub.Application.BusinessRules.TrainingContentBusinessRules;
using HrHub.Application.Helpers;
using HrHub.Application.Managers.FileTypeManagers;
using HrHub.Application.Managers.UserCertificateManagers;
using HrHub.Core.Base;
using HrHub.Core.Data.Repository;
using HrHub.Core.Helpers;
using HrHub.Domain.Contracts.Dtos.ContentLibraryDtos;
using HrHub.Domain.Contracts.Dtos.TrainingContentDtos;
using HrHub.Domain.Contracts.Responses.CommonResponse;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.Repositories.Abstract;
using HrHub.Infrastructre.Repositories.Concrete;
using HrHub.Infrastructre.UnitOfWorks;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using UglyToad.PdfPig;
namespace HrHub.Application.Managers.TrainingContentManagers
{
    [LifeCycle(Abstraction.Enums.LifeCycleTypes.Scoped)]
    public class TrainingContentManager : ManagerBase, ITrainingContentManager
    {
        private readonly IHrUnitOfWork unitOfWork;
        private readonly Repository<TrainingContent> trainingContentRepository;
        private readonly Repository<ContentLibrary> contentLibraryRepository;
        private readonly Repository<Instructor> instructorRepository;
        private readonly Repository<TrainingSection> trainingSectionRepository;
        private readonly Repository<Training> trainingRepository;
        private readonly IFileTypeManager fileTypeManager;
        private readonly IMapper mapper;
        private readonly Repository<CurrAccTrainingUser> currAccTrainingUserRepository;
        private readonly Repository<UserContentsViewLog> userContentsViewLogRepository;
        private readonly Repository<UserContentsViewLogDetail> userContentsViewLogDetailRepository;
        private readonly Repository<Exam> examRepository;
        private readonly Repository<UserExam> userExamRepository;
        private readonly Repository<ExamVersion> examVersionRepository;
        private readonly IUserCertificateManager userCertificateManager;

        public TrainingContentManager(IHttpContextAccessor httpContextAccessor,
                                      IHrUnitOfWork unitOfWork,
                                      IMapper mapper,
                                      IFileTypeManager fileTypeManager,
                                      IUserCertificateManager userCertificateManager) : base(httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.trainingContentRepository = unitOfWork.CreateRepository<TrainingContent>();
            this.mapper = mapper;
            this.contentLibraryRepository = unitOfWork.CreateRepository<ContentLibrary>();
            this.instructorRepository = unitOfWork.CreateRepository<Instructor>();
            this.trainingSectionRepository = unitOfWork.CreateRepository<TrainingSection>();
            this.trainingRepository = unitOfWork.CreateRepository<Training>();
            this.fileTypeManager = fileTypeManager;
            this.currAccTrainingUserRepository = unitOfWork.CreateRepository<CurrAccTrainingUser>();
            this.userContentsViewLogRepository = unitOfWork.CreateRepository<UserContentsViewLog>();
            this.userContentsViewLogDetailRepository = unitOfWork.CreateRepository<UserContentsViewLogDetail>();
            this.examRepository = unitOfWork.CreateRepository<Exam>();
            this.userExamRepository = unitOfWork.CreateRepository<UserExam>();
            this.examVersionRepository = unitOfWork.CreateRepository<ExamVersion>();
            this.userCertificateManager = userCertificateManager;
        }

        public async Task<Response<ReturnIdResponse>> AddTrainingContentAsync(AddTrainingContentDto data, CancellationToken cancellationToken = default)
        {
            var lectureSettings = AppSettingsHelper.GetData<LectureSettings>();

            var trainingId = await trainingSectionRepository.GetAsync(predicate: t => t.Id == data.TrainingSectionId, selector: s => s.TrainingId);
            var maxRowNum = await trainingContentRepository.MaxAsync(
                predicate: c => c.TrainingSection.TrainingId == trainingId && c.ContentTypeId == data.ContentTypeId,
                selector: s => s.OrderId
            );

            await unitOfWork.BeginTransactionAsync();

            try
            {
                #region InstructorCodeWithDirectory
                var instructor = await instructorRepository.GetAsync(i => i.UserId == this.GetCurrentUserId());
                if (instructor == null)
                    return ProduceFailResponse<ReturnIdResponse>("Instructor bulunamadı!", StatusCodes.Status404NotFound);

                string directoryPath = Path.Combine("Uploads", instructor.InstructorCode);
                string thumbnailDirectoryPath = Path.Combine(directoryPath, "Thumbnails");

                // Klasörleri oluştur
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);
                if (!Directory.Exists(thumbnailDirectoryPath))
                    Directory.CreateDirectory(thumbnailDirectoryPath);

                string fileName;
                AddContentLibraryDto contentLibraryData = new();
                int? calculatedPartCount = null;

                if (data.File != null)
                {
                    string sanitizedFileName = Path.GetFileNameWithoutExtension(data.File.FileName)
                        .Replace(" ", "-")
                        .Replace("_", "-")
                        .ToLowerInvariant();

                    string extension = Path.GetExtension(data.File.FileName)?.ToLowerInvariant();
                    fileName = $"{sanitizedFileName}{extension}";
                    string filePath = Path.Combine(directoryPath, fileName);

                    int counter = 1;
                    while (File.Exists(filePath))
                    {
                        fileName = $"{sanitizedFileName}({counter}){extension}";
                        filePath = Path.Combine(directoryPath, fileName);
                        counter++;
                    }
                    // Dosya kaydetme işlemi
                    using var fileStream = data.File.OpenReadStream();
                    byte[] fileContent = new byte[data.File.Length];
                    await fileStream.ReadAsync(fileContent, cancellationToken);
                    await FileHelper.SaveFileAsync(directoryPath, fileName, fileContent);

                    // **Thumbnail oluşturma işlemi**
                    string thumbnailFileName = $"{sanitizedFileName}.jpg";
                    string thumbnailFilePath = null;
                    TimeSpan? videoDuration = null;
                    int? pageCount = null;
                    double? fileSize = null;

                    if (extension == ".mp4" || extension == ".avi" || extension == ".mov")
                    {
                        videoDuration = await GetVideoDurationAsync(filePath);
                        thumbnailFilePath = Path.Combine(thumbnailDirectoryPath, thumbnailFileName);
                        GenerateThumbnail(filePath, thumbnailFilePath, lectureSettings.ThumbnailCaptureSecond); // 5. saniyeden kare al
                        
                        if (videoDuration.HasValue)
                        {
                            int secondsPerPart = 15 * 60; // 15 dakika
                            var totalSeconds = videoDuration.Value.TotalSeconds;
                            calculatedPartCount = (int)Math.Ceiling(totalSeconds / secondsPerPart);
                        }
                    }
                    else if (extension == ".pdf")
                    {
                        var pdfDetails = await GetPdfDetailsAsync(filePath); // **PDF bilgileri**
                        pageCount = pdfDetails.PageCount;
                        fileSize = pdfDetails.FileSize;
                    }


                    // **ContentLibrary Add**
                    var fileTypeResponse = await fileTypeManager.GetByIdFileTypeAsync(Path.GetExtension(data?.File?.FileName));
                    var fileTypeResponseId = fileTypeResponse.Body.Id;

                    contentLibraryData = new AddContentLibraryDto
                    {
                        FileName = fileName,
                        FilePath = filePath,
                        VideoDuration = videoDuration,
                        FileTypeId = fileTypeResponseId,
                        TrainingContentId = null, // Şimdilik null, aşağıda güncellenecek
                        Thumbnail = thumbnailFilePath, // Thumbnail dosya yolu
                        CreatedDate = DateTime.UtcNow,
                        CreateUserId = this.GetCurrentUserId(),
                        IsActive = true,
                        DocumentFileSize = fileSize,
                        DocumentPageCount = pageCount
                    };
                }
                #endregion

                #region AddTrainingContent
                var newContent = mapper.Map<TrainingContent>(data);
                newContent.OrderId = maxRowNum.HasValue ? maxRowNum.Value + 1 : 1;
                newContent.PartCount = calculatedPartCount ?? 1;

                var result = await trainingContentRepository.AddAndReturnAsync(newContent, cancellationToken);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                #endregion

                #region Library
                ContentLibrary contentLibraryEntity = null;
                if (data.ContentLibraryId.HasValue) // Kitaplıktan bir içerik seçildiyse
                {
                    var existingLibraryContent = await contentLibraryRepository.GetAsync(c => c.Id == data.ContentLibraryId.Value);
                    if (existingLibraryContent != null)
                    {
                        existingLibraryContent.TrainingContentId = result.Id;
                        contentLibraryEntity = contentLibraryRepository.UpdateAndReturn(existingLibraryContent);
                        await unitOfWork.SaveChangesAsync(cancellationToken);
                    }
                }
                else
                {
                    contentLibraryData.TrainingContentId = result.Id;
                    contentLibraryEntity = mapper.Map<ContentLibrary>(contentLibraryData);
                    await contentLibraryRepository.AddAsync(contentLibraryEntity, cancellationToken);
                }
                #endregion

                await unitOfWork.SaveChangesAsync(cancellationToken);
                await unitOfWork.CommitTransactionAsync();
                return ProduceSuccessResponse(new ReturnIdResponse { Id = newContent.Id });
            }
            catch (Exception ex)
            {
                await unitOfWork.RollBackTransactionAsync();
                return ProduceFailResponse<ReturnIdResponse>($"İşlem sırasında bir hata oluştu: {ex.Message}", 500);
            }
        }

        public async Task<Response<CommonResponse>> UpdateTrainingContentAsync(UpdateTrainingContentDto data, CancellationToken cancellationToken = default)
        {
            var lectureSettings = AppSettingsHelper.GetData<LectureSettings>();
            await unitOfWork.BeginTransactionAsync();

            try
            {
                #region **Eğitim İçeriğini Bulma**
                var existingContent = await trainingContentRepository.GetAsync(
                    predicate: p => p.Id == data.Id,
                    include: c => c.Include(s => s.ContentLibraries)
                );

                if (existingContent == null)
                {
                    return ProduceFailResponse<CommonResponse>("Güncellenecek içerik bulunamadı.", StatusCodes.Status404NotFound);
                }
                #endregion

                #region Dosya Güncelleme ve Kitaplık
                int? calculatedPartCount = null;
                if (data.File != null)
                {
                    var instructor = await instructorRepository.GetAsync(i => i.UserId == this.GetCurrentUserId());
                    if (instructor == null)
                        return ProduceFailResponse<CommonResponse>("Instructor bulunamadı!", StatusCodes.Status404NotFound);

                    string directoryPath = Path.Combine("Uploads", instructor.InstructorCode);
                    string thumbnailDirectoryPath = Path.Combine(directoryPath, "Thumbnails");

                    // **Dizinleri oluştur**
                    if (!Directory.Exists(directoryPath))
                        Directory.CreateDirectory(directoryPath);
                    if (!Directory.Exists(thumbnailDirectoryPath))
                        Directory.CreateDirectory(thumbnailDirectoryPath);

                    // **Dosya adını oluştur ve temizle**
                    string sanitizedFileName = Path.GetFileNameWithoutExtension(data.File.FileName)
                        .Replace(" ", "-")
                        .Replace("_", "-")
                        .ToLowerInvariant();

                    string extension = Path.GetExtension(data.File.FileName)?.ToLowerInvariant();
                    string fileName = $"{sanitizedFileName}{extension}";
                    string filePath = Path.Combine(directoryPath, fileName);

                    int counter = 1;
                    while (File.Exists(filePath))
                    {
                        fileName = $"{sanitizedFileName}({counter}){extension}";
                        filePath = Path.Combine(directoryPath, fileName);
                        counter++;
                    }

                    // **Dosya kaydetme işlemi
                    using var fileStream = data.File.OpenReadStream();
                    byte[] fileContent = new byte[data.File.Length];
                    await fileStream.ReadAsync(fileContent, cancellationToken);
                    await FileHelper.SaveFileAsync(directoryPath, fileName, fileContent);

                    // **Thumbnail oluşturma
                    string thumbnailFileName = $"{sanitizedFileName}.jpg";
                    string thumbnailFilePath = Path.Combine(thumbnailDirectoryPath, thumbnailFileName);
                    TimeSpan? videoDuration = null;

                    if (extension == ".mp4" || extension == ".avi" || extension == ".mov")
                    {
                        GenerateThumbnail(filePath, thumbnailFilePath, lectureSettings.ThumbnailCaptureSecond); // **Videolar için 5. saniyeden thumbnail al**
                       
                        videoDuration = await GetVideoDurationAsync(filePath);                      
                        if (videoDuration.HasValue)
                        {
                            int secondsPerPart = 15 * 60; // veya: lectureSettings.PartLengthInMinutes * 60;
                            var totalSeconds = videoDuration.Value.TotalSeconds;
                            calculatedPartCount = (int)Math.Ceiling(totalSeconds / secondsPerPart);
                        }
                    }

                    //Dosya formatını kontrol et
                    var fileTypeResponse = await fileTypeManager.GetByIdFileTypeAsync(extension);
                    if (fileTypeResponse.Body == null)
                        return ProduceFailResponse<CommonResponse>("Desteklenmeyen dosya türü.", HrStatusCodes.Status117FileFormatError);

                    // **ContentLibrary
                    //var videoDuration = await GetVideoDurationAsync(filePath);
                    var contentLibrary = existingContent.ContentLibraries.FirstOrDefault();
                    if (contentLibrary != null)
                    {
                        contentLibrary.FileName = fileName;
                        contentLibrary.FilePath = filePath;
                        contentLibrary.FileTypeId = fileTypeResponse.Body.Id;
                        contentLibrary.Thumbnail = thumbnailFilePath;
                        contentLibrary.UpdateUserId = this.GetCurrentUserId();
                        contentLibrary.UpdateDate = DateTime.UtcNow;
                        contentLibrary.VideoDuration = videoDuration;
                        contentLibraryRepository.Update(contentLibrary);
                    }
                    else
                    {
                        var newContentLibrary = new ContentLibrary
                        {
                            FileName = fileName,
                            FilePath = filePath,
                            FileTypeId = fileTypeResponse.Body.Id,
                            TrainingContentId = data.Id,
                            Thumbnail = thumbnailFilePath, // **Yeni thumbnail dosya yolu**
                            CreatedDate = DateTime.UtcNow,
                            CreateUserId = this.GetCurrentUserId(),
                            VideoDuration = videoDuration,
                            IsActive = true
                        };
                        await contentLibraryRepository.AddAsync(newContentLibrary, cancellationToken);
                    }
                }
                #endregion

                #region **Eğitim İçeriği Güncelleme**
                mapper.Map(data, existingContent);
                existingContent.UpdateDate = DateTime.UtcNow;
                existingContent.UpdateUserId = this.GetCurrentUserId();
                if (calculatedPartCount.HasValue)
                    existingContent.PartCount = calculatedPartCount.Value;

                trainingContentRepository.Update(existingContent);
                #endregion

                await unitOfWork.SaveChangesAsync(cancellationToken);
                await unitOfWork.CommitTransactionAsync();

                return ProduceSuccessResponse(new CommonResponse
                {
                    Message = "Eğitim içeriği başarıyla güncellendi.",
                    Code = StatusCodes.Status200OK,
                    Result = true
                });
            }
            catch (Exception ex)
            {
                await unitOfWork.RollBackTransactionAsync();
                return ProduceFailResponse<CommonResponse>($"Güncelleme işlemi sırasında bir hata oluştu: {ex.Message}", StatusCodes.Status500InternalServerError);
            }
        }

        #region OldAdd_Update
        //public async Task<Response<ReturnIdResponse>> AddTrainingContentAsync(AddTrainingContentDto data, CancellationToken cancellationToken = default)
        //{
        //    var maxRowNum = await contentRepository.MaxAsync(
        //        predicate: c => c.ContentTypeId == data.ContentTypeId,
        //        selector: s => s.OrderId
        //    );

        //    await unitOfWork.BeginTransactionAsync();

        //    try
        //    {
        //        #region InstructorCodeWithDirectory
        //        var instructor = await instructorRepository.GetAsync(i => i.UserId == 15);// this.GetCurrentUserId());
        //        if (instructor == null)
        //            return ProduceFailResponse<ReturnIdResponse>("Instructor bulunamadı!", StatusCodes.Status404NotFound);

        //        string directoryPath = Path.Combine("Uploads", instructor.InstructorCode);
        //        string fileName;
        //        AddContentLibraryDto contentLibraryData = new();
        //        if (data.File != null /*&& data.FileTypeId.HasValue*/)
        //        {
        //            fileName = $"{data.File.FileName}";
        //            using var fileStream = data.File.OpenReadStream();
        //            byte[] fileContent = new byte[data.File.Length];
        //            await fileStream.ReadAsync(fileContent, cancellationToken);
        //            var fileSaved = await FileHelper.SaveFileAsync(directoryPath, fileName, fileContent);
        //            string filePath = Path.Combine(directoryPath, fileName);
        //            if (!fileSaved)
        //            {
        //                int counter = 1;
        //                while (File.Exists(filePath))
        //                {
        //                    fileName = $"{fileName}({counter})";
        //                    counter++;
        //                }
        //                //return ProduceFailResponse<ReturnIdResponse>("Dosya zaten mevcut.", StatusCodes.Status409Conflict);
        //            }
        //            // ContentLibrary Add
        //            var fileTypeResponse = await fileTypeManager.GetByIdFileTypeAsync(Path.GetExtension(data?.File?.FileName));
        //            var fileTypeResponseId = fileTypeResponse.Body.Id;

        //            contentLibraryData = new AddContentLibraryDto
        //            {
        //                FileName = fileName,
        //                FilePath = Path.Combine(directoryPath, fileName),
        //                FileTypeId = /*data.FileTypeId.Value*/fileTypeResponseId,
        //                CreatedDate = DateTime.UtcNow,
        //                CreateUserId = 15,//this.GetCurrentUserId(),
        //                IsActive = true
        //            };
        //        }
        //        #endregion

        //        #region AddTrainingContent
        //        var newContent = mapper.Map<TrainingContent>(data);
        //        newContent.OrderId = maxRowNum.HasValue ? maxRowNum.Value + 1 : 1;

        //        var result = await contentRepository.AddAndReturnAsync(newContent, cancellationToken);
        //        await unitOfWork.SaveChangesAsync(cancellationToken);
        //        #endregion

        //        #region Library
        //        ContentLibrary contentLibraryEntity = null;
        //        if (data.ContentLibraryId.HasValue) // Kitaplıktan bir içerik seçildiyse
        //        {
        //            var existingLibraryContent = await contentLibraryRepository.GetAsync(c => c.Id == data.ContentLibraryId.Value);
        //            if (existingLibraryContent != null)
        //            {
        //                existingLibraryContent.TrainingContentId = result.Id;
        //                contentLibraryEntity = contentLibraryRepository.UpdateAndReturn(existingLibraryContent);
        //                await unitOfWork.SaveChangesAsync(cancellationToken);
        //            }
        //        }
        //        else
        //        {
        //            contentLibraryData.TrainingContentId = result.Id;
        //            contentLibraryEntity = mapper.Map<ContentLibrary>(contentLibraryData);
        //            //if (data.FileTypeId is null)
        //            //{ 
        //            //    var fileTypeResponse = await fileTypeManager.GetByIdFileTypeAsync(Path.GetExtension(data?.File?.FileName));
        //            //    contentLibraryEntity.FileTypeId = fileTypeResponse.Body.Id;
        //            //}
        //            await contentLibraryRepository.AddAsync(contentLibraryEntity, cancellationToken);
        //        }
        //        #endregion

        //        await unitOfWork.SaveChangesAsync(cancellationToken);
        //        await unitOfWork.CommitTransactionAsync();
        //        return ProduceSuccessResponse(new ReturnIdResponse { Id = newContent.Id });
        //    }
        //    catch (Exception ex)
        //    {
        //        await unitOfWork.RollBackTransactionAsync();
        //        return ProduceFailResponse<ReturnIdResponse>($"İşlem sırasında bir hata oluştu: {ex.Message}", 500);
        //    }
        //}


        //public async Task<Response<CommonResponse>> UpdateTrainingContentAsync(UpdateTrainingContentDto data, CancellationToken cancellationToken = default)
        //{
        //    await unitOfWork.BeginTransactionAsync();

        //    try
        //    {
        //        #region Eğitim İçeriğini Bulma
        //        var existingContent = await contentRepository.GetAsync(
        //            predicate: p => p.Id == data.Id,
        //            include: c => c.Include(s => s.ContentLibraries)
        //        );

        //        if (existingContent == null)
        //        {
        //            return ProduceFailResponse<CommonResponse>("Güncellenecek içerik bulunamadı.", StatusCodes.Status404NotFound);
        //        }
        //        #endregion

        //        #region Dosya Güncelleme ve Kitaplık
        //        if (data.File != null)
        //        {
        //            var instructor = await instructorRepository.GetAsync(i => i.UserId == this.GetCurrentUserId());
        //            if (instructor == null)
        //                return ProduceFailResponse<CommonResponse>("Instructor bulunamadı!", StatusCodes.Status404NotFound);

        //            string directoryPath = Path.Combine("Uploads", instructor.InstructorCode);
        //            string fileName;

        //            // Dosyayı kaydet
        //            fileName = $"{data.File.FileName}";
        //            using var fileStream = data.File.OpenReadStream();
        //            byte[] fileContent = new byte[data.File.Length];
        //            await fileStream.ReadAsync(fileContent, cancellationToken);
        //            var fileSaved = await FileHelper.SaveFileAsync(directoryPath, fileName, fileContent);

        //            string filePath = Path.Combine(directoryPath, fileName);
        //            if (!fileSaved)
        //            {
        //                int counter = 1;
        //                while (File.Exists(filePath))
        //                {
        //                    fileName = $"{fileName}({counter})";
        //                    counter++;
        //                }
        //                //return ProduceFailResponse<ReturnIdResponse>("Dosya zaten mevcut.", StatusCodes.Status409Conflict);
        //            }


        //            var fileTypeResponse = await fileTypeManager.GetByIdFileTypeAsync(Path.GetExtension(data.File.FileName));
        //            if (fileTypeResponse.Body == null)
        //                return ProduceFailResponse<CommonResponse>("Desteklenmeyen dosya türü.", HrStatusCodes.Status117FileFormatError);


        //            var contentLibrary = existingContent.ContentLibraries.FirstOrDefault();
        //            if (contentLibrary != null)
        //            {
        //                contentLibrary.FileName = fileName;
        //                contentLibrary.FilePath = Path.Combine(directoryPath, fileName);
        //                contentLibrary.FileTypeId = fileTypeResponse.Body.Id;
        //                contentLibrary.UpdateUserId = this.GetCurrentUserId();
        //                contentLibrary.UpdateDate = DateTime.UtcNow;
        //                contentLibraryRepository.Update(contentLibrary);
        //            }
        //            else
        //            {
        //                var newContentLibrary = new ContentLibrary
        //                {
        //                    FileName = fileName,
        //                    FilePath = Path.Combine(directoryPath, fileName),
        //                    FileTypeId = fileTypeResponse.Body.Id,
        //                    TrainingContentId = data.Id,
        //                    CreatedDate = DateTime.UtcNow,
        //                    CreateUserId = this.GetCurrentUserId(),
        //                    IsActive = true
        //                };
        //                await contentLibraryRepository.AddAsync(newContentLibrary, cancellationToken);
        //            }
        //        }
        //        #endregion

        //        #region Eğitim İçeriği Güncelleme
        //        mapper.Map(data, existingContent);
        //        existingContent.UpdateDate = DateTime.UtcNow;
        //        existingContent.UpdateUserId = 15;// this.GetCurrentUserId();

        //        contentRepository.Update(existingContent);
        //        #endregion

        //        await unitOfWork.SaveChangesAsync(cancellationToken);
        //        await unitOfWork.CommitTransactionAsync();

        //        return ProduceSuccessResponse(new CommonResponse
        //        {
        //            Message = "Eğitim içeriği başarıyla güncellendi.",
        //            Code = StatusCodes.Status200OK,
        //            Result = true
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        await unitOfWork.RollBackTransactionAsync();
        //        return ProduceFailResponse<CommonResponse>($"Güncelleme işlemi sırasında bir hata oluştu", StatusCodes.Status500InternalServerError);
        //    }
        //}
        #endregion

        public async Task<Response<CommonResponse>> DeleteTrainingContentAsync(long id, CancellationToken cancellationToken = default)
        {
            var contentDto = await trainingContentRepository.GetAsync(predicate: p => p.Id == id, selector: s => mapper.Map<DeleteTrainingContentDto>(s));
            if (ValidationHelper.RuleBasedValidator<DeleteTrainingContentDto>(contentDto, typeof(IDeleteTrainingContentBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<CommonResponse>();

            var contentEntity = await trainingContentRepository.GetAsync(predicate: p => p.Id == id, include: i => i.Include(p => p.ContentType));

            #region Content Delete
            if (contentEntity.ContentType.Code == ContentTypeConst.Lecture)
                await trainingContentRepository.DeleteAsync(contentEntity);
            else
            {
                contentEntity.IsDelete = true;
                contentEntity.DeleteDate = DateTime.UtcNow;
                contentEntity.DeleteUserId = this.GetCurrentUserId();
                trainingContentRepository.Update(contentEntity);
            }
            #endregion

            #region ContentLibrary Delete
            var contentLibraryEntity = await contentLibraryRepository.GetListAsync(predicate: p => p.TrainingContent.Id == id);
            contentLibraryEntity.ForEach(i => i.IsDelete = true);
            contentLibraryRepository.UpdateList(contentLibraryEntity.ToList());
            #endregion

            #region OrderId Update
            var contents = await trainingContentRepository.GetListAsync(predicate: c => c.ContentTypeId == contentEntity.ContentTypeId
                                                                                && c.OrderId > contentEntity.OrderId);
            contents.ForEach(content => content.OrderId -= 1);
            trainingContentRepository.UpdateList(contents.ToList());
            #endregion

            await unitOfWork.BeginTransactionAsync();
            try
            {
                await unitOfWork.SaveChangesAsync(cancellationToken);
                await unitOfWork.CommitTransactionAsync();

                return ProduceSuccessResponse(new CommonResponse
                {
                    Code = StatusCodes.Status200OK,
                    Message = "İçerik başarıyla silindi!",
                    Result = true
                });
            }
            catch (Exception ex)
            {
                await unitOfWork.RollBackTransactionAsync();
                return ProduceFailResponse<CommonResponse>("İşlem sırasında bir hata oluştu!", 500);
            }
        }
        public async Task<Response<IEnumerable<GetListTrainingContentDto>>> GetTrainingContentListAsync()
        {
            var trainingList = await trainingContentRepository.GetListAsync(predicate: p => p.IsDelete != false,
                                                                        include: i => i.Include(s => s.ContentType)
                                                                        .Include(s => s.TrainingSection)
                                                                        .Include(s => s.ContentLibraries),
                                                                          selector: s => new GetListTrainingContentDto
                                                                          {
                                                                              Id = s.Id,
                                                                              IsActive = s.IsActive,
                                                                              Title = s.Title,
                                                                              Abbreviation = s.Abbreviation,
                                                                              Code = s.Code,
                                                                              Description = s.Description,
                                                                              Time = s.Time,
                                                                              PageCount = s.PageCount,
                                                                              CompletedRate = s.CompletedRate,

                                                                              Mandatory = s.Mandatory,
                                                                              OrderId = s.OrderId,
                                                                              AllowSeeking = s.AllowSeeking,
                                                                              PartCount = s.PartCount,
                                                                              MinReadTimeThreshold = s.MinReadTimeThreshold,

                                                                              TrainingSectionTitle = s.TrainingSection.Title,
                                                                              TrainingContentAbbreviation = s.TrainingSection.Abbreviation,
                                                                              TrainingSectionCode = s.TrainingSection.Code,
                                                                              TrainingSectionDescription = s.TrainingSection.Description,

                                                                              TrainingContentTitle = s.ContentType.Title,
                                                                              TrainingContentCode = s.ContentType.Code,
                                                                              TrainingContentDescription = s.ContentType.Description,
                                                                              TrainingContentLangCode = s.ContentType.LangCode,

                                                                              ContentLibraries = s.ContentLibraries.Select(cl => new GetContentLibraryDto
                                                                              {
                                                                                  FileName = cl.FileName,
                                                                                  FilePath = cl.FilePath,
                                                                                  Thumbnail = cl.Thumbnail,
                                                                                  VideoDuration = cl.VideoDuration,
                                                                                  DocumentFileSize = cl.DocumentFileSize,
                                                                                  DocumentPageCount = cl.DocumentPageCount
                                                                              }).ToList()
                                                                          });
            return ProduceSuccessResponse(trainingList);

        }
        public async Task<Response<GetTrainingContentDto>> GetTrainingContentAsync(long id)
        {
            var trainingListDto = await trainingContentRepository.GetAsync(predicate: p => p.Id == id,
                                                                        include: i => i.Include(s => s.ContentType)
                                                                        .Include(s => s.TrainingSection)
                                                                        .Include(s => s.ContentLibraries),
                                                                        selector: s => new GetTrainingContentDto
                                                                        {
                                                                            IsActive = s.IsActive,
                                                                            Title = s.Title,
                                                                            Abbreviation = s.Abbreviation,
                                                                            Code = s.Code,
                                                                            Description = s.Description,
                                                                            Time = s.Time,
                                                                            PageCount = s.PageCount,
                                                                            CompletedRate = s.CompletedRate,

                                                                            Mandatory = s.Mandatory,
                                                                            OrderId = s.OrderId,
                                                                            AllowSeeking = s.AllowSeeking,
                                                                            PartCount = s.PartCount,
                                                                            MinReadTimeThreshold = s.MinReadTimeThreshold,

                                                                            TrainingSectionTitle = s.TrainingSection.Title,
                                                                            TrainingContentAbbreviation = s.TrainingSection.Abbreviation,
                                                                            TrainingSectionCode = s.TrainingSection.Code,
                                                                            TrainingSectionDescription = s.TrainingSection.Description,

                                                                            TrainingContentTitle = s.ContentType.Title,
                                                                            TrainingContentCode = s.ContentType.Code,
                                                                            TrainingContentDescription = s.ContentType.Description,
                                                                            TrainingContentLangCode = s.ContentType.LangCode,
                                                                            ContentLibraryFileName = s.ContentLibraries.FirstOrDefault(d => d.TrainingContentId == id).FileName,
                                                                            ContentLibraryFilePath = s.ContentLibraries.FirstOrDefault(d => d.TrainingContentId == id).FilePath,
                                                                            ContentLibraryThumbnail = s.ContentLibraries.FirstOrDefault(d => d.TrainingContentId == id).Thumbnail,
                                                                            ContentLibraryVideoDuration = s.ContentLibraries.FirstOrDefault(d => d.TrainingContentId == id).VideoDuration,
                                                                            DocumentFileSize = s.ContentLibraries.FirstOrDefault(d => d.TrainingContentId == id).DocumentFileSize,
                                                                            DocumentPageCount = s.ContentLibraries.FirstOrDefault(d => d.TrainingContentId == id).DocumentPageCount
                                                                        });
            return ProduceSuccessResponse(trainingListDto);

        }
        private void GenerateThumbnail(string videoPath, string outputImagePath, int second = 5)
        {
            string ffmpegPath = @"C:\ffmpeg\bin\ffmpeg.exe"; // FFmpeg yolu

            if (!File.Exists(ffmpegPath))
            {
                return;
            }

            // **Eğer thumbnail dosyası zaten varsa, sil
            if (File.Exists(outputImagePath))
            {
                try
                {
                    File.Delete(outputImagePath);
                }
                catch (Exception ex)
                {
                    return;
                }
            }

            ProcessStartInfo processInfo = new ProcessStartInfo
            {
                FileName = ffmpegPath,
                Arguments = $"-i \"{videoPath}\" -ss {second} -vframes 1 \"{outputImagePath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process { StartInfo = processInfo })
            {
                process.Start();
            }
        }
        private async Task<TimeSpan?> GetVideoDurationAsync(string videoPath)
        {

            string ffmpegPath = @"C:\ffmpeg\bin\ffmpeg.exe";

            ProcessStartInfo processInfo = new ProcessStartInfo
            {
                FileName = ffmpegPath,
                Arguments = $"-i \"{videoPath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process { StartInfo = processInfo })
            {
                process.Start();
                string output = await process.StandardError.ReadToEndAsync();
                process.WaitForExit();

                //Regex ile Süre bul
                var match = System.Text.RegularExpressions.Regex.Match(output, @"Duration: (\d+):(\d+):(\d+.\d+)");
                if (match.Success)
                {
                    int hours = int.Parse(match.Groups[1].Value);
                    int minutes = int.Parse(match.Groups[2].Value);
                    double seconds = double.Parse(match.Groups[3].Value, System.Globalization.CultureInfo.InvariantCulture);

                    return new TimeSpan(0, hours, minutes, (int)seconds);
                }

                return null;
            }
        }
        public async Task<(int PageCount, double FileSize)> GetPdfDetailsAsync(string pdfPath)
        {
            try
            {
                using (PdfDocument pdfDoc = PdfDocument.Open(pdfPath))
                {
                    int pageCount = pdfDoc.NumberOfPages;
                    long fileSizeBytes = new FileInfo(pdfPath).Length; // **Dosya Boyutunu Byte Olarak Al**

                    double fileSizeMB = Math.Round(fileSizeBytes / (1024.0 * 1024.0), 2);

                    return (pageCount, fileSizeMB);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PDF Bilgileri Okunamadı: {ex.Message}");
                return (0, 0);
            }
        }

        public async Task<Response<CommonResponse>> LogWatchProgressAsync(LogWatchProgressDto logDto, CancellationToken cancellationToken = default)
        {
            var currentUserId = GetCurrentUserId();

            // 1. İÇERİK KONTROLÜ: İçeriğin hangi Eğitime (TrainingId) ait olduğunu bul.
            var content = await trainingContentRepository.GetAsync<TrainingContent>(
                predicate: x => x.Id == logDto.TrainingContentId,
                include: i => i.Include(c => c.TrainingSection),
                cancellationToken: cancellationToken
            );

            if (content == null)
                return ProduceFailResponse<CommonResponse>("İçerik bulunamadı.", StatusCodes.Status404NotFound);

            long trainingId = content.TrainingSection.TrainingId ?? 0;

            // 2. YETKİ KONTROLÜ: Kullanıcı bu eğitimi almış mı?
            var assignment = await currAccTrainingUserRepository.GetAsync<CurrAccTrainingUser>(
                predicate: x => x.UserId == currentUserId
                             && x.CurrAccTrainings.TrainingId == trainingId
                             && x.IsActive == true,
                include: i => i.Include(x => x.CurrAccTrainings),
                cancellationToken: cancellationToken
            );

            if (assignment == null)
                return ProduceFailResponse<CommonResponse>("Bu eğitimi izleme yetkiniz yok.", StatusCodes.Status403Forbidden);

            // 3. MASTER LOG İŞLEMLERİ (UserContentsViewLog)
            // Kullanıcının bu içerik için bir kaydı var mı?
            var masterLog = await userContentsViewLogRepository.GetAsync<UserContentsViewLog>(
                predicate: x => x.TrainingContentId == logDto.TrainingContentId
                             && x.CurrAccTrainingUserId == assignment.Id, // Atama ID'si
                cancellationToken: cancellationToken
            );

            if (masterLog == null)
            {
                // Yoksa oluştur
                masterLog = new UserContentsViewLog
                {
                    TrainingContentId = logDto.TrainingContentId,
                    CurrAccTrainingUserId = assignment.Id,
                    StartDate = DateTime.UtcNow,
                    IsActive = true,
                    IsCompleted = false,
                    // İlk kayıtta UpdateDate de set edilsin ki sıralamada görünsün
                    UpdateDate = DateTime.UtcNow
                };
                await userContentsViewLogRepository.AddAsync(masterLog, cancellationToken);
                await unitOfWork.SaveChangesAsync(cancellationToken); // ID oluşması için Save şart
            }
            else
            {
                // Varsa UpdateDate güncelle (Eğitimlerim sayfasında 'Son İzlenen' olarak yukarı çıkması için kritik)
                masterLog.UpdateDate = DateTime.UtcNow;
                userContentsViewLogRepository.Update(masterLog);
            }

            // 4. DETAY LOG İŞLEMLERİ (UserContentsViewLogDetail)
            // Bu 15 saniyelik parçayı daha önce izlemiş mi?
            var existingDetail = await userContentsViewLogDetailRepository.GetAsync<UserContentsViewLogDetail>(
                predicate: x => x.UserContentsViewLogId == masterLog.Id && x.PartNumber == logDto.PartNumber,
                cancellationToken: cancellationToken
            );

            if (existingDetail == null)
            {
                // İzlememişse yeni detay ekle
                var newDetail = new UserContentsViewLogDetail
                {
                    UserContentsViewLogId = masterLog.Id,
                    PartNumber = logDto.PartNumber,
                    LogDate = DateTime.UtcNow, // CreateDate yerine LogDate kullanıyoruz
                    IsActive = true
                };
                await userContentsViewLogDetailRepository.AddAsync(newDetail, cancellationToken);
            }
            else
            {
                // Zaten izlemişse, son erişim tarihini güncelle (Geri sarıp tekrar izleme senaryosu)
                existingDetail.LogDate = DateTime.UtcNow;
                userContentsViewLogDetailRepository.Update(existingDetail);
            }

            // 5. TAMAMLANMA DURUMU
            // Eğer Frontend 'Bu son parça' dediyse içeriği tamamlandı işaretle
            if (logDto.IsLastPart && !masterLog.IsCompleted)
            {
                masterLog.IsCompleted = true;
                masterLog.CompletedDate = DateTime.UtcNow;
                userContentsViewLogRepository.Update(masterLog);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return ProduceSuccessResponse(new CommonResponse
            {
                Message = "İlerleme kaydedildi.",
                Result = true,
                Code = StatusCodes.Status200OK
            });
        }

        // Gerekli Repository'leri Constructor'a eklediğinden emin ol:
        // IRepository<Exam>, IRepository<UserExam>, IRepository<ExamVersion>

        public async Task<Response<GetContentForPlayerDto>> GetTrainingContentByIdForUserAsync(long id)
        {
            var currentUserId = GetCurrentUserId();

            // 1. İÇERİĞİ ÇEK (ContentType Include Edildi)
            var targetContent = await trainingContentRepository.GetAsync<TrainingContent>(
                predicate: x => x.Id == id,
                include: i => i.Include(c => c.TrainingSection).Include(c => c.ContentType),
                cancellationToken: default
            );

            if (targetContent == null)
                return ProduceFailResponse<GetContentForPlayerDto>("İçerik bulunamadı.", StatusCodes.Status404NotFound);

            // 2. MAPPING (Temel DTO Doldurma)
            var dto = mapper.Map<GetContentForPlayerDto>(targetContent);
            dto.CanView = true;
            dto.MissingContents = new List<MissingContentItemDto>();
            dto.LastWatchedPart = 0;
            dto.IsCompleted = false;

            long trainingId = targetContent.TrainingSection.TrainingId ?? 0;

            // 3. EĞİTİM ATAMASINI BUL
            var assignment = await currAccTrainingUserRepository.GetAsync(
                predicate: x => x.UserId == currentUserId
                             && x.CurrAccTrainings.TrainingId == trainingId
                             && x.IsActive == true
            );

            if (assignment == null)
            {
                dto.CanView = false;
                dto.BlockMessage = "Eğitim ataması bulunamadı.";
                return ProduceSuccessResponse(dto);
            }

            // 4. LOGLARI ÇEK (Video izleme durumları için)
            var userLogs = await userContentsViewLogRepository.GetListAsync(
                predicate: x => x.CurrAccTrainingUserId == assignment.Id
            );
            var targetLog = userLogs.FirstOrDefault(x => x.TrainingContentId == id);
            if (targetLog != null) dto.IsCompleted = targetLog.IsCompleted;


            // =================================================================================
            // KONTROL A: MANDATORY (ZORUNLU) İÇERİK KONTROLÜ
            // =================================================================================
            // Eğer içerik daha önce tamamlanmadıysa geçmişe bak
            if (!dto.IsCompleted)
            {
                var allContents = await trainingContentRepository.GetListAsync(
                    predicate: x => x.TrainingSection.TrainingId == trainingId && x.IsActive == true && x.IsDelete == false,
                    include: i => i.Include(s => s.TrainingSection)
                );

                // Kendisinden önceki Zorunlu içerikler bitmiş mi?
                var requiredContents = allContents
                    .Where(c => c.OrderId < targetContent.OrderId && c.Mandatory == true)
                    .OrderBy(c => c.OrderId)
                    .ToList();

                foreach (var req in requiredContents)
                {
                    var log = userLogs.FirstOrDefault(l => l.TrainingContentId == req.Id);
                    if (log == null || !log.IsCompleted)
                    {
                        dto.CanView = false;
                        dto.MissingContents.Add(new MissingContentItemDto { Id = req.Id, OrderId = req.OrderId ?? 0, Title = req.Title });
                    }
                }

                if (!dto.CanView)
                {
                    dto.BlockMessage = "Bu bölüme geçmeden önce tamamlamanız gereken içerikler var.";
                    dto.FilePath = null; // Güvenlik: Linki gizle
                    return ProduceSuccessResponse(dto); // Direkt dön, aşağıya bakmaya gerek yok
                }
            }

            // =================================================================================
            // KONTROL B: SINAV DURUM ANALİZİ (Exam Logic)
            // =================================================================================
            // ContentType kontrolü (Sabitlerini kendi projene göre kontrol et: ContentTypeConst.Exam)
            if (targetContent.ContentType != null && targetContent.ContentType.Code == "Exam")
            {
                dto.IsExam = true;
                dto.FilePath = null; // Sınavın video dosyası olmaz, güvenlik için temizle.

                // Sınav Tanımını Bul (Exam -> TrainingContents ilişkisi)
                var exam = await examRepository.GetAsync(
                    predicate: x => x.TrainingContents.Any(tc => tc.Id == id),
                    include: i => i.Include(e => e.ExamVersions)
                                   .Include(e => e.ExamAction)
                );

                if (exam != null)
                {
                    dto.ExamId = exam.Id;

                    // Aksiyon Mesajı
                    if (exam.ExamAction != null)
                        dto.ExamActionDescription = !string.IsNullOrEmpty(exam.ExamAction.Description)
                            ? exam.ExamAction.Description
                            : "Sınav başarısızlığı durumunda eğitim süreci etkilenebilir.";

                    // Aktif Versiyonu Bul
                    var activeVersion = exam.ExamVersions
                        .Where(v => v.IsPublished == true)
                        .OrderByDescending(v => v.PublishedDate ?? v.CreatedDate)
                        .FirstOrDefault();

                    if (activeVersion != null)
                    {
                        dto.PassingScore = (double?)activeVersion.PassingScore;

                        // Öğrencinin bu versiyondaki TÜM sınav geçmişini çek
                        var userExams = await userExamRepository.GetListAsync(
                            predicate: ue => ue.ExamVersionId == activeVersion.Id
                                          && ue.CurrAccTrainingUserId == assignment.Id
                        );

                        dto.AttemptCount = userExams.Count();

                        // En son işlem gören (veya oluşturulan) kaydı al
                        var lastSession = userExams.OrderByDescending(ue => ue.StartDate).FirstOrDefault();

                        if (lastSession == null)
                        {
                            dto.ExamStatus = "NotStarted"; // Hiç giriş yok
                        }
                        else
                        {
                            dto.UserExamId = lastSession.Id;
                            dto.UserScore = (double?)lastSession.SuccessRate;

                            if (!lastSession.IsCompleted)
                            {
                                // Kayıt var ve bitmemiş -> DEVAM ET
                                dto.ExamStatus = "Continue";
                            }
                            else
                            {
                                // Bitmiş -> Sonuca bak
                                if (lastSession.IsSuccess)
                                {
                                    dto.ExamStatus = "Passed";
                                    dto.IsCompleted = true; // Frontend ve Backend mutabık
                                }
                                else
                                {
                                    dto.ExamStatus = "Failed";
                                    dto.IsCompleted = false; // Frontend "Tekrar Dene" butonu koyacak
                                }
                            }
                        }
                    }
                    else
                    {
                        dto.CanView = false;
                        dto.BlockMessage = "Bu sınavın aktif bir versiyonu bulunmuyor. Eğitmenle iletişime geçin.";
                    }
                }
            }
            else
            {
                dto.IsExam = false;
            }

            return ProduceSuccessResponse(dto);
        }

        public async Task<Response<GetContentForPlayerDto>> GetNextContentAsync(GetNextContentRequestDto request)
        {
            var currentUserId = GetCurrentUserId();

            // 1. Sıralı İçerik ID'lerini Çek
            var allContents = await trainingContentRepository.GetListAsync(
                predicate: c => c.TrainingSection.TrainingId == request.TrainingId
                                && c.IsActive == true && c.IsDelete != true
                                && c.TrainingSection.IsActive == true && c.TrainingSection.IsDelete != true,
                include: i => i.Include(x => x.TrainingSection), // Sıralama için gerekli olabilir
                orderBy: o => o.OrderBy(c => c.TrainingSection.RowNumber).ThenBy(c => c.OrderId)
            );

            if (!allContents.Any())
                return ProduceFailResponse<GetContentForPlayerDto>("İçerik bulunamadı.", HrStatusCodes.Status111DataNotFound);

            // 2. Kullanıcının İzlediği Logları Çek
            // NOT: Burada TrainingId üzerinden logları çekiyoruz.
            var watchedLogs = await userContentsViewLogRepository.GetListAsync(
                predicate: x => x.CurrAccTrainingUser.UserId == currentUserId && x.TrainingContent.TrainingSection.TrainingId == request.TrainingId && x.IsCompleted == true
            );

            var watchedContentIds = watchedLogs.Select(x => x.TrainingContentId).ToList();

            // 3. İzlenmemiş İLK içeriği bul
            var nextContent = allContents.FirstOrDefault(content => !watchedContentIds.Contains(content.Id));

            // 4. EĞER BİTMİŞSE -> SERTİFİKA
            if (nextContent == null)
            {
                // Sertifika sürecini başlat
                await userCertificateManager.CreateCertificateRequestAsync(request.TrainingId);

                return ProduceSuccessResponse(new GetContentForPlayerDto
                {
                    IsTrainingFinished = true,
                    Message = "Tebrikler! Eğitimi tamamladınız. Sertifikanız hazırlanıyor."
                });
            }

            // 5. SIRADAKİ VARSA -> SENİN METODUNU ÇAĞIR
            return await GetTrainingContentByIdForUserAsync(nextContent.Id);
        }

    }
}
