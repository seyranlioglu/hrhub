using AutoMapper;
using FluentValidation.Results;
using HrHub.Abstraction.BusinessRules;
using HrHub.Abstraction.Consts;
using HrHub.Abstraction.Contracts.Dtos.TrainingDtos;
using HrHub.Abstraction.Data.EfCore.Repository;
using HrHub.Abstraction.Extensions;
using HrHub.Abstraction.Result;
using HrHub.Abstraction.StatusCodes;
using HrHub.Application.BusinessRules.TrainingBusinessRules;
using HrHub.Core.Base;
using HrHub.Core.Data.Repository;
using HrHub.Core.Helpers;
using HrHub.Domain.Contracts.Dtos.DashboardDtos;
using HrHub.Domain.Contracts.Dtos.TrainingDtos;
using HrHub.Domain.Contracts.Responses.CommonResponse;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.Repositories.Abstract;
using HrHub.Infrastructre.Repositories.Concrete;
using HrHub.Infrastructre.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HrHub.Application.Managers.Trainings;

public class TrainingManager : ManagerBase, ITrainingManager
{
    private readonly IHrUnitOfWork hrUnitOfWork;
    private readonly IMapper mapper;
    private readonly Repository<Training> trainingRepository;
    private readonly Repository<TrainingStatus> trainingStatuRepository;
    private readonly Repository<TrainingContent> trainingContentRepository;
    private readonly Repository<TrainingSection> trainingSectionRepository;
    private readonly ICurrAccTrainingUserRepository currAccTrainingUserRepository;
    private readonly IUserContentsViewLogRepository userContentsViewLogRepository;
    private readonly IInstructorRepository instructorRepository;
    public TrainingManager(IHttpContextAccessor httpContextAccessor,
                           IHrUnitOfWork hrUnitOfWork,
                           IMapper mapper,
                           ICurrAccTrainingUserRepository currAccTrainingUserRepository,
                           IUserContentsViewLogRepository userContentsViewLogRepository,
                           IInstructorRepository instructorRepository) : base(httpContextAccessor)
    {
        this.hrUnitOfWork = hrUnitOfWork;
        trainingRepository = hrUnitOfWork.CreateRepository<Training>();
        trainingStatuRepository = hrUnitOfWork.CreateRepository<TrainingStatus>();
        trainingContentRepository = hrUnitOfWork.CreateRepository<TrainingContent>();
        trainingSectionRepository = hrUnitOfWork.CreateRepository<TrainingSection>();
        this.mapper = mapper;
        this.currAccTrainingUserRepository = currAccTrainingUserRepository;
        this.userContentsViewLogRepository = userContentsViewLogRepository;
        this.instructorRepository = instructorRepository;
    }

    public async Task<Response<ReturnIdResponse>> AddTrainingAsync(AddTrainingDto data, CancellationToken cancellationToken = default)
    {
        if (ValidationHelper.RuleBasedValidator<AddTrainingDto>(data, typeof(IAddTrainingBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
            return cBasedValidResult.SendResponse<ReturnIdResponse>();


        var trainingEntity = mapper.Map<Training>(data);
        trainingEntity.IsActive = true;
        trainingEntity.ForWhomId = data.ForWhomId == 0 ? (long?)null : data.ForWhomId;
        trainingEntity.InstructorId = data.InstructorId == 0 ? (long?)null : data.InstructorId;
        trainingEntity.CompletionTimeUnitId = data.CompletionTimeUnitId == 0 ? (long?)null : data.CompletionTimeUnitId;
        trainingEntity.TrainingLevelId = data.TrainingLevelId == 0 ? (long?)null : data.TrainingLevelId;
        trainingEntity.PreconditionId = data.PreconditionId == 0 ? (long?)null : data.PreconditionId;
        trainingEntity.ForWhomId = data.ForWhomId == 0 ? (long?)null : data.ForWhomId;
        trainingEntity.EducationLevelId = data.EducationLevelId == 0 ? (long?)null : data.EducationLevelId;
        trainingEntity.PriceTierId = data.PriceTierId == 0 ? (long?)null : data.PriceTierId;
        trainingEntity.CurrentAmount = data.Amount - (data.Amount * data.DiscountRate / 100); // Bunu konuşuruz!!! 
        trainingEntity.TrainingLanguageId = data.TrainingLanguageId;
        trainingEntity.CompletionTime = data.CompletionTime;
        //CompletionTime hesaplanacak, elle girilmeyecek. Konuşacağız
        trainingEntity.TrainingStatusId = await trainingStatuRepository.GetAsync(predicate: p => p.Code == TrainingStatuConst.Preparing,
                                                                                 selector: s => s.Id);
        var result = await trainingRepository.AddAndReturnAsync(trainingEntity);
        await hrUnitOfWork.SaveChangesAsync();
        return ProduceSuccessResponse(new ReturnIdResponse
        {
            Id = result.Id
        });
    }
    public async Task<Response<CommonResponse>> UpdateTrainingAsync(UpdateTrainingDto dto, CancellationToken cancellationToken = default)
    {
        var training = await trainingRepository.GetAsync(predicate: t => t.Id == dto.Id);

        if (training == null)
        {
            return ProduceFailResponse<CommonResponse>("Eğitim bulunamadı.", StatusCodes.Status404NotFound);
        }

        bool isGeneralUpdate = !string.IsNullOrEmpty(dto.Title) || !string.IsNullOrEmpty(dto.Description);

        if (isGeneralUpdate)
        {
            if (ValidationHelper.RuleBasedValidator<UpdateTrainingDto>(dto, typeof(IUpdateTrainingBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<CommonResponse>();

            // Mapper'ı sadece veri varsa çalıştır veya null olmayanları map'le. 
            // (Not: AutoMapper konfigürasyonunda Null ignore yoksa bu tehlikelidir. 
            // Şimdilik DTO'dan entity'e map ediyoruz.)

            mapper.Map(dto, training);

            // Fiyat hesaplaması sadece yeni fiyat/oran geldiyse yapılmalı
            if (dto.Amount.HasValue || dto.DiscountRate.HasValue)
            {
                training.CurrentAmount = (dto.Amount ?? training.Amount) - ((dto.Amount ?? training.Amount) * (dto.DiscountRate ?? training.DiscountRate) / 100);
            }

            trainingRepository.Update(training);
        }

        // 2. REORDER (SIRALAMA) İŞLEMİ
        // Eğer ContentOrderIds doluysa sıralama işlemini yap
        if (dto.ContentOrderIds != null && dto.ContentOrderIds.Any())
        {

            int sectionOrder = 0;
            foreach (var sectionDto in dto.ContentOrderIds)
            {
                if (sectionDto == null) continue;

                // Bölüm (Section) Sırasını Güncelle
                var trainingSection = await trainingSectionRepository.GetAsync(x => x.Id == sectionDto.SectionId);
                if (trainingSection != null)
                {
                    trainingSection.RowNumber = ++sectionOrder;
                    trainingSection.UpdateDate = DateTime.UtcNow;
                    trainingSection.UpdateUserId = GetCurrentUserId();
                    trainingSectionRepository.Update(trainingSection);

                    // İçerik (Content) Sırasını Güncelle
                    if (sectionDto.Contents != null && sectionDto.Contents.Any())
                    {
                        var sectionContents = await trainingContentRepository.GetListAsync(c => c.TrainingSectionId == sectionDto.SectionId);
                        int contentOrder = 0;

                        foreach (var contentDto in sectionDto.Contents)
                        {
                            var existingContent = sectionContents.FirstOrDefault(c => c.Id == contentDto.ContentId);
                            if (existingContent != null)
                            {
                                existingContent.OrderId = ++contentOrder;
                                existingContent.UpdateDate = DateTime.UtcNow;
                                existingContent.UpdateUserId = GetCurrentUserId();
                            }
                        }
                        // Toplu update (EfCore tracking sayesinde SaveChanges ile yansır ama explicit update iyidir)
                        trainingContentRepository.UpdateList(sectionContents.ToList());
                    }
                }
            }
        }

        await hrUnitOfWork.SaveChangesAsync(cancellationToken);

        return ProduceSuccessResponse(new CommonResponse
        {
            Message = "Güncelleme işlemi başarıyla tamamlandı.",
            Code = StatusCodes.Status200OK,
            Result = true
        });

        #region oldCode
        //if (training is not null)
        //{

        //    var mapperData = mapper.Map(dto, training);
        //    training.CurrentAmount = dto.Amount - (dto.Amount * dto.DiscountRate / 100);
        //    trainingRepository.Update(mapperData);
        //}

        //int sectionOrder = 0;
        //foreach (var section in dto.ContentOrderIds)
        //{
        //    var trainingSection = await trainingSectionRepository.GetAsync(x => x.Id == section.SectionId);
        //    if (trainingSection is not null)
        //    {
        //        trainingSection.RowNumber = ++sectionOrder;
        //        trainingSection.UpdateDate = DateTime.UtcNow;
        //        trainingSection.UpdateUserId = GetCurrentUserId();
        //        trainingSectionRepository.Update(trainingSection);
        //    }
        //}

        //#region **TrainingContent_Section Update
        //if (dto.ContentOrderIds != null && dto.ContentOrderIds.Any())
        //{
        //    foreach (var section in dto.ContentOrderIds)
        //    {
        //        var sectionContents = await trainingContentRepository.GetListAsync(c => c.TrainingSectionId == section.SectionId);
        //        int newOrder = 0;

        //        foreach (var content in section.Contents)
        //        {
        //            var existingContent = sectionContents.FirstOrDefault(c => c.Id == content.ContentId);
        //            if (existingContent != null)
        //            {
        //                existingContent.OrderId = ++newOrder;
        //                existingContent.UpdateDate = DateTime.UtcNow;
        //                existingContent.UpdateUserId = this.GetCurrentUserId();
        //            }
        //        }
        //        trainingContentRepository.UpdateList(sectionContents.ToList());

        //    }
        //}
        //#endregion
        //await hrUnitOfWork.SaveChangesAsync(cancellationToken);

        //return ProduceSuccessResponse(new CommonResponse
        //{
        //    Message = "Success",
        //    Code = StatusCodes.Status200OK,
        //    Result = true
        //}); 
        #endregion
    }
    public async Task<Response<CommonResponse>> DeleteTrainingAsync(long id, CancellationToken cancellationToken = default)
    {
        var trainingDto = await trainingRepository.GetAsync(predicate: t => t.Id == id, selector: s => mapper.Map<DeleteTrainingDto>(s));
        if (ValidationHelper.RuleBasedValidator<DeleteTrainingDto>(trainingDto, typeof(IExistTrainingBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
            return cBasedValidResult.SendResponse<CommonResponse>();

        var trainingEntity = await trainingRepository.GetAsync(predicate: p => p.Id == id);
        trainingEntity.IsDelete = true;
        trainingEntity.DeleteDate = DateTime.UtcNow;
        trainingEntity.DeleteUserId = this.GetCurrentUserId();

        trainingRepository.Update(trainingEntity);
        await hrUnitOfWork.SaveChangesAsync(cancellationToken);
        return ProduceSuccessResponse(new CommonResponse
        {
            Message = "Success",
            Code = StatusCodes.Status200OK,
            Result = true
        });
    }
    public async Task<Response<IEnumerable<GetTrainingDto>>> GetTrainingListAsync()
    {
        var trainingListDto = await trainingRepository.GetListAsync(predicate: p => p.IsDelete != true,
                                                                    include: i => i.Include(s => s.TrainingCategory)
                                                                    .Include(s => s.Instructor)
                                                                    .Include(s => s.TrainingLanguage)
                                                                    .Include(s => s.TimeUnit)
                                                                    .Include(s => s.TrainingLevel)
                                                                    .Include(s => s.TrainingStatus)
                                                                    .Include(s => s.EducationLevel)
                                                                    .Include(s => s.ForWhom)
                                                                    .Include(s => s.Precondition)
                                                                    .Include(s => s.PriceTier)
                                                                    .Include(s => s.TrainingType)
                                                                    .Include(s => s.TrainingSections)
                                                                        .ThenInclude(d => d.TrainingContents)
                                                                            .ThenInclude(e => e.ContentType)
                                                                    .Include(s => s.TrainingSections)
                                                                        .ThenInclude(d => d.TrainingContents)
                                                                            .ThenInclude(e => e.ContentLibraries) // **ContentLibrary Eklendi**
                                                                    .Include(s => s.WhatYouWillLearns),
                                                                            selector: s => mapper.Map<GetTrainingDto>(s));
        return ProduceSuccessResponse(trainingListDto);

    }
    public async Task<Response<GetTrainingDto>> GetTrainingByIdAsync(long id)
    {
        var training = await trainingRepository.GetAsync(
            predicate: p => p.Id == id && (p.IsDelete == false || p.IsDelete == null),
            include: i => i.Include(s => s.TrainingCategory)
                           .Include(s => s.Instructor)
                           .Include(s => s.TrainingLanguage)
                           .Include(s => s.TimeUnit)
                           .Include(s => s.TrainingLevel)
                           .Include(s => s.TrainingStatus)
                           .Include(s => s.EducationLevel)
                           .Include(s => s.ForWhom)
                           .Include(s => s.Precondition)
                           .Include(s => s.PriceTier)
                           .Include(s => s.TrainingType)
                           .Include(s => s.TrainingSections.Where(t => t.IsDelete == false || t.IsDelete == null))
                               .ThenInclude(section => section.TrainingContents.Where(c => c.IsDelete == false || c.IsDelete == null))
                                   .ThenInclude(content => content.ContentType)
                           .Include(s => s.TrainingSections)
                               .ThenInclude(section => section.TrainingContents)
                                   .ThenInclude(content => content.ContentLibraries)
                           .Include(s => s.WhatYouWillLearns.Where(w => w.IsDelete == false || w.IsDelete == null))
        );

        var trainingDto = mapper.Map<GetTrainingDto>(training);
        return ProduceSuccessResponse(trainingDto);
    }
    public async Task<Response<CommonResponse>> ReorderTrainingContentAsync(ReorderTrainingContentDto dto, CancellationToken cancellationToken = default)
    {
        // 1. Eğitim var mı kontrolü (Opsiyonel ama güvenli)
        var exists = await trainingRepository.ExistsAsync(t => t.Id == dto.TrainingId);
        if (!exists)
        {
            return ProduceFailResponse<CommonResponse>("Eğitim bulunamadı.", StatusCodes.Status404NotFound);
        }

        // 2. REORDER (SIRALAMA) İŞLEMİ
        if (dto.ContentOrderIds != null && dto.ContentOrderIds.Any())
        {
            int sectionOrder = 0;
            foreach (var sectionDto in dto.ContentOrderIds)
            {
                if (sectionDto == null) continue;

                // Bölüm (Section) Sırasını Güncelle
                var trainingSection = await trainingSectionRepository.GetAsync(x => x.Id == sectionDto.SectionId);

                // Güvenlik Kontrolü: Bu section gerçekten bu eğitime mi ait?
                if (trainingSection != null && trainingSection.TrainingId == dto.TrainingId)
                {
                    trainingSection.RowNumber = ++sectionOrder;
                    trainingSection.UpdateDate = DateTime.UtcNow;
                    trainingSection.UpdateUserId = GetCurrentUserId();
                    trainingSectionRepository.Update(trainingSection);

                    // İçerik (Content) Sırasını Güncelle
                    if (sectionDto.Contents != null && sectionDto.Contents.Any())
                    {
                        // Section'a ait tüm içerikleri çek
                        var sectionContents = await trainingContentRepository.GetListAsync(c => c.TrainingSectionId == sectionDto.SectionId);
                        int contentOrder = 0;

                        foreach (var contentDto in sectionDto.Contents)
                        {
                            var existingContent = sectionContents.FirstOrDefault(c => c.Id == contentDto.ContentId);
                            if (existingContent != null)
                            {
                                existingContent.OrderId = ++contentOrder;
                                existingContent.UpdateDate = DateTime.UtcNow;
                                existingContent.UpdateUserId = GetCurrentUserId();
                            }
                        }
                        // Toplu update
                        trainingContentRepository.UpdateList(sectionContents.ToList());
                    }
                }
            }
        }

        await hrUnitOfWork.SaveChangesAsync(cancellationToken);

        return ProduceSuccessResponse(new CommonResponse
        {
            Message = "İçerik sıralaması başarıyla güncellendi.",
            Code = StatusCodes.Status200OK,
            Result = true
        });
    }
    public async Task<Response<IEnumerable<GetMyTrainingDto>>> GetMyTrainingsAsync(CancellationToken cancellationToken = default)
    {
        var currentUserId = GetCurrentUserId();
        string defaultTrainingImage = "/images/default-course-cover.png";

        // 1. Kullanıcıya atanmış eğitimleri bul
        // Include zincirine dikkat: CurrAccTraining üzerinden Training'e ulaşıyoruz ama
        // diğer detayları aşağıda TrainingRepository'den çekeceğiz, burayı şişirmeyelim.
        var assignedList = await currAccTrainingUserRepository.GetListAsync(
            predicate: x => x.UserId == currentUserId && x.IsActive == true,
            include: i => i.Include(x => x.CurrAccTrainings),
            cancellationToken: cancellationToken
        );

        if (!assignedList.Any())
        {
            return ProduceSuccessResponse(Enumerable.Empty<GetMyTrainingDto>());
        }

        var trainingIds = assignedList.Select(x => x.CurrAccTrainings.TrainingId).Distinct().ToList();

        // 2. Eğitim Detaylarını Çek (Eğitmen, Kategori, Bölümler, İçerikler)
        var trainings = await trainingRepository.GetListAsync(
            predicate: x => trainingIds.Contains(x.Id) && x.IsActive == true && x.IsDelete == false,
            include: i => i.Include(t => t.TrainingSections).ThenInclude(s => s.TrainingContents)
                           .Include(t => t.Instructor).ThenInclude(ins => ins.User) // Eğitmen ve User bilgisi
                           .Include(t => t.TrainingCategory), // Kategori bilgisi
            cancellationToken: cancellationToken
        );

        // 3. Logları Çek (İçerik ID havuzu üzerinden)
        var relevantContentIds = trainings
            .SelectMany(t => t.TrainingSections)
            .SelectMany(s => s.TrainingContents)
            .Where(c => c.IsDelete == false)
            .Select(c => c.Id)
            .ToList();

        var userLogs = await userContentsViewLogRepository.GetListAsync(
            predicate: x => x.CurrAccTrainingUser.UserId == currentUserId && relevantContentIds.Contains(x.TrainingContentId),
            cancellationToken: cancellationToken
        );

        // 4. Mapping
        var resultList = new List<GetMyTrainingDto>();

        foreach (var training in trainings)
        {
            var contentsOfThisTraining = training.TrainingSections
                                        .SelectMany(s => s.TrainingContents)
                                        .Where(c => c.IsDelete == false)
                                        .ToList();

            int totalCount = contentsOfThisTraining.Count;

            // Log Hesaplamaları
            var logsForThisTraining = userLogs.Where(l => contentsOfThisTraining.Any(c => c.Id == l.TrainingContentId)).ToList();
            int completedCount = logsForThisTraining.Count(l => l.IsCompleted == true);
            int percentage = totalCount > 0 ? (int)((double)completedCount / totalCount * 100) : 0;

            // Kaldığı Yer
            var lastLog = logsForThisTraining.OrderByDescending(l => l.UpdateDate ?? l.CreatedDate).FirstOrDefault();
            long? resumeContentId = lastLog?.TrainingContentId;
            if (resumeContentId == null && contentsOfThisTraining.Any())
            {
                resumeContentId = contentsOfThisTraining.OrderBy(c => c.OrderId).FirstOrDefault()?.Id;
            }

            // Atama Bilgisi
            var assignRecord = assignedList.FirstOrDefault(x => x.CurrAccTrainings.TrainingId == training.Id);

            // Eğitmen Adı Oluşturma
            string instructorFullName = "HrHub Instructor";
            if (training.Instructor?.User != null)
            {
                instructorFullName = $"{training.Instructor.User.Name} {training.Instructor.User.SurName}";
            }

            var now = DateTime.UtcNow;
            string status = "Active";

            if (assignRecord.StartDate.HasValue && assignRecord.StartDate > now)
            {
                status = "NotStarted"; // Buton pasif, "Başlamadı" yazısı
            }
            else if (assignRecord.DueDate.HasValue && assignRecord.DueDate < now)
            {
                // Burada bir kontrol daha lazım: Kullanıcı zaten talep göndermiş mi?
                // Şimdilik basitçe:
                status = "Expired"; // "Erişim Talebi Gönder" butonu aktif olur
            }

            resultList.Add(new GetMyTrainingDto
            {
                Id = training.Id,
                Title = training.Title,
                Description = training.Description,
                PicturePath = !string.IsNullOrEmpty(training.HeaderImage) ? training.HeaderImage : defaultTrainingImage,

                // YENİ ALANLAR
                CategoryName = training.TrainingCategory?.Title ?? "Genel",
                InstructorName = instructorFullName,
                InstructorTitle = training.Instructor?.Title,

                TotalContentCount = totalCount,
                CompletedContentCount = completedCount,
                ProgressPercentage = percentage > 100 ? 100 : percentage,
                IsCompleted = percentage >= 100,

                LastWatchedContentId = resumeContentId,
                LastAccessDate = lastLog?.UpdateDate ?? lastLog?.CreatedDate,
                AssignDate = assignRecord?.CreatedDate,
                DueDate = assignRecord?.DueDate,
                StartDate = assignRecord?.StartDate,
                AccessStatus = status
            });
        }

        return ProduceSuccessResponse(resultList.AsEnumerable());
    }
    public async Task<Response<TrainingDetailDto>> GetTrainingDetailForUserAsync(long trainingId)
    {
        var currentUserId = GetCurrentUserId();
        string defaultImage = "/images/default-course-cover.png";

        // 1. Önce Atama (Assignment) kaydını bul (Tarihler burada!)
        var assignment = await currAccTrainingUserRepository.GetAsync(
            predicate: x => x.UserId == currentUserId && x.CurrAccTrainings.TrainingId == trainingId && x.IsActive == true,
            include: i => i.Include(x => x.CurrAccTrainings)
        );

        if (assignment == null)
            return ProduceFailResponse<TrainingDetailDto>("Bu eğitime atamanız bulunmamaktadır.", HrStatusCodes.Status404NotFound);

        // 2. Business Rule Kontrolü (Senin standart ValidationHelper yapınla)
        var accessDto = new TrainingAccessCheckDto { StartDate = assignment.StartDate, DueDate = assignment.DueDate };
        var validationResult = ValidationHelper.RuleBasedValidator<TrainingAccessCheckDto>(accessDto, typeof(IBusinessRule));

        if (validationResult is ValidationResult res && !res.IsValid)
        {
            // Kurala takıldıysa (Tarih geçersizse) içeriği hiç çekmeden hata dön
            return res.SendResponse<TrainingDetailDto>();
        }

        // 1. EĞİTİMİ VE İLİŞKİLİ TÜM VERİLERİ TEK SORGUDA ÇEK
        var training = await trainingRepository.GetAsync<Training>(
            predicate: x => x.Id == trainingId && x.IsActive == true && x.IsDelete == false,
            include: i => i.Include(t => t.TrainingSections).ThenInclude(s => s.TrainingContents)
                           .Include(t => t.Instructor).ThenInclude(u => u.User)
                            .Include(t => t.TrainingLevel) 
                           ,
            cancellationToken: default
        );

        if (training == null)
            return ProduceFailResponse<TrainingDetailDto>("Eğitim bulunamadı.", StatusCodes.Status404NotFound);

        // 2. İÇERİK ID HAVUZUNU OLUŞTUR
        // (Logları filtrelemek için bu eğitimin tüm içerik ID'lerine ihtiyacımız var)
        var contentIds = training.TrainingSections
            .SelectMany(s => s.TrainingContents)
            .Where(c => c.IsDelete == false)
            .Select(c => c.Id)
            .ToList();

        // 3. KULLANICININ LOGLARINI ÇEK
        // Sadece bu eğitime ait ve bu kullanıcıya ait logları getir.
        var userLogs = await userContentsViewLogRepository.GetListAsync(
            predicate: x => x.CurrAccTrainingUser.UserId == currentUserId && contentIds.Contains(x.TrainingContentId)
        );

        // -- HESAPLAMALAR --

        // a. Tamamlanan İçerikler (Hızlı arama için HashSet kullanıyoruz)
        var completedContentIds = new HashSet<long>(
            userLogs.Where(l => l.IsCompleted).Select(l => l.TrainingContentId)
        );

        // b. Kaldığı Yer (Resume)
        // Loglarda en son güncellenen kaydı bul.
        var lastLog = userLogs.OrderByDescending(l => l.UpdateDate ?? l.CreatedDate).FirstOrDefault();
        long? resumeContentId = lastLog?.TrainingContentId;

        // Eğer hiç log yoksa (Kullanıcı hiç başlamadıysa), ilk sıradaki içeriği 'Başlangıç' olarak ayarla.
        if (resumeContentId == null && contentIds.Any())
        {
            var firstContent = training.TrainingSections
                .OrderBy(s => s.RowNumber)
                .SelectMany(s => s.TrainingContents.OrderBy(c => c.OrderId))
                .FirstOrDefault(c => c.IsDelete == false);

            resumeContentId = firstContent?.Id;
        }

        // c. Genel İlerleme Yüzdesi
        int totalCount = contentIds.Count;
        int completedCount = completedContentIds.Count;
        int percentage = totalCount > 0 ? (int)((double)completedCount / totalCount * 100) : 0;

        // 4. DTO MAPPING (Manuel Mapping - En Güvenlisi)
        var responseDto = new TrainingDetailDto
        {
            // Temel Bilgiler
            Id = training.Id,
            Title = training.Title,
            Description = training.Description,
            PicturePath = !string.IsNullOrEmpty(training.HeaderImage) ? training.HeaderImage : defaultImage,
            InstructorName = training.Instructor?.User != null
                ? $"{training.Instructor.User.Name} {training.Instructor.User.SurName}"
                : "HrHub Eğitmen",

            // İlerleme Bilgileri
            ProgressPercentage = percentage > 100 ? 100 : percentage,
            LastWatchedContentId = resumeContentId,

            // Genel Bakış Verileri (Tablo eşleşmelerine göre)
            LangCode = training.LangCode ?? "TR",
            // TrainingLevel navigation property'si varsa: training.TrainingLevel?.Name
            LevelName = "Genel Seviye",
            // Sertifika oranları 0'dan büyükse sertifika vardır
            HasCertificate = (training.CertificateOfAchievementRate > 0 || training.CertificateOfParticipationRate > 0),

            // Müfredat Ağacı Oluşturma
            Sections = training.TrainingSections
                .OrderBy(s => s.RowNumber) // Bölüm sırası
                .Select(s => new TrainingSectionForUserDto
                {
                    Id = s.Id,
                    Title = s.Title,
                    OrderId = s.RowNumber,
                    Contents = s.TrainingContents
                        .Where(c => c.IsDelete == false)
                        .OrderBy(c => c.OrderId) // İçerik sırası
                        .Select(c => new TrainingContentListItemDto
                        {
                            Id = c.Id,
                            Title = c.Title,
                            Time = c.Time != TimeSpan.Zero || c.Time != null
                                   ? (int)Math.Ceiling(c.Time.Value.TotalMinutes)
                                   : 0,
                            OrderId = c.OrderId ?? 0,
                            IsActive = c.IsActive,
                            IsCompleted = completedContentIds.Contains(c.Id)
                        }).ToList()
                }).ToList()
        };

        return ProduceSuccessResponse(responseDto);
    }

    /// <summary>
    /// Asynchronously retrieves the list of trainings given by the currently authenticated instructor.
    /// </summary>
    /// <remarks>Only trainings associated with the current user as an instructor are returned. Deleted
    /// trainings are excluded from the result.</remarks>
    /// <returns>A task that represents the asynchronous operation. The task result contains a response with an enumerable
    /// collection of training data transfer objects representing the trainings given by the current instructor. If the
    /// user is not registered as an instructor, the collection will be empty.</returns>
    public async Task<Response<IEnumerable<GetTrainingDto>>> GetMyGivenTrainingsAsync()
    {
        var currentUserId = GetCurrentUserId();

        // 1. Bu kullanıcı bir Eğitmen mi?
        var instructor = await instructorRepository.GetAsync(x => x.UserId == currentUserId);

        if (instructor == null)
        {
            // Eğer eğitmen kaydı yoksa boş liste dön (veya yetki hatası fırlat)
            return ProduceSuccessResponse(Enumerable.Empty<GetTrainingDto>());
        }

        // 2. Eğitmenin verdiği eğitimleri çek
        var trainings = await trainingRepository.GetListAsync(
            predicate: x => x.InstructorId == instructor.Id && x.IsDelete == false,
            include: i => i.Include(t => t.TrainingCategory)
                           .Include(t => t.TrainingStatus)
                           .Include(t => t.TrainingLevel)
                           .Include(t => t.TrainingLanguage)
        // İhtiyaç duyulan diğer include'lar...
        );

        // 3. Mapping
        var trainingDtos = mapper.Map<IEnumerable<GetTrainingDto>>(trainings);

        return ProduceSuccessResponse(trainingDtos);
    }

    public async Task<Response<List<TrainingViewCardDto>>> GetRecommendedTrainingsAsync()
    {
        long userId = GetCurrentUserId();
        // 1. Kullanıcının MEVCUT (Aldığı/Atanmış) eğitimlerini çekiyoruz.
        // Selector ile sadece ihtiyacımız olan ID'leri alıyoruz (Performans).
        var userHistory = await currAccTrainingUserRepository.GetListAsync(
            predicate: x => x.UserId == userId,
            selector: x => new { x.CurrAccTrainings.TrainingId, x.CurrAccTrainings.Training.CategoryId }
        );

        var ownedTrainingIds = userHistory.Select(x => x.TrainingId).ToList();

        // Kullanıcının en son aldığı eğitimin kategorisini buluyoruz (İlgi alanı tespiti)
        // Eğer hiç eğitimi yoksa 0 veya null gelir.
        var lastInteractedCategoryId = userHistory
            .OrderByDescending(x => x.TrainingId) // veya CreatedDate
            .Select(x => x.CategoryId)
            .FirstOrDefault();

        List<TrainingViewCardDto> recommendations = new();

        // 2. A PLANI: Kullanıcının ilgilendiği kategoriden henüz almadığı en yeni eğitimleri getir.
        if (lastInteractedCategoryId > 0)
        {
            var interestBased = await trainingRepository.GetPagedListAsync(
                predicate: x => x.IsActive &&
                                x.CategoryId == lastInteractedCategoryId &&
                                !ownedTrainingIds.Contains(x.Id),
                orderBy: q => q.OrderByDescending(x => x.CreatedDate),
                skip: 0,
                take: 10,
                selector: x => new TrainingViewCardDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    HeaderImage = x.HeaderImage,
                    CategoryTitle = x.TrainingCategory.Title,
                    InstructorTitle = x.Instructor.Title,
                    InstructorPicturePath = x.Instructor.PicturePath,
                    Amount = GetTrainingAmount(x),
                    CurrentAmount = GetTrainingCurrentAmount(x),
                    DiscountRate = GetTrainingDiscountRate(x),
                    TrainingLevelTitle = x.TrainingLevel.Title,
                    CreatedDate = x.CreatedDate
                    // Rating vb. gerekirse buraya eklenir
                }
            );

            recommendations.AddRange(interestBased);
        }

        // 3. B PLANI (FALLBACK): Eğer liste dolmadıysa (veya kullanıcı yeniyse), Son Eklenenleri getir.
        if (recommendations.Count < 10)
        {
            int neededCount = 10 - recommendations.Count;

            // Zaten listeye eklediklerimizin ID'lerini de hariç tutalım ki tekrar etmesin.
            var addedIds = recommendations.Select(r => r.Id).ToList();
            var excludeIds = ownedTrainingIds.Concat(addedIds).ToList();

            var newReleases = await trainingRepository.GetPagedListAsync(
                predicate: x => x.IsActive && !excludeIds.Contains(x.Id),
                orderBy: q => q.OrderByDescending(x => x.CreatedDate),
                skip: 0,
                take: neededCount,
                selector: x => new TrainingViewCardDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    HeaderImage = x.HeaderImage,
                    CategoryTitle = x.TrainingCategory.Title,
                    InstructorTitle = x.Instructor.Title,
                    InstructorPicturePath = x.Instructor.PicturePath,
                    Amount = GetTrainingAmount(x),
                    CurrentAmount = GetTrainingCurrentAmount(x),
                    DiscountRate = GetTrainingDiscountRate(x),
                    TrainingLevelTitle = x.TrainingLevel.Title,
                    CreatedDate = x.CreatedDate
                }
            );

            recommendations.AddRange(newReleases);
        }

        return ProduceSuccessResponse(recommendations);
    }

    private decimal? GetTrainingAmount(Training training)
    {
        if (training.TrainingAmounts == null || !training.TrainingAmounts.Any())
            return null;

        return training.TrainingAmounts
            .OrderBy(w => w.AmountPerLicence)
            .FirstOrDefault()?
            .AmountPerLicence;
    }

    private decimal GetTrainingDiscountRate(Training training)
    {
        if (training.TrainingAmounts == null || !training.TrainingAmounts.Any())
            return 0;

        return training.TrainingAmounts
            .OrderBy(w => w.AmountPerLicence)
            .FirstOrDefault()?
            .DiscountRate ?? 0;
    }

    private decimal GetTrainingCurrentAmount(Training training)
    {
        if (training.TrainingAmounts == null || !training.TrainingAmounts.Any())
            return 0;

        return training.TrainingAmounts
            .OrderBy(w => w.AmountPerLicence)
            .FirstOrDefault()?
            .AmountPerLicence ?? 0;
    }
}
