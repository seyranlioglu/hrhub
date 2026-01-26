using AutoMapper;
using FluentValidation.Results;
using HrHub.Abstraction.BusinessRules;
using HrHub.Abstraction.Consts;
using HrHub.Abstraction.Contracts.Dtos.TrainingDtos;
using HrHub.Abstraction.Data.Collections;
using HrHub.Abstraction.Data.EfCore.Repository;
using HrHub.Abstraction.Extensions;
using HrHub.Abstraction.Result;
using HrHub.Abstraction.Settings;
using HrHub.Abstraction.StatusCodes;
using HrHub.Application.BusinessRules.TrainingBusinessRules;
using HrHub.Core.Base;
using HrHub.Core.Data.Repository;
using HrHub.Core.Helpers;
using HrHub.Domain.Contracts.Dtos.DashboardDtos;
using HrHub.Domain.Contracts.Dtos.TrainingDtos;
using HrHub.Domain.Contracts.Enums;
using HrHub.Domain.Contracts.Responses.CommonResponse;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.Repositories.Abstract;
using HrHub.Infrastructre.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HrHub.Application.Managers.Trainings
{
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
        private readonly Repository<TrainingCategory> trainingCategoryRepository;
        private readonly Repository<TrainingLevel> trainingLevelRepository;
        private readonly Repository<TrainingLanguage> trainingLanguageRepository;
        private readonly Repository<FavoriteTraining> favoriteTrainingRepository;
        private readonly Repository<TrainingReview> trainingReviewRepository;

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
            trainingCategoryRepository = hrUnitOfWork.CreateRepository<TrainingCategory>();
            trainingLevelRepository = hrUnitOfWork.CreateRepository<TrainingLevel>();
            trainingLanguageRepository = hrUnitOfWork.CreateRepository<TrainingLanguage>();
            favoriteTrainingRepository = hrUnitOfWork.CreateRepository<FavoriteTraining>();
            trainingReviewRepository = hrUnitOfWork.CreateRepository<TrainingReview>();
            this.mapper = mapper;
            this.currAccTrainingUserRepository = currAccTrainingUserRepository;
            this.userContentsViewLogRepository = userContentsViewLogRepository;
            this.instructorRepository = instructorRepository;
        }

        // =================================================================================================
        // CRUD İŞLEMLERİ
        // =================================================================================================

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
            trainingEntity.EducationLevelId = data.EducationLevelId == 0 ? (long?)null : data.EducationLevelId;
            trainingEntity.PriceTierId = data.PriceTierId == 0 ? (long?)null : data.PriceTierId;
            trainingEntity.TrainingLanguageId = data.TrainingLanguageId;
            trainingEntity.CompletionTime = data.CompletionTime;

            trainingEntity.TrainingStatusId = await trainingStatuRepository.GetAsync(
                predicate: p => p.Code == TrainingStatuConst.Preparing,
                selector: s => s.Id);

            var result = await trainingRepository.AddAndReturnAsync(trainingEntity);
            await hrUnitOfWork.SaveChangesAsync();

            return ProduceSuccessResponse(new ReturnIdResponse { Id = result.Id });
        }

        public async Task<Response<CommonResponse>> UpdateTrainingAsync(UpdateTrainingDto dto, CancellationToken cancellationToken = default)
        {
            var training = await trainingRepository.GetAsync(predicate: t => t.Id == dto.Id);
            if (training == null) return ProduceFailResponse<CommonResponse>("Eğitim bulunamadı.", StatusCodes.Status404NotFound);

            bool isGeneralUpdate = !string.IsNullOrEmpty(dto.Title) || !string.IsNullOrEmpty(dto.Description);

            if (isGeneralUpdate)
            {
                if (ValidationHelper.RuleBasedValidator<UpdateTrainingDto>(dto, typeof(IUpdateTrainingBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                    return cBasedValidResult.SendResponse<CommonResponse>();

                mapper.Map(dto, training);
                trainingRepository.Update(training);
            }

            if (dto.ContentOrderIds != null && dto.ContentOrderIds.Any())
            {
                int sectionOrder = 0;
                foreach (var sectionDto in dto.ContentOrderIds)
                {
                    if (sectionDto == null) continue;
                    var trainingSection = await trainingSectionRepository.GetAsync(x => x.Id == sectionDto.SectionId);
                    if (trainingSection != null)
                    {
                        trainingSection.RowNumber = ++sectionOrder;
                        trainingSection.UpdateDate = DateTime.UtcNow;
                        trainingSection.UpdateUserId = GetCurrentUserId();
                        trainingSectionRepository.Update(trainingSection);

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
                            trainingContentRepository.UpdateList(sectionContents.ToList());
                        }
                    }
                }
            }

            await hrUnitOfWork.SaveChangesAsync(cancellationToken);
            return ProduceSuccessResponse(new CommonResponse { Message = "Güncelleme işlemi başarıyla tamamlandı.", Code = StatusCodes.Status200OK, Result = true });
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
            return ProduceSuccessResponse(new CommonResponse { Message = "Success", Code = StatusCodes.Status200OK, Result = true });
        }

        // =================================================================================================
        // LİSTELEME VE DETAY
        // =================================================================================================

        public async Task<Response<CommonResponse>> ReorderTrainingContentAsync(ReorderTrainingContentDto dto, CancellationToken cancellationToken = default)
        {
            var exists = await trainingRepository.ExistsAsync(t => t.Id == dto.TrainingId);
            if (!exists) return ProduceFailResponse<CommonResponse>("Eğitim bulunamadı.", StatusCodes.Status404NotFound);

            if (dto.ContentOrderIds != null && dto.ContentOrderIds.Any())
            {
                int sectionOrder = 0;
                foreach (var sectionDto in dto.ContentOrderIds)
                {
                    if (sectionDto == null) continue;
                    var trainingSection = await trainingSectionRepository.GetAsync(x => x.Id == sectionDto.SectionId);
                    if (trainingSection != null && trainingSection.TrainingId == dto.TrainingId)
                    {
                        trainingSection.RowNumber = ++sectionOrder;
                        trainingSection.UpdateDate = DateTime.UtcNow;
                        trainingSection.UpdateUserId = GetCurrentUserId();
                        trainingSectionRepository.Update(trainingSection);

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
                            trainingContentRepository.UpdateList(sectionContents.ToList());
                        }
                    }
                }
            }
            await hrUnitOfWork.SaveChangesAsync(cancellationToken);
            return ProduceSuccessResponse(new CommonResponse { Message = "İçerik sıralaması başarıyla güncellendi.", Code = StatusCodes.Status200OK, Result = true });
        }

        public async Task<Response<TrainingDetailDto>> GetTrainingDetailForUserAsync(long trainingId)
        {
            var currentUserId = GetCurrentUserId();
            string defaultImage = "/images/default-course-cover.png";

            var assignment = await currAccTrainingUserRepository.GetAsync(
                predicate: x => x.UserId == currentUserId && x.CurrAccTrainings.TrainingId == trainingId && x.IsActive == true,
                include: i => i.Include(x => x.CurrAccTrainings)
            );

            if (assignment == null) return ProduceFailResponse<TrainingDetailDto>("Bu eğitime atamanız bulunmamaktadır.", HrStatusCodes.Status404NotFound);

            var accessDto = new TrainingAccessCheckDto { StartDate = assignment.StartDate, DueDate = assignment.DueDate };
            var validationResult = ValidationHelper.RuleBasedValidator<TrainingAccessCheckDto>(accessDto, typeof(IBusinessRule));
            if (validationResult is ValidationResult res && !res.IsValid) return res.SendResponse<TrainingDetailDto>();

            var training = await trainingRepository.GetAsync<Training>(
                predicate: x => x.Id == trainingId && x.IsActive == true && x.IsDelete == false,
                include: i => i.Include(t => t.TrainingSections).ThenInclude(s => s.TrainingContents)
                               .Include(t => t.Instructor).ThenInclude(u => u.User)
                               .Include(t => t.TrainingLevel)
                               .Include(t => t.PriceTier).ThenInclude(pt => pt.Details) // Fiyat detayları
                               .Include(t => t.PriceTier).ThenInclude(pt => pt.CampaignPriceTiers).ThenInclude(cpt => cpt.Campaign) // Kampanyalar
            );

            if (training == null) return ProduceFailResponse<TrainingDetailDto>("Eğitim bulunamadı.", StatusCodes.Status404NotFound);

            var contentIds = training.TrainingSections.SelectMany(s => s.TrainingContents).Where(c => c.IsDelete == false).Select(c => c.Id).ToList();
            var userLogs = await userContentsViewLogRepository.GetListAsync(
                predicate: x => x.CurrAccTrainingUser.UserId == currentUserId && contentIds.Contains(x.TrainingContentId)
            );

            var completedContentIds = new HashSet<long>(userLogs.Where(l => l.IsCompleted).Select(l => l.TrainingContentId));
            var lastLog = userLogs.OrderByDescending(l => l.UpdateDate ?? l.CreatedDate).FirstOrDefault();
            long? resumeContentId = lastLog?.TrainingContentId;

            if (resumeContentId == null && contentIds.Any())
            {
                var firstContent = training.TrainingSections.OrderBy(s => s.RowNumber)
                    .SelectMany(s => s.TrainingContents.OrderBy(c => c.OrderId)).FirstOrDefault(c => c.IsDelete == false);
                resumeContentId = firstContent?.Id;
            }

            int totalCount = contentIds.Count;
            int completedCount = completedContentIds.Count;
            int percentage = totalCount > 0 ? (int)((double)completedCount / totalCount * 100) : 0;

            var responseDto = new TrainingDetailDto
            {
                Id = training.Id,
                Title = training.Title,
                Description = training.Description,
                PicturePath = !string.IsNullOrEmpty(training.HeaderImage) ? training.HeaderImage : defaultImage,
                InstructorName = training.Instructor?.User != null ? $"{training.Instructor.User.Name} {training.Instructor.User.SurName}" : "HrHub Eğitmen",
                ProgressPercentage = percentage > 100 ? 100 : percentage,
                LastWatchedContentId = resumeContentId,
                LangCode = training.LangCode ?? "TR",
                LevelName = "Genel Seviye",
                HasCertificate = (training.CertificateOfAchievementRate > 0 || training.CertificateOfParticipationRate > 0),
                Sections = training.TrainingSections.OrderBy(s => s.RowNumber).Select(s => new TrainingSectionForUserDto
                {
                    Id = s.Id,
                    Title = s.Title,
                    OrderId = s.RowNumber,
                    Contents = s.TrainingContents.Where(c => c.IsDelete == false).OrderBy(c => c.OrderId).Select(c => new TrainingContentListItemDto
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Time = c.Time != null ? (int)Math.Ceiling(c.Time.Value.TotalMinutes) : 0,
                        OrderId = c.OrderId ?? 0,
                        IsActive = c.IsActive,
                        IsCompleted = completedContentIds.Contains(c.Id)
                    }).ToList()
                }).ToList()
            };

            return ProduceSuccessResponse(responseDto);
        }

        public async Task<Response<IEnumerable<GetTrainingDto>>> GetMyGivenTrainingsAsync()
        {
            var currentUserId = GetCurrentUserId();
            var instructor = await instructorRepository.GetAsync(x => x.UserId == currentUserId);
            if (instructor == null) return ProduceSuccessResponse(Enumerable.Empty<GetTrainingDto>());

            var trainings = await trainingRepository.GetListAsync(
                predicate: x => x.InstructorId == instructor.Id && x.IsDelete == false,
                include: i => i.Include(t => t.TrainingCategory)
                               .Include(t => t.TrainingStatus)
                               .Include(t => t.TrainingLevel)
                               .Include(t => t.TrainingLanguage)
                               .Include(t => t.PriceTier).ThenInclude(pt => pt.Details)
                               .Include(t => t.PriceTier).ThenInclude(pt => pt.CampaignPriceTiers).ThenInclude(cpt => cpt.Campaign)
            );

            var trainingDtos = new List<GetTrainingDto>();
            foreach (var training in trainings)
            {
                var dto = mapper.Map<GetTrainingDto>(training);
                var priceInfo = CalculatePricing(training);
                dto.Amount = priceInfo.Amount;
                dto.CurrentAmount = priceInfo.CurrentAmount;
                dto.DiscountRate = priceInfo.DiscountRate;
                trainingDtos.Add(dto);
            }

            return ProduceSuccessResponse(trainingDtos.AsEnumerable());
        }

        public async Task<Response<List<TrainingViewCardDto>>> GetRecommendedTrainingsAsync()
        {
            try
            {
                long userId = GetCurrentUserId();
                var userHistory = await currAccTrainingUserRepository.GetListAsync(predicate: x => x.UserId == userId, selector: x => new { TrainingId = x.CurrAccTrainings != null ? x.CurrAccTrainings.TrainingId : 0, CategoryId = (x.CurrAccTrainings != null && x.CurrAccTrainings.Training != null) ? x.CurrAccTrainings.Training.CategoryId : (long?)null });
                var ownedTrainingIds = userHistory.Select(x => (long)x.TrainingId).ToList();
                var lastInteractedCategoryId = userHistory.Where(x => x.CategoryId != null && x.CategoryId > 0).OrderByDescending(x => x.TrainingId).Select(x => x.CategoryId).FirstOrDefault();

                List<TrainingViewCardDto> finalResult = new();

                var query = trainingRepository.GetQuery(
                    include: i => i.Include(t => t.TrainingCategory)
                                   .Include(t => t.Instructor).ThenInclude(u => u.User)
                                   .Include(t => t.TrainingLevel)
                                   .Include(t => t.PriceTier).ThenInclude(pt => pt.Details)
                                   .Include(t => t.PriceTier).ThenInclude(pt => pt.CampaignPriceTiers).ThenInclude(cpt => cpt.Campaign)
                                   .Include(t => t.TrainingReviews)
                                   .Include(t => t.TrainingSections).ThenInclude(s => s.TrainingContents)
                );

                if (lastInteractedCategoryId.HasValue && lastInteractedCategoryId.Value > 0)
                {
                    var interestBased = await query.Where(x => x.IsActive == true && x.IsDelete != true && x.CategoryId == lastInteractedCategoryId && !ownedTrainingIds.Contains(x.Id))
                        .OrderByDescending(x => x.CreatedDate).Take(10).ToListAsync();
                    finalResult.AddRange(MapToCardDto(interestBased));
                }

                if (finalResult.Count < 10)
                {
                    int needed = 10 - finalResult.Count;
                    var existingIds = finalResult.Select(r => r.Id).ToList();
                    existingIds.AddRange(ownedTrainingIds);
                    var newReleases = await query.Where(x => x.IsActive == true && x.IsDelete != true && !existingIds.Contains(x.Id))
                        .OrderByDescending(x => x.CreatedDate).Take(needed).ToListAsync();
                    finalResult.AddRange(MapToCardDto(newReleases));
                }

                return ProduceSuccessResponse(finalResult);
            }
            catch (Exception ex) { return ProduceSuccessResponse(new List<TrainingViewCardDto>()); }
        }

        public async Task<Response<IEnumerable<GetTrainingDto>>> GetTrainingListAsync()
        {
            var trainings = await trainingRepository.GetListAsync(predicate: p => p.IsDelete != true,
                include: i => i.Include(s => s.TrainingCategory).Include(s => s.Instructor).Include(s => s.TrainingLanguage).Include(s => s.TimeUnit)
                               .Include(s => s.TrainingLevel).Include(s => s.TrainingStatus).Include(s => s.EducationLevel).Include(s => s.ForWhom)
                               .Include(s => s.Precondition).Include(s => s.TrainingType)
                               .Include(s => s.PriceTier).ThenInclude(pt => pt.Details)
                               .Include(s => s.PriceTier).ThenInclude(pt => pt.CampaignPriceTiers).ThenInclude(cpt => cpt.Campaign)
                               .Include(s => s.TrainingSections).ThenInclude(d => d.TrainingContents).ThenInclude(e => e.ContentType)
                               .Include(s => s.TrainingSections).ThenInclude(d => d.TrainingContents).ThenInclude(e => e.ContentLibraries)
                               .Include(s => s.WhatYouWillLearns)
            );

            var dtoList = new List<GetTrainingDto>();
            foreach (var t in trainings)
            {
                var dto = mapper.Map<GetTrainingDto>(t);
                var priceInfo = CalculatePricing(t);
                dto.Amount = priceInfo.Amount;
                dto.CurrentAmount = priceInfo.CurrentAmount;
                dto.DiscountRate = priceInfo.DiscountRate;
                dtoList.Add(dto);
            }
            return ProduceSuccessResponse(dtoList.AsEnumerable());
        }

        public async Task<Response<GetTrainingDto>> GetTrainingByIdAsync(long id)
        {
            var training = await trainingRepository.GetAsync(
                predicate: p => p.Id == id && (p.IsDelete == false || p.IsDelete == null),
                include: i => i.Include(s => s.TrainingCategory).Include(s => s.Instructor).Include(s => s.TrainingLanguage)
                               .Include(s => s.TimeUnit).Include(s => s.TrainingLevel).Include(s => s.TrainingStatus)
                               .Include(s => s.EducationLevel).Include(s => s.ForWhom).Include(s => s.Precondition)
                               .Include(s => s.PriceTier).ThenInclude(pt => pt.Details)
                               .Include(s => s.PriceTier).ThenInclude(pt => pt.CampaignPriceTiers).ThenInclude(cpt => cpt.Campaign)
                               .Include(s => s.TrainingType)
                               .Include(s => s.TrainingSections.Where(t => t.IsDelete == false || t.IsDelete == null))
                                   .ThenInclude(section => section.TrainingContents.Where(c => c.IsDelete == false || c.IsDelete == null))
                                       .ThenInclude(content => content.ContentType)
                               .Include(s => s.TrainingSections).ThenInclude(section => section.TrainingContents).ThenInclude(content => content.ContentLibraries)
                               .Include(s => s.WhatYouWillLearns.Where(w => w.IsDelete == false || w.IsDelete == null))
            );

            if (training == null) return ProduceFailResponse<GetTrainingDto>("Eğitim bulunamadı.", StatusCodes.Status404NotFound);

            var trainingDto = mapper.Map<GetTrainingDto>(training);
            var priceInfo = CalculatePricing(training);

            trainingDto.Amount = priceInfo.Amount;
            trainingDto.CurrentAmount = priceInfo.CurrentAmount;
            trainingDto.DiscountRate = priceInfo.DiscountRate;

            if (training.PriceTier != null)
            {
                trainingDto.PriceTierCode = training.PriceTier.Code;
                trainingDto.PriceTierTitle = training.PriceTier.Title;
                trainingDto.PriceTierDescription = training.PriceTier.Description;
            }
            return ProduceSuccessResponse(trainingDto);
        }

        public async Task<Response<PagedList<TrainingListItemDto>>> GetAdvancedTrainingListAsync(SearchTrainingRequestDto request)
        {
            try
            {
                long currentUserId = IsAuthenticate() ? GetCurrentUserId() : 0;

                // 1. QUERY OLUŞTURMA (Tüm Include'lar ile)
                var query = trainingRepository.GetQuery(
                    include: i => i.Include(w => w.TrainingStatus)
                                   .Include(w => w.Instructor).ThenInclude(u => u.User)
                                   .Include(w => w.TrainingCategory)
                                   .Include(w => w.TrainingLevel) // Seviye bilgisini çekmek için önemli
                                                                  // Fiyat ve Kampanya
                                   .Include(w => w.PriceTier).ThenInclude(pt => pt.Details)
                                   .Include(w => w.PriceTier).ThenInclude(pt => pt.CampaignPriceTiers).ThenInclude(cpt => cpt.Campaign)
                                   // İçerik İstatistikleri için
                                   .Include(w => w.TrainingSections).ThenInclude(s => s.TrainingContents)
                                   // Liste detayları ve puanlar için
                                   .Include(w => w.WhatYouWillLearns)
                                   .Include(w => w.TrainingReviews)
                );

                // 2. TEMEL FİLTRELER
                query = query.Where(t => t.IsActive == true
                                      && t.TrainingStatus.Code == TrainingStatuConst.Published
                                      && (t.IsDelete == false || t.IsDelete == null));

                // 3. GELİŞMİŞ FİLTRELER (Request'ten gelenler)
                if (request.OnlyPrivate)
                    query = query.Where(t => t.IsPrivate == true);

                if (!string.IsNullOrWhiteSpace(request.SearchText) && request.SearchText.Length >= 2)
                {
                    string text = request.SearchText.Trim().ToLower();
                    query = query.Where(t => t.Title.ToLower().Contains(text)
                                          || (t.Description != null && t.Description.ToLower().Contains(text))
                                          || (t.Instructor != null && t.Instructor.User != null &&
                                             (t.Instructor.User.Name.ToLower().Contains(text) || t.Instructor.User.SurName.ToLower().Contains(text))));
                }

                if (request.CategoryIds != null && request.CategoryIds.Any())
                    query = query.Where(t => t.CategoryId.HasValue && request.CategoryIds.Contains(t.CategoryId.Value));

                if (request.LevelIds != null && request.LevelIds.Any())
                    query = query.Where(t => t.TrainingLevelId.HasValue && request.LevelIds.Contains(t.TrainingLevelId.Value));

                if (request.LanguageIds != null && request.LanguageIds.Any())
                    query = query.Where(t => t.TrainingLanguageId.HasValue && request.LanguageIds.Contains(t.TrainingLanguageId.Value));

                if (request.InstructorIds != null && request.InstructorIds.Any())
                    query = query.Where(t => t.InstructorId.HasValue && request.InstructorIds.Contains(t.InstructorId.Value));

                // Rating Filtresi (Database tarafında hesaplama bazen sorun olabilir ama deneyelim)
                if (request.MinRating.HasValue && request.MinRating > 0)
                {
                    // Not: EF Core bazen karmaşık Average sorgularını SQL'e çeviremeyebilir.
                    // Eğer hata alırsan bu filtreyi bellekte (Mapledikten sonra) yapmamız gerekebilir.
                    // Şimdilik senin yapını koruyorum.
                    query = query.Where(t => t.TrainingReviews.Any() &&
                                             t.TrainingReviews.Where(r => r.IsActive == true && r.IsApproved == true)
                                                              .Average(r => r.Rating) >= request.MinRating.Value);
                }

                // 4. SIRALAMA VE SAYFALAMA
                if (string.IsNullOrEmpty(request.SortBy))
                {
                    query = query.OrderByDescending(t => t.CreatedDate);
                }
                else
                {
                    switch (request.SortBy.ToLower())
                    {
                        case "price_asc":
                            // Fiyat Tier tablosunda olduğu için Join ile sıralama yapmak gerekebilir
                            // Basit çözüm: Minimum fiyata göre sırala
                            query = query.OrderBy(t => t.PriceTier.Details.Min(d => d.Amount));
                            break;

                        case "price_desc":
                            query = query.OrderByDescending(t => t.PriceTier.Details.Min(d => d.Amount));
                            break;

                        case "rating":
                            // İlişkisel tablodan ortalama almak SQL tarafında bazen yavaş olabilir ama doğrusu budur:
                            query = query.OrderByDescending(t => t.TrainingReviews.Any()
                                ? t.TrainingReviews.Average(r => r.Rating)
                                : 0);
                            break;

                        case "popular":
                            // Öğrenci sayısına veya yorum sayısına göre
                            query = query.OrderByDescending(t => t.TrainingReviews.Count());
                            break;

                        case "oldest":
                            query = query.OrderBy(t => t.CreatedDate);
                            break;

                        default: // "newest"
                            query = query.OrderByDescending(t => t.CreatedDate);
                            break;
                    }
                }

                int totalRecords = await query.CountAsync();

                var pagedEntities = await query
                    .Skip(request.PageIndex * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync();

                // 5. MAPLEME (Helper Metodu Çağırıyoruz)
                var resultList = MapToListItemDto(pagedEntities);

                // 6. KULLANICIYA ÖZEL DURUMLAR (Favori / Satın Alma)
                if (currentUserId > 0 && resultList.Any())
                {
                    var trainingIds = resultList.Select(x => x.Id).ToList();

                    // Kullanıcının bu sayfadaki eğitimlerden hangilerine sahip olduğu
                    var assignedIds = await currAccTrainingUserRepository.GetQuery(x =>
                        x.UserId == currentUserId &&
                        x.IsActive == true &&
                        trainingIds.Contains(x.CurrAccTrainings.TrainingId)
                    ).Select(x => x.CurrAccTrainings.TrainingId).ToListAsync();

                    // Kullanıcının bu sayfadaki eğitimlerden hangilerini favorilediği
                    var favoriteIds = await favoriteTrainingRepository.GetQuery(x =>
                        x.UserId == currentUserId &&
                        x.IsActive == true &&
                        trainingIds.Contains(x.TrainingId)
                    ).Select(x => x.TrainingId).ToListAsync();

                    // Listeyi güncelle
                    foreach (var item in resultList)
                    {
                        item.IsAssigned = assignedIds.Contains(item.Id);
                        item.IsFavorite = favoriteIds.Contains(item.Id);
                    }
                }

                return ProduceSuccessResponse(new PagedList<TrainingListItemDto>(resultList, totalRecords, request.PageIndex, request.PageSize));
            }
            catch (Exception ex)
            {
                // Loglama yapılabilir
                return ProduceFailResponse<PagedList<TrainingListItemDto>>("Eğitim listesi alınırken hata: " + ex.Message, 500);
            }
        }

        public async Task<Response<List<TrainingViewCardDto>>> SearchTrainingsAsync(string searchTerm, int pageIndex = 0, int pageSize = 12)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || searchTerm.Length < 3) return ProduceSuccessResponse(new List<TrainingViewCardDto>());
            searchTerm = searchTerm.Trim().ToLower();

            var pagedEntities = await trainingRepository.GetPagedListAsync(
                predicate: x => x.IsActive == true && (x.IsDelete == false || x.IsDelete == null) &&
                                (x.Title.ToLower().Contains(searchTerm) || (x.Description != null && x.Description.ToLower().Contains(searchTerm))),
                include: i => i.Include(t => t.TrainingCategory).Include(t => t.Instructor).ThenInclude(ins => ins.User).Include(t => t.TrainingLevel)
                               .Include(t => t.PriceTier).ThenInclude(pt => pt.Details)
                               .Include(t => t.PriceTier).ThenInclude(pt => pt.CampaignPriceTiers).ThenInclude(cpt => cpt.Campaign)
                               .Include(t => t.Reviews),
                orderBy: o => o.OrderByDescending(t => t.CreatedDate),
                skip: pageIndex * pageSize, take: pageSize
            );

            return ProduceSuccessResponse(MapToCardDto(pagedEntities.ToList()));
        }

        // =================================================================================================
        // DİĞER METODLAR
        // =================================================================================================

        public async Task<Response<IEnumerable<GetMyTrainingDto>>> GetMyTrainingsAsync(CancellationToken cancellationToken = default)
        {
            var currentUserId = GetCurrentUserId();
            string defaultTrainingImage = "/images/default-course-cover.png";

            var assignedList = await currAccTrainingUserRepository.GetListAsync(
                predicate: x => x.UserId == currentUserId && x.IsActive == true,
                include: i => i.Include(x => x.CurrAccTrainings),
                cancellationToken: cancellationToken
            );

            if (!assignedList.Any()) return ProduceSuccessResponse(Enumerable.Empty<GetMyTrainingDto>());

            var trainingIds = assignedList.Select(x => x.CurrAccTrainings.TrainingId).Distinct().ToList();
            var trainings = await trainingRepository.GetListAsync(
                predicate: x => trainingIds.Contains(x.Id) && x.IsActive == true && x.IsDelete == false,
                include: i => i.Include(t => t.TrainingSections).ThenInclude(s => s.TrainingContents)
                               .Include(t => t.Instructor).ThenInclude(ins => ins.User).Include(t => t.TrainingCategory),
                cancellationToken: cancellationToken
            );

            var relevantContentIds = trainings.SelectMany(t => t.TrainingSections).SelectMany(s => s.TrainingContents).Where(c => c.IsDelete == false).Select(c => c.Id).ToList();
            var userLogs = await userContentsViewLogRepository.GetListAsync(predicate: x => x.CurrAccTrainingUser.UserId == currentUserId && relevantContentIds.Contains(x.TrainingContentId), cancellationToken: cancellationToken);

            var resultList = new List<GetMyTrainingDto>();
            foreach (var training in trainings)
            {
                var contentsOfThisTraining = training.TrainingSections.SelectMany(s => s.TrainingContents).Where(c => c.IsDelete == false).ToList();
                int totalCount = contentsOfThisTraining.Count;
                var logsForThisTraining = userLogs.Where(l => contentsOfThisTraining.Any(c => c.Id == l.TrainingContentId)).ToList();
                int completedCount = logsForThisTraining.Count(l => l.IsCompleted == true);
                int percentage = totalCount > 0 ? (int)((double)completedCount / totalCount * 100) : 0;
                var lastLog = logsForThisTraining.OrderByDescending(l => l.UpdateDate ?? l.CreatedDate).FirstOrDefault();
                long? resumeContentId = lastLog?.TrainingContentId ?? contentsOfThisTraining.OrderBy(c => c.OrderId).FirstOrDefault()?.Id;
                var assignRecord = assignedList.FirstOrDefault(x => x.CurrAccTrainings.TrainingId == training.Id);

                string status = "Active";
                if (assignRecord.StartDate.HasValue && assignRecord.StartDate > DateTime.UtcNow) status = "NotStarted";
                else if (assignRecord.DueDate.HasValue && assignRecord.DueDate < DateTime.UtcNow) status = "Expired";

                resultList.Add(new GetMyTrainingDto
                {
                    Id = training.Id,
                    Title = training.Title,
                    Description = training.Description,
                    PicturePath = !string.IsNullOrEmpty(training.HeaderImage) ? training.HeaderImage : defaultTrainingImage,
                    CategoryName = training.TrainingCategory?.Title ?? "Genel",
                    InstructorName = training.Instructor?.User != null ? $"{training.Instructor.User.Name} {training.Instructor.User.SurName}" : "HrHub Instructor",
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

        public async Task<Response<List<TrainingCardDto>>> GetNavbarRecentTrainingsAsync(int count = 5)
        {
            try
            {
                long userId = GetCurrentUserId();
                var resultList = new List<TrainingCardDto>();
                var recentLogsRaw = await userContentsViewLogRepository.GetListAsync(
                    predicate: x => x.CurrAccTrainingUser.UserId == userId, orderBy: o => o.OrderByDescending(x => x.CreatedDate),
                    include: i => i.Include(x => x.CurrAccTrainingUser).ThenInclude(u => u.CurrAccTrainings).ThenInclude(ct => ct.Training),
                    selector: x => new { TrainingId = x.CurrAccTrainingUser.CurrAccTrainings.Training.Id, Title = x.CurrAccTrainingUser.CurrAccTrainings.Training.Title, RawHeaderImage = x.CurrAccTrainingUser.CurrAccTrainings.Training.HeaderImage }
                );
                resultList.AddRange(recentLogsRaw.GroupBy(x => x.TrainingId).Select(g => g.First()).Take(count)
                    .Select(x => new TrainingCardDto { Id = x.TrainingId, Title = x.Title, ImageUrl = GetHeaderImage(x.RawHeaderImage), Progress = 25 }));

                if (resultList.Count < count)
                {
                    int remaining = count - resultList.Count;
                    var existingIds = resultList.Select(x => x.Id).ToList();
                    var newAssignments = await currAccTrainingUserRepository.GetPagedListAsync(
                        predicate: x => x.UserId == userId && x.IsActive == true && !existingIds.Contains(x.CurrAccTrainings.TrainingId),
                        orderBy: o => o.OrderByDescending(x => x.CreatedDate), include: i => i.Include(u => u.CurrAccTrainings).ThenInclude(ct => ct.Training),
                        skip: 0, take: remaining, selector: a => new { Id = a.CurrAccTrainings.Training.Id, Title = a.CurrAccTrainings.Training.Title, RawHeaderImage = a.CurrAccTrainings.Training.HeaderImage }
                    );
                    resultList.AddRange(newAssignments.Select(x => new TrainingCardDto { Id = x.Id, Title = x.Title, ImageUrl = GetHeaderImage(x.RawHeaderImage), Progress = 0 }));
                }
                return ProduceSuccessResponse(resultList);
            }
            catch (Exception ex) { return ProduceFailResponse<List<TrainingCardDto>>("Hata: " + ex.Message, 500); }
        }

        public async Task<Response<CommonResponse>> ToggleFavoriteAsync(long trainingId)
        {
            try
            {
                long userId = GetCurrentUserId();
                var existingFav = await favoriteTrainingRepository.GetAsync(x => x.UserId == userId && x.TrainingId == trainingId);
                if (existingFav != null) { favoriteTrainingRepository.Delete(existingFav); await hrUnitOfWork.SaveChangesAsync(); return ProduceSuccessResponse(new CommonResponse { Result = false, Code = 200, Message = "Success" }); }
                else { await favoriteTrainingRepository.AddAsync(new FavoriteTraining { UserId = userId, TrainingId = trainingId, IsActive = true, CreatedDate = DateTime.UtcNow, CreateUserId = userId }); await hrUnitOfWork.SaveChangesAsync(); return ProduceSuccessResponse(new CommonResponse { Result = true, Code = 200, Message = "Success" }); }
            }
            catch (Exception ex) { return ProduceFailResponse<CommonResponse>("Hata: " + ex.Message, 500); }
        }

        public async Task<Response<CommonResponse>> AddOrUpdateReviewAsync(AddReviewDto request)
        {
            try
            {
                long userId = GetCurrentUserId();
                if (!await currAccTrainingUserRepository.GetQuery(x => x.UserId == userId && x.CurrAccTrainings.TrainingId == request.TrainingId && x.IsActive == true).AnyAsync())
                    return ProduceFailResponse<CommonResponse>("Satın almadığınız eğitimi değerlendiremezsiniz.", HrStatusCodes.Status403Forbidden);

                var review = await trainingReviewRepository.GetAsync(x => x.UserId == userId && x.TrainingId == request.TrainingId);
                if (review != null) { review.Rating = request.Rating; review.Comment = request.Comment; review.UpdateDate = DateTime.UtcNow; trainingReviewRepository.Update(review); }
                else { await trainingReviewRepository.AddAsync(new TrainingReview { UserId = userId, TrainingId = request.TrainingId, Rating = request.Rating, Comment = request.Comment, IsApproved = true, IsActive = true, CreatedDate = DateTime.UtcNow, CreateUserId = userId }); }
                await hrUnitOfWork.SaveChangesAsync();
                return ProduceSuccessResponse(new CommonResponse { Result = true, Code = 200, Message = "Success" });
            }
            catch (Exception ex) { return ProduceFailResponse<CommonResponse>("Hata: " + ex.Message, 500); }
        }

        public async Task<Response<TrainingFilterOptionsDto>> GetTrainingFilterOptionsAsync()
        {
            try
            {
                var response = new TrainingFilterOptionsDto();

                // 1. DÜZELTİLEN SATIR BURASI:
                // include parametresi 'orderBy' parametresinin yerini aldığı için,
                // 'include: ...' şeklinde isimlendirilmiş argüman kullanıyoruz.
                var instructors = await instructorRepository.GetQuery(
                        predicate: x => x.IsActive == true && x.IsDelete != true,
                        include: i => i.Include(u => u.User) // Explicit naming
                    )
                    .Select(x => new FlatFilterData
                    {
                        Id = x.Id,
                        Title = x.User.Name + " " + x.User.SurName,
                        ParentId = (long?)null,
                        Type = "INS",
                        Order = 0
                    })
                    .ToListAsync();

                // Diğerleri zaten düzgündü
                var cats = await trainingCategoryRepository.GetQuery(x => x.IsActive == true && x.IsDelete != true)
                    .Select(x => new FlatFilterData { Id = x.Id, Title = x.Title, ParentId = x.MasterCategoryId, Type = "CAT", Order = 0 }).ToListAsync();

                var levels = await trainingLevelRepository.GetQuery(x => x.IsActive == true && x.IsDelete != true)
                    .Select(x => new FlatFilterData { Id = x.Id, Title = x.Title, ParentId = (long?)null, Type = "LVL", Order = (int)x.Id }).ToListAsync();

                var langs = await trainingLanguageRepository.GetQuery(x => x.IsActive == true && x.IsDelete != true)
                    .Select(x => new FlatFilterData { Id = x.Id, Title = x.Title, ParentId = (long?)null, Type = "LNG", Order = 0 }).ToListAsync();

                // Hepsini birleştir
                var flatList = cats.Concat(levels).Concat(langs).Concat(instructors).ToList();

                response.Categories = flatList.Where(x => x.Type == "CAT").OrderBy(x => x.Title).Select(x => new FilterItemDto { Id = x.Id, Title = x.Title, ParentId = x.ParentId }).ToList();
                response.Levels = flatList.Where(x => x.Type == "LVL").OrderBy(x => x.Order).Select(x => new FilterItemDto { Id = x.Id, Title = x.Title, ParentId = null }).ToList();
                response.Languages = flatList.Where(x => x.Type == "LNG").OrderBy(x => x.Title).Select(x => new FilterItemDto { Id = x.Id, Title = x.Title, ParentId = null }).ToList();
                response.Instructors = flatList.Where(x => x.Type == "INS").OrderBy(x => x.Title).Select(x => new FilterItemDto { Id = x.Id, Title = x.Title, ParentId = null }).ToList();

                return ProduceSuccessResponse(response);
            }
            catch (Exception ex) { return ProduceFailResponse<TrainingFilterOptionsDto>("Filtre seçenekleri alınırken hata oluştu: " + ex.Message, 500); }
        }

        public async Task<Response<TrainingPublicDetailDto>> GetTrainingPublicDetailAsync(long id)
        {
            // 1. QUERY OLUŞTURMA: Tek seferde tüm veriyi (ilişkiler dahil) çekecek projection.
            // Bu sayede N+1 problemi oluşmaz ve sadece gerekli kolonlar çekilir.
            var query = trainingRepository.GetQuery(t => t.Id == id && t.IsActive == true && (t.IsDelete == false || t.IsDelete == null))
                .Select(t => new
                {
                    // Ana Eğitim Bilgileri
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    HeaderImage = t.HeaderImage,
                    Trailer = t.Trailer,
                    Language = t.TrainingLanguage.Title,
                    CategoryName = t.TrainingCategory.Title,
                    LevelName = t.TrainingLevel.Title,
                    LastUpdateDate = t.UpdateDate ?? t.CreatedDate,

                    // Fiyat Hesaplaması İçin Gerekli Ham Veri (Compleks obje yerine sadece ilgili alanlar)
                    PriceTierDetails = t.PriceTier.Details.Where(d => d.IsActive).ToList(),
                    Campaigns = t.PriceTier.CampaignPriceTiers.Select(c => c.Campaign).Where(c => c.IsActive && c.EndDate > DateTime.UtcNow).ToList(),

                    // Eğitmen Bilgileri
                    Instructor = new
                    {
                        Id = t.InstructorId ?? 0,
                        Name = t.Instructor.User.Name + " " + t.Instructor.User.SurName,
                        Title = t.Instructor.Title,
                        Image = t.Instructor.PicturePath,
                        Bio = t.Instructor.Description
                    },

                    // İstatistikler (Alt sorgu olarak çalışır, çok hızlıdır)
                    Rating = t.TrainingReviews.Any(r => r.IsActive == true && r.IsApproved == true)
                             ? t.TrainingReviews.Where(r => r.IsActive == true && r.IsApproved == true).Average(r => r.Rating)
                             : 0,
                    ReviewCount = t.TrainingReviews.Count(r => r.IsActive == true && r.IsApproved == true),

                    // Müfredat (Nested Projection)
                    Sections = t.TrainingSections
                        .Where(s => s.IsActive == true && s.IsDelete != true)
                        .OrderBy(s => s.RowNumber)
                        .Select(s => new
                        {
                            s.Id,
                            s.Title,
                            s.RowNumber,
                            Contents = s.TrainingContents
                                .Where(c => c.IsActive == true && c.IsDelete != true)
                                .OrderBy(c => c.OrderId)
                                .Select(c => new
                                {
                                    c.Id,
                                    c.Title,
                                    IsPreview = false, // DB'ye alan eklendiyse: c.IsPreview
                                    Time = c.Time,
                                    Type = c.ContentType.Title
                                }).ToList()
                        }).ToList(),

                    // Neler Öğreneceksiniz
                    WhatYouWillLearn = t.WhatYouWillLearns
                        .Where(w => w.IsActive == true && w.IsDelete != true)
                        .Select(w => w.Title).ToList(),

                    // Yorumlar (İlk 5)
                    TopReviews = t.TrainingReviews
                        .Where(r => r.IsActive == true && r.IsApproved == true)
                        .OrderByDescending(r => r.CreatedDate)
                        .Take(5)
                        .Select(r => new
                        {
                            UserName = r.User.Name + " " + r.User.SurName.Substring(0, 1) + ".",
                            UserImage = "r.User.ProfileImagePath", // TODO buraya kullanıcı fotoğrafı konulacak.
                            r.Rating,
                            r.Comment,
                            r.CreatedDate
                        }).ToList()
                });

            // 2. VERİTABANI ÇAĞRISI (Tek Sefer)
            var rawData = await query.FirstOrDefaultAsync();

            if (rawData == null)
                return ProduceFailResponse<TrainingPublicDetailDto>("Eğitim bulunamadı.", StatusCodes.Status404NotFound);

            // 3. BELLEK İÇİ DÜZENLEME (Mapping & Calculation)
            // Veritabanından gelen anonim objeyi DTO'ya çeviriyoruz.

            // Fiyat Hesaplama Mantığı (Helper kullanmadan, elimizdeki veriyle)
            decimal listPrice = 0;
            if (rawData.PriceTierDetails != null && rawData.PriceTierDetails.Any())
            {
                var baseRule = rawData.PriceTierDetails
                    .Where(d => d.MinLicenceCount <= 1)
                    .OrderByDescending(d => d.MinLicenceCount)
                    .FirstOrDefault();
                listPrice = baseRule?.Amount ?? rawData.PriceTierDetails.OrderBy(d => d.MinLicenceCount).First().Amount;
            }

            // Kampanya Mantığı (Basitçe en yüksek indirimi uygula)
            decimal sellingPrice = listPrice;
            if (rawData.Campaigns != null && rawData.Campaigns.Any())
            {
                // Örnek: İlk geçerli kampanyayı al (Geliştirilebilir)
                var campaign = rawData.Campaigns.First();
                // Burada indirim hesaplama mantığını campaign tipine göre yapabilirsin
                // Şimdilik kampanya yokmuş gibi davranıyorum, bozulmasın diye.
            }

            decimal discountRate = listPrice > 0 ? ((listPrice - sellingPrice) / listPrice) * 100 : 0;

            // Ekstra Count Sorguları (Mecburen ayrı, çünkü Aggregate Root dışı)
            // Bunları cachelemek performansı artırır.
            var studentCount = await currAccTrainingUserRepository.CountAsync(x => x.CurrAccTrainings.TrainingId == id);

            // DTO Oluşturma
            var dto = new TrainingPublicDetailDto
            {
                Id = rawData.Id,
                Title = rawData.Title,
                Description = rawData.Description,
                HeaderImage = !string.IsNullOrEmpty(rawData.HeaderImage) ? rawData.HeaderImage : "/images/default-course-cover.png",
                PreviewVideoPath = rawData.Trailer,
                Language = rawData.Language ?? "Türkçe",
                CategoryName = rawData.CategoryName,
                LevelName = rawData.LevelName,
                LastUpdateDate = rawData.LastUpdateDate,

                Amount = listPrice,
                CurrentAmount = sellingPrice,
                DiscountRate = discountRate,

                InstructorId = rawData.Instructor.Id,
                InstructorName = rawData.Instructor.Name,
                InstructorTitle = rawData.Instructor.Title,
                InstructorImage = !string.IsNullOrEmpty(rawData.Instructor.Image) ? rawData.Instructor.Image : "/images/defaults/user-dummy.png",
                InstructorBio = rawData.Instructor.Bio,

                Rating = (double)rawData.Rating,
                ReviewCount = rawData.ReviewCount,
                StudentCount = studentCount,

                // Instructor istatistiklerini şimdilik bu kursun verileriyle dolduruyoruz (DB'yi yormamak için)
                InstructorTotalCourses = 1, // Bunu ayrı sorgu yapabilirsin: await trainingRepository.CountAsync(...)
                InstructorTotalStudents = studentCount,
                InstructorRating = (double)rawData.Rating,

                WhatYouWillLearn = rawData.WhatYouWillLearn,

                Sections = rawData.Sections.Select(s => new PublicSectionDto
                {
                    Id = s.Id,
                    Title = s.Title,
                    OrderId = (int)s.RowNumber,
                    Contents = s.Contents.Select(c => new PublicContentDto
                    {
                        Id = c.Id,
                        Title = c.Title,
                        IsPreview = c.IsPreview,
                        DurationMinutes = c.Time.HasValue ? (int)c.Time.Value.TotalMinutes : 0,
                        Type = c.Type
                    }).ToList()
                }).ToList(),

                TopReviews = rawData.TopReviews.Select(r => new PublicReviewDto
                {
                    UserName = r.UserName,
                    UserImage = !string.IsNullOrEmpty(r.UserImage) ? r.UserImage : "/images/defaults/user-dummy.png",
                    Rating = (double)r.Rating,
                    Comment = r.Comment,
                    Date = r.CreatedDate
                }).ToList()
            };

            return ProduceSuccessResponse(dto);
        }

        // =================================================================================================
        // PRIVATE HELPERS
        // =================================================================================================

        private (decimal Amount, decimal CurrentAmount, decimal DiscountRate) CalculatePricing(Training training)
        {
            if (training.PriceTier == null || training.PriceTier.Details == null || !training.PriceTier.Details.Any())
                return (0, 0, 0);

            // 1. ADIM: Liste Fiyatını (Anchor Price) Bul - Detail tablosunda "1 Kişi" için olan fiyat
            var baseDetailRule = training.PriceTier.Details
                .Where(d => d.IsActive && d.MinLicenceCount <= 1)
                .OrderByDescending(d => d.MinLicenceCount)
                .FirstOrDefault();

            if (baseDetailRule == null)
            {
                baseDetailRule = training.PriceTier.Details.OrderBy(d => d.MinLicenceCount).FirstOrDefault();
            }

            decimal listPrice = baseDetailRule?.Amount ?? 0; // Liste Fiyatı
            decimal sellingPrice = listPrice;                // Satış Fiyatı
            decimal discountRate = 0;

            // 2. ADIM: KAMPANYA KONTROLÜ
            if (training.PriceTier.CampaignPriceTiers != null && training.PriceTier.CampaignPriceTiers.Any())
            {
                var activeCampaign = training.PriceTier.CampaignPriceTiers
                    .Select(cpt => cpt.Campaign)
                    .FirstOrDefault(c => c.IsActive &&
                                         c.StartDate <= DateTime.UtcNow &&
                                         c.EndDate >= DateTime.UtcNow);

                if (activeCampaign != null)
                {
                    if (activeCampaign.Type == CampaignType.PercentageDiscount)
                    {
                        sellingPrice = listPrice - (listPrice * activeCampaign.Value / 100);
                    }
                    else if (activeCampaign.Type == CampaignType.FixedAmountDiscount)
                    {
                        sellingPrice = listPrice - activeCampaign.Value;
                    }
                }
            }

            if (sellingPrice < 0) sellingPrice = 0;

            if (listPrice > 0)
            {
                discountRate = ((listPrice - sellingPrice) / listPrice) * 100;
            }

            return (listPrice, sellingPrice, discountRate);
        }

        private List<TrainingViewCardDto> MapToCardDto(List<Training> entities)
        {
            return entities.Select(x => {
                var priceInfo = CalculatePricing(x);
                return new TrainingViewCardDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    HeaderImage = GetHeaderImage(x.HeaderImage),
                    CategoryTitle = x.TrainingCategory?.Title ?? "",
                    InstructorTitle = x.Instructor?.User != null ? $"{x.Instructor.User.Name} {x.Instructor.User.SurName}" : "",
                    Amount = priceInfo.Amount,
                    CurrentAmount = priceInfo.CurrentAmount,
                    DiscountRate = priceInfo.DiscountRate,
                    ReviewCount = x.TrainingReviews?.Count ?? 0,
                    Rating = (x.TrainingReviews?.Any() ?? false) ? x.Reviews.Average(r => r.TrainingPoint) : 0,
                    CreatedDate = x.CreatedDate
                };
            }).ToList();
        }

        private List<TrainingListItemDto> MapToListItemDto(List<Training> entities)
        {
            // 30 günden yeni olanlara "Yeni" etiketi basmak için tarih sınırı
            var newThresholdDate = DateTime.UtcNow.AddDays(-30);

            return entities.Select(x => {
                // 1. Fiyat Hesaplaması (Senin yazdığın helper)
                var priceInfo = CalculatePricing(x);

                // 2. İçerik İstatistiklerini Hesapla (Ders Sayısı ve Toplam Dakika)
                // Null check yaparak patlamasını önlüyoruz.
                var allContents = x.TrainingSections?.SelectMany(s => s.TrainingContents).Where(c => c.IsActive == true).ToList();
                int totalMinutes = allContents?.Sum(c => Convert.ToInt32(c.Time.Value.TotalMinutes)) ?? 0; // DurationMinutes int varsayıyorum
                int lessonCount = allContents?.Count ?? 0;

                // 3. Puan Hesaplama
                double avgRating = 0;
                int reviewCount = 0;
                if (x.TrainingReviews != null && x.TrainingReviews.Any(r => r.IsActive == true && r.IsApproved == true))
                {
                    var activeReviews = x.TrainingReviews.Where(r => r.IsActive == true && r.IsApproved == true).ToList();
                    reviewCount = activeReviews.Count;
                    avgRating = activeReviews.Average(r => r.Rating); // Rating propertysi
                }

                // 4. DTO Oluşturma (Tüm alanlar dolu)
                return new TrainingListItemDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    HeaderImage = GetHeaderImage(x.HeaderImage), // Helper metodun

                    // Kategori Bilgileri
                    CategoryId = x.CategoryId,
                    ParentCategoryId = x.TrainingCategory?.MasterCategoryId,
                    CategoryName = x.TrainingCategory?.Title ?? "",

                    // Eğitmen Bilgileri
                    InstructorName = x.Instructor?.User != null ? $"{x.Instructor.User.Name} {x.Instructor.User.SurName}" : "",
                    // Eğitmen resmi varsa onu, yoksa default
                    InstructorImage = !string.IsNullOrEmpty(x.Instructor?.PicturePath) ? x.Instructor.PicturePath : "assets/images/defaults/user-dummy.png",

                    // Fiyatlandırma
                    Amount = priceInfo.Amount,
                    CurrentAmount = priceInfo.CurrentAmount,
                    DiscountRate = priceInfo.DiscountRate,
                    PriceTierId = x.PriceTierId ?? 0,

                    // Meta Veriler
                    LevelName = x.TrainingLevel?.Title ?? "Genel", // Seviye (Başlangıç vs.)
                    Rating = avgRating,
                    ReviewCount = reviewCount,

                    // Eğitim İçerik İstatistikleri
                    TotalMinutes = totalMinutes,
                    LessonCount = lessonCount,

                    // Neler Öğreneceksiniz (İlk 3-4 maddeyi alalım ki liste şişmesin)
                    WhatYouWillLearn = x.WhatYouWillLearns?
                                        .Where(w => w.IsActive == true)
                                        .OrderBy(w => w.Id)
                                        .Select(w => w.Title)
                                        .Take(4)
                                        .ToList() ?? new List<string>(),

                    CreatedDate = x.CreatedDate,

                    // --- UI ROZET MANTIĞI ---
                    IsPrivate = x.IsPrivate,
                    IsActive = x.IsActive == true,

                    // "YENİ": Oluşturulma tarihi son 30 gün içindeyse
                    IsNew = x.CreatedDate >= newThresholdDate,

                    // "ÇOK SATAN": Bunu istersen Review sayısına veya Rating'e bağlayabiliriz.
                    // Örn: Puanı 4.5 üzeri ve en az 10 yorumu varsa "Bestseller" olsun.
                    IsBestseller = (avgRating >= 4.5 && reviewCount >= 10),

                    // Favori ve Atanmış durumu kullanıcıya özeldir,
                    // bu liste genel olduğu için varsayılan false döneriz.
                    // Asıl değerler GetAdvancedTrainingListAsync içinde set edilecek.
                    IsFavorite = false,
                    IsAssigned = false
                };
            }).ToList();
        }

        private string GetHeaderImage(string imagePath)
        {
            return string.IsNullOrEmpty(imagePath) ? "none" : imagePath;
        }

        private class FlatFilterData { public long Id { get; set; } public string Title { get; set; } public long? ParentId { get; set; } public string Type { get; set; } public int Order { get; set; } }
    }
}