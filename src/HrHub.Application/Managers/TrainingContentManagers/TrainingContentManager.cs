using AutoMapper;
using FluentValidation.Results;
using HrHub.Abstraction.Attributes;
using HrHub.Abstraction.Consts;
using HrHub.Abstraction.Extensions;
using HrHub.Abstraction.Result;
using HrHub.Abstraction.StatusCodes;
using HrHub.Application.BusinessRules.TrainingContentBusinessRules;
using HrHub.Application.Helpers;
using HrHub.Application.Managers.FileTypeManagers;
using HrHub.Core.Base;
using HrHub.Core.Data.Repository;
using HrHub.Core.Helpers;
using HrHub.Domain.Contracts.Dtos.ContentLibraryDtos;
using HrHub.Domain.Contracts.Dtos.TrainingContentDtos;
using HrHub.Domain.Contracts.Responses.CommonResponse;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HrHub.Application.Managers.TrainingContentManagers
{
    [LifeCycle(Abstraction.Enums.LifeCycleTypes.Scoped)]
    public class TrainingContentManager : ManagerBase, ITrainingContentManager
    {
        private readonly IHrUnitOfWork unitOfWork;
        private readonly Repository<TrainingContent> contentRepository;
        private readonly Repository<ContentLibrary> contentLibraryRepository;
        private readonly Repository<Instructor> instructorRepository;
        private readonly IFileTypeManager fileTypeManager;
        private readonly IMapper mapper;

        public TrainingContentManager(IHttpContextAccessor httpContextAccessor,
                                      IHrUnitOfWork unitOfWork,
                                      IMapper mapper,
                                      IFileTypeManager fileTypeManager) : base(httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.contentRepository = unitOfWork.CreateRepository<TrainingContent>();
            this.mapper = mapper;
            this.contentLibraryRepository = unitOfWork.CreateRepository<ContentLibrary>();
            this.instructorRepository = unitOfWork.CreateRepository<Instructor>();
            this.fileTypeManager = fileTypeManager;
        }


        public async Task<Response<ReturnIdResponse>> AddTrainingContentAsync(AddTrainingContentDto data, CancellationToken cancellationToken = default)
        {
            var maxRowNum = await contentRepository.MaxAsync(
                predicate: c => c.ContentTypeId == data.ContentTypeId,
                selector: s => s.OrderId
            );

            await unitOfWork.BeginTransactionAsync();

            try
            {
                #region InstructorCodeWithDirectory
                var instructor = await instructorRepository.GetAsync(i => i.UserId == 15);// this.GetCurrentUserId());
                if (instructor == null)
                    return ProduceFailResponse<ReturnIdResponse>("Instructor bulunamadı!", StatusCodes.Status404NotFound);

                string directoryPath = Path.Combine("Uploads", instructor.InstructorCode);
                string fileName;
                AddContentLibraryDto contentLibraryData = new();
                if (data.File != null /*&& data.FileTypeId.HasValue*/)
                {
                    fileName = $"{DateTime.Now:yyyyMMddHHmmss}_{data.File.FileName}";
                    using var fileStream = data.File.OpenReadStream();
                    byte[] fileContent = new byte[data.File.Length];
                    await fileStream.ReadAsync(fileContent, cancellationToken);
                    var fileSaved = await FileHelper.SaveFileAsync(directoryPath, fileName, fileContent);
                    if (!fileSaved)
                        return ProduceFailResponse<ReturnIdResponse>("Dosya zaten mevcut.", StatusCodes.Status409Conflict);

                    // ContentLibrary Add
                    var fileTypeResponse = await fileTypeManager.GetByIdFileTypeAsync(Path.GetExtension(data?.File?.FileName));
                    var fileTypeResponseId = fileTypeResponse.Body.Id;

                    contentLibraryData = new AddContentLibraryDto
                    {
                        FileName = fileName,
                        FilePath = Path.Combine(directoryPath, fileName),
                        FileTypeId = /*data.FileTypeId.Value*/fileTypeResponseId,
                        CreatedDate = DateTime.UtcNow,
                        CreateUserId = 15,//this.GetCurrentUserId(),
                        IsActive = true
                    };
                }
                #endregion

                #region AddTrainingContent
                var newContent = mapper.Map<TrainingContent>(data);
                newContent.OrderId = maxRowNum.HasValue ? maxRowNum.Value + 1 : 1;

                var result = await contentRepository.AddAndReturnAsync(newContent, cancellationToken);
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
                    //if (data.FileTypeId is null)
                    //{ 
                    //    var fileTypeResponse = await fileTypeManager.GetByIdFileTypeAsync(Path.GetExtension(data?.File?.FileName));
                    //    contentLibraryEntity.FileTypeId = fileTypeResponse.Body.Id;
                    //}
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
            await unitOfWork.BeginTransactionAsync();

            try
            {
                #region Eğitim İçeriğini Bulma
                var existingContent = await contentRepository.GetAsync(
                    predicate: p => p.Id == data.Id,
                    include: c => c.Include(s => s.ContentLibraries)
                );

                if (existingContent == null)
                {
                    return ProduceFailResponse<CommonResponse>("Güncellenecek içerik bulunamadı.", StatusCodes.Status404NotFound);
                }
                #endregion

                #region Dosya Güncelleme ve Kitaplık
                if (data.File != null)
                {
                    string directoryPath = Path.Combine("Uploads", existingContent.TrainingSectionId.ToString());
                    string fileName = $"{DateTime.Now:yyyyMMddHHmmss}_{data.File.FileName}";

                    // Dosyayı kaydet
                    using var fileStream = data.File.OpenReadStream();
                    byte[] fileContent = new byte[data.File.Length];
                    await fileStream.ReadAsync(fileContent, cancellationToken);
                    var fileSaved = await FileHelper.SaveFileAsync(directoryPath, fileName, fileContent);

                    if (!fileSaved)
                        return ProduceFailResponse<CommonResponse>("Dosya zaten mevcut.", StatusCodes.Status409Conflict);


                    var fileTypeResponse = await fileTypeManager.GetByIdFileTypeAsync(Path.GetExtension(data.File.FileName));
                    if (fileTypeResponse.Body == null)
                        return ProduceFailResponse<CommonResponse>("Desteklenmeyen dosya türü.", HrStatusCodes.Status117FileFormatError);


                    var contentLibrary = existingContent.ContentLibraries.FirstOrDefault();
                    if (contentLibrary != null)
                    {
                        contentLibrary.FileName = fileName;
                        contentLibrary.FilePath = Path.Combine(directoryPath, fileName);
                        contentLibrary.FileTypeId = fileTypeResponse.Body.Id;
                        contentLibrary.UpdateUserId = this.GetCurrentUserId();
                        contentLibrary.UpdateDate = DateTime.UtcNow;
                        contentLibraryRepository.Update(contentLibrary);
                    }
                    else
                    {
                        var newContentLibrary = new ContentLibrary
                        {
                            FileName = fileName,
                            FilePath = Path.Combine(directoryPath, fileName),
                            FileTypeId = fileTypeResponse.Body.Id,
                            TrainingContentId = data.Id,
                            CreatedDate = DateTime.UtcNow,
                            CreateUserId = this.GetCurrentUserId(),
                            IsActive = true
                        };
                        await contentLibraryRepository.AddAsync(newContentLibrary, cancellationToken);
                    }
                }
                #endregion

                #region Eğitim İçeriği Güncelleme
                mapper.Map(data, existingContent);
                existingContent.UpdateDate = DateTime.UtcNow;
                existingContent.UpdateUserId = 15;// this.GetCurrentUserId();

                contentRepository.Update(existingContent);
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
                return ProduceFailResponse<CommonResponse>($"Güncelleme işlemi sırasında bir hata oluştu", StatusCodes.Status500InternalServerError);
            }
        }


        //public async Task<Response<ReturnIdResponse>> AddTrainingContentAsync(AddTrainingContentDto data, CancellationToken cancellationToken = default)
        //{
        //    // Validation
        //    //if (ValidationHelper.RuleBasedValidator<AddTrainingContentDto>(data, typeof(IAddTrainingContentBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
        //    //    return cBasedValidResult.SendResponse<ReturnIdResponse>();

        //    // Max Row Number for OrderId
        //    var maxRowNum = (await contentRepository.MaxAsync(predicate: c => c.ContentTypeId == data.ContentTypeId
        //                                                                        && c.TrainingSectionId == data.TrainingSectionId,                                                                            
        //                                                                        selector: s => s.OrderId));



        //    await unitOfWork.BeginTransactionAsync();

        //    try
        //    {
        //        var newContent = mapper.Map<TrainingContent>(data);
        //        newContent.OrderId = maxRowNum.HasValue ? maxRowNum.Value + 1 : 1;

        //        var result = await contentRepository.AddAndReturnAsync(newContent, cancellationToken);
        //        await unitOfWork.SaveChangesAsync(cancellationToken);

        //        var contentLibrary = await contentLibraryRepository.GetAsync(predicate: p => p.FileName == data.ContentLibraryFileName);
        //        if (contentLibrary is null)
        //        {
        //            var contentLibrayData = mapper.Map<ContentLibrary>(data);
        //            contentLibrayData.TrainingContentId = result.Id;
        //            await contentLibraryRepository.AddAsync(contentLibrayData, cancellationToken);
        //            await unitOfWork.SaveChangesAsync(cancellationToken);
        //        }
        //        await unitOfWork.CommitTransactionAsync();

        //        return ProduceSuccessResponse(new ReturnIdResponse
        //        {
        //            Id = newContent.Id
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        await unitOfWork.RollBackTransactionAsync();
        //        return ProduceFailResponse<ReturnIdResponse>("İşlem sırasında bir hata oluştu", 404);
        //    }
        //}


        //public async Task<Response<CommonResponse>> UpdateTrainingContentAsync(UpdateTrainingContentDto data, CancellationToken cancellationToken = default)
        //{
        //    //var validationResult = ValidationHelper.RuleBasedValidator<UpdateTrainingContentDto>(data, typeof(IUpdateTrainingContentBusinessRule));
        //    //if (validationResult is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
        //    //    return cBasedValidResult.SendResponse<CommonResponse>();



        //    //var contentLibraryExists = await contentLibraryRepository.ExistsAsync(p => p.FileName == data.ContentLibraryFileName
        //    //                                                                           && p.TrainingContentId != data.Id);
        //    //if (contentLibraryExists)
        //    //    return ProduceFailResponse<CommonResponse>("Bu içerik zaten yüklenmiş!", 404);

        //    await unitOfWork.BeginTransactionAsync();
        //    try
        //    {
        //        var existingContent = await contentRepository.GetAsync(predicate: p => p.Id == data.Id);



        //        mapper.Map(data, existingContent);
        //        existingContent.OrderId = data.OrderId;

        //        contentRepository.Update(existingContent);

        //        var contentLibrary = await contentLibraryRepository.GetAsync(predicate: p => p.Id == data.Id);
        //        if (contentLibrary is not null)
        //        {
        //            contentLibrary.FileName = data.ContentLibraryFileName;
        //            contentLibrary.FilePath = data.ContentLibraryFilePath;
        //            await contentLibraryRepository.UpdateAsync(contentLibrary);
        //        }

        //        await unitOfWork.SaveChangesAsync(cancellationToken);
        //        await unitOfWork.CommitTransactionAsync();

        //        return ProduceSuccessResponse(new CommonResponse
        //        {
        //            Code = StatusCodes.Status200OK,
        //            Message = "Success",
        //            Result = true
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        await unitOfWork.RollBackTransactionAsync();
        //        return ProduceFailResponse<CommonResponse>("İşlem sırasında bir hata oluştu!", 500);
        //    }
        //}

        public async Task<Response<CommonResponse>> DeleteTrainingContentAsync(long id, CancellationToken cancellationToken = default)
        {
            var contentDto = await contentRepository.GetAsync(predicate: p => p.Id == id, selector: s => mapper.Map<DeleteTrainingContentDto>(s));
            if (ValidationHelper.RuleBasedValidator<DeleteTrainingContentDto>(contentDto, typeof(IDeleteTrainingContentBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<CommonResponse>();

            var contentEntity = await contentRepository.GetAsync(predicate: p => p.Id == id, include: i =>i.Include(p=>p.ContentType));

            #region Content Delete
            if (contentEntity.ContentType.Code == ContentTypeConst.Video)
                await contentRepository.DeleteAsync(contentEntity);
            else
            {
                contentEntity.IsDelete = true;
                contentEntity.DeleteDate = DateTime.UtcNow;
                contentEntity.DeleteUserId = this.GetCurrentUserId();
                contentRepository.Update(contentEntity);
            }
            #endregion

            #region ContentLibrary Delete
            var contentLibraryEntity = await contentLibraryRepository.GetListAsync(predicate: p => p.TrainingContent.Id == id);
            contentLibraryEntity.ForEach(i => i.IsDelete = true);
            contentLibraryRepository.UpdateList(contentLibraryEntity.ToList());
            #endregion

            #region OrderId Update
            var contents = await contentRepository.GetListAsync(predicate: c => c.ContentTypeId == contentEntity.ContentTypeId
                                                                                && c.OrderId > contentEntity.OrderId);
            contents.ForEach(content => content.OrderId -= 1);
            contentRepository.UpdateList(contents.ToList());
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
        public async Task<Response<IEnumerable<GetTrainingContentDto>>> GetTrainingContentListAsync()
        {
            var trainingList = await contentRepository.GetListAsync(predicate: p => p.IsDelete != true,
                                                                        include: i => i.Include(s => s.ContentType)
                                                                        .Include(s => s.TrainingSection)
                                                                        .Include(s => s.ContentLibraries));
            var trainingListDto = mapper.Map<IEnumerable<GetTrainingContentDto>>(trainingList);
            return ProduceSuccessResponse(trainingListDto);

        }

        public async Task<Response<GetTrainingContentDto>> GetTrainingContentAsync(long id)
        {
            var trainingListDto = await contentRepository.GetAsync(predicate: p => p.IsDelete != true && p.Id == id,
                                                                        include: i => i.Include(s => s.ContentType)
                                                                        .Include(s => s.TrainingSection)
                                                                        .Include(s => s.ContentLibraries),
                                                                        selector: s => mapper.Map<GetTrainingContentDto>(s));
            return ProduceSuccessResponse(trainingListDto);

        }
    }
}
