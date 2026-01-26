using HrHub.Abstraction.Result;
using HrHub.Abstraction.Settings;
using HrHub.Abstraction.StatusCodes;
using HrHub.Core.Base;
using HrHub.Core.Data.Repository;
using HrHub.Core.Helpers;
using HrHub.Domain.Contracts.Dtos.DashboardDtos;
using HrHub.Domain.Contracts.Enums;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HrHub.Application.Managers.DashboardManagers
{
    public class DashboardManager : ManagerBase, IDashboardManager
    {
        private readonly IHrUnitOfWork unitOfWork;
        private readonly Repository<Training> trainingRepository;
        private readonly Repository<CurrAccTrainingUser> currAccTrainingUserRepository;
        private readonly Repository<UserContentsViewLog> userContentsViewLogRepository;

        public DashboardManager(IHttpContextAccessor httpContextAccessor,
                                IHrUnitOfWork unitOfWork) : base(httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;

            this.trainingRepository = unitOfWork.CreateRepository<Training>();
            this.currAccTrainingUserRepository = unitOfWork.CreateRepository<CurrAccTrainingUser>();
            this.userContentsViewLogRepository = unitOfWork.CreateRepository<UserContentsViewLog>();
        }

        // 1. HERO SECTION (Son İzlenen)
        public async Task<Response<ContinueTrainingDto>> GetLastActiveTrainingAsync()
        {
            try
            {
                long userId = GetCurrentUserId();

                var lastLog = await userContentsViewLogRepository.GetAsync<UserContentsViewLog>(
                    predicate: x => x.CurrAccTrainingUser.UserId == userId,
                    orderBy: o => o.OrderByDescending(x => x.CreatedDate),
                    include: i => i.Include(x => x.CurrAccTrainingUser)
                                   .ThenInclude(u => u.CurrAccTrainings)
                                   .ThenInclude(ct => ct.Training)
                                   .Include(x => x.TrainingContent)
                );

                // Eğer log yoksa, atanmış ilk eğitimi öner
                if (lastLog == null || lastLog.CurrAccTrainingUser?.CurrAccTrainings?.Training == null)
                {
                    var firstAssigned = await currAccTrainingUserRepository.GetAsync(
                        predicate: x => x.UserId == userId && x.IsActive == true,
                        include: i => i.Include(u => u.CurrAccTrainings).ThenInclude(ct => ct.Training)
                    );

                    if (firstAssigned != null && firstAssigned.CurrAccTrainings?.Training != null)
                    {
                        var trainingData = firstAssigned.CurrAccTrainings.Training;
                        return ProduceSuccessResponse(new ContinueTrainingDto
                        {
                            TrainingId = trainingData.Id,
                            Title = trainingData.Title,
                            ImageUrl = GetHeaderImage(trainingData.HeaderImage),
                            CategoryId = trainingData.CategoryId ?? 0,
                            Progress = 0,
                            LastLessonName = "Eğitime Başla"
                        });
                    }

                    return ProduceFailResponse<ContinueTrainingDto>("Henüz atanmış bir eğitim bulunamadı.", HrStatusCodes.Status111DataNotFound);
                }

                var activeTraining = lastLog.CurrAccTrainingUser.CurrAccTrainings.Training;

                var result = new ContinueTrainingDto
                {
                    TrainingId = activeTraining.Id,
                    Title = activeTraining.Title,
                    ImageUrl = GetHeaderImage(activeTraining.HeaderImage),
                    CategoryId = activeTraining.CategoryId ?? 0,
                    Progress = 10, // TODO: İlerleme hesaplaması user bazlı yapılmalı (TrainingManager'daki mantıkla)
                    LastLessonName = lastLog.TrainingContent?.Title ?? "Son İzlenen Bölüm"
                };

                return ProduceSuccessResponse(result);
            }
            catch (Exception exp)
            {
                return ProduceFailResponse<ContinueTrainingDto>("Hata: " + exp.Message, HrStatusCodes.Status111DataNotFound);
            }
        }

        // 2. İSTATİSTİKLER
        public async Task<Response<DashboardStatsDto>> GetUserStatsAsync()
        {
            long userId = GetCurrentUserId();

            var userTrainings = await currAccTrainingUserRepository.GetListAsync(
                predicate: x => x.UserId == userId && x.IsActive == true,
                include: i => i.Include(x => x.UserCertificates)
            );

            var stats = new DashboardStatsDto
            {
                CompletedTrainingsCount = userTrainings.Count(x => x.UserCertificates != null && x.UserCertificates.Any()),
                InProgressTrainingsCount = userTrainings.Count(x => x.UserCertificates == null || !x.UserCertificates.Any()),
                TotalCertificates = userTrainings.Sum(x => x.UserCertificates?.Count ?? 0)
            };

            return ProduceSuccessResponse(stats);
        }

        // 3. ATANAN EĞİTİMLER (Pricing Updated)
        public async Task<Response<List<TrainingCardDto>>> GetAssignedTrainingsAsync()
        {
            long userId = GetCurrentUserId();
            var predicateBuilder = PredicateBuilder.New<CurrAccTrainingUser>();

            predicateBuilder = predicateBuilder.And(x => x.UserId == userId);
            predicateBuilder = predicateBuilder.And(x => x.IsActive == true);
            predicateBuilder = predicateBuilder.And(x => !x.UserCertificates.Any());

            // İlişkileri Include ediyoruz
            var assignedList = await currAccTrainingUserRepository.GetListAsync(
                predicate: predicateBuilder,
                include: i => i.Include(x => x.CurrAccTrainings)
                               .ThenInclude(ct => ct.Training).ThenInclude(t => t.TrainingCategory)
                               .Include(x => x.CurrAccTrainings).ThenInclude(ct => ct.Training).ThenInclude(t => t.Instructor).ThenInclude(ins => ins.User)
                               .Include(x => x.CurrAccTrainings).ThenInclude(ct => ct.Training).ThenInclude(t => t.TrainingReviews)
                               .Include(x => x.CurrAccTrainings).ThenInclude(ct => ct.Training).ThenInclude(t => t.TrainingLevel)
                               .Include(x => x.CurrAccTrainings).ThenInclude(ct => ct.Training).ThenInclude(t => t.TrainingSections).ThenInclude(s => s.TrainingContents)
                               // FİYAT SİSTEMİ
                               .Include(x => x.CurrAccTrainings).ThenInclude(ct => ct.Training).ThenInclude(t => t.PriceTier).ThenInclude(pt => pt.Details)
                               .Include(x => x.CurrAccTrainings).ThenInclude(ct => ct.Training).ThenInclude(t => t.PriceTier).ThenInclude(pt => pt.CampaignPriceTiers).ThenInclude(cpt => cpt.Campaign)
            );

            var dtoList = assignedList.Select(x => {
                var training = x.CurrAccTrainings.Training;

                // Fiyat Hesapla
                var priceInfo = CalculatePricing(training);

                // Puan Hesapla
                double rating = 0;
                int reviewCount = 0;
                if (training.TrainingReviews != null && training.TrainingReviews.Any(r => r.IsActive == true && r.IsApproved == true))
                {
                    rating = training.TrainingReviews.Where(r => r.IsActive == true && r.IsApproved == true).Average(r => r.Rating);
                    reviewCount = training.TrainingReviews.Count(r => r.IsActive == true && r.IsApproved == true);
                }

                // Süre Hesapla
                int totalMinutes = 0;
                if (training.TrainingSections != null)
                {
                    totalMinutes = training.TrainingSections
                                    .SelectMany(s => s.TrainingContents)
                                    .Where(c => c.IsActive == true && c.Time.HasValue)
                                    .Sum(c => (int)c.Time.Value.TotalMinutes);
                }

                return new TrainingCardDto
                {
                    Id = training.Id,
                    Title = training.Title,
                    Description = training.Description,
                    HeaderImage = GetHeaderImage(training.HeaderImage),
                    CategoryId = training.CategoryId ?? 0,
                    ParentCategoryId = training.TrainingCategory != null ? (training.TrainingCategory.MasterCategoryId ?? 0) : 0,
                    CategoryTitle = training.TrainingCategory != null ? training.TrainingCategory.Title : "",
                    InstructorName = training.Instructor != null && training.Instructor.User != null
                        ? $"{training.Instructor.User.Name} {training.Instructor.User.SurName}"
                        : "HrHub Eğitmen",
                    IsAssigned = true,
                    Rating = rating,
                    ReviewCount = reviewCount,
                    TrainingLevelTitle = training.TrainingLevel != null ? training.TrainingLevel.Title : "Genel",
                    TotalDurationMinutes = totalMinutes,

                    // Fiyatlar
                    Amount = priceInfo.Amount,
                    CurrentAmount = priceInfo.CurrentAmount,
                    DiscountRate = priceInfo.DiscountRate,

                    CreatedDate = training.CreatedDate
                };
            }).ToList();

            return ProduceSuccessResponse(dtoList);
        }

        // 4. ÖNERİLEN EĞİTİMLER (Pricing Updated)
        public async Task<Response<List<TrainingViewCardDto>>> GetRecommendedTrainingsAsync()
        {
            try
            {
                long userId = GetCurrentUserId();

                // 1. Kullanıcı Geçmişi
                var userHistory = await currAccTrainingUserRepository.GetListAsync(
                    predicate: x => x.UserId == userId,
                    selector: x => new {
                        TrainingId = x.CurrAccTrainings != null ? x.CurrAccTrainings.TrainingId : 0,
                        CategoryId = (x.CurrAccTrainings != null && x.CurrAccTrainings.Training != null) ? x.CurrAccTrainings.Training.CategoryId : (long?)null
                    }
                );

                var ownedTrainingIds = userHistory.Select(x => (long)x.TrainingId).ToHashSet();

                var lastInteractedCategoryId = userHistory
                    .Where(x => x.CategoryId != null && x.CategoryId > 0)
                    .OrderByDescending(x => x.TrainingId)
                    .Select(x => x.CategoryId)
                    .FirstOrDefault();

                List<TrainingViewCardDto> finalResult = new();

                // 2. QUERY HAZIRLA (Tüm Include'lar burada)
                var query = unitOfWork.CreateRepository<Training>().GetQuery()
                    .Include(t => t.TrainingCategory)
                    .Include(t => t.Instructor).ThenInclude(u => u.User)
                    .Include(t => t.TrainingLevel)
                    // FİYAT
                    .Include(t => t.PriceTier).ThenInclude(pt => pt.Details)
                    .Include(t => t.PriceTier).ThenInclude(pt => pt.CampaignPriceTiers).ThenInclude(cpt => cpt.Campaign)
                    // DİĞER
                    .Include(t => t.TrainingReviews)
                    .Include(t => t.TrainingSections).ThenInclude(s => s.TrainingContents)
                    .Where(x => x.IsActive == true && x.IsDelete != true && !ownedTrainingIds.Contains(x.Id));

                // A PLANI: Kategori Bazlı (Veriyi Çek -> Sonra Map'le)
                // Not: EF Core sorgusu içinde karmaşık fiyat hesaplaması yapamayacağımız için veriyi çekip bellekte (MapToCardDto) hesaplıyoruz.
                if (lastInteractedCategoryId.HasValue && lastInteractedCategoryId.Value > 0)
                {
                    var interestBasedEntities = await query
                        .Where(x => x.CategoryId == lastInteractedCategoryId)
                        .OrderByDescending(x => x.CreatedDate)
                        .Take(10)
                        .ToListAsync();

                    finalResult.AddRange(MapToCardDto(interestBasedEntities));
                }

                // B PLANI: Genel (Eğer 10 tane dolmadıysa)
                if (finalResult.Count < 10)
                {
                    int needed = 10 - finalResult.Count;
                    var existingIds = finalResult.Select(f => f.Id).ToList();

                    var generalEntities = await query
                        .Where(x => !existingIds.Contains(x.Id))
                        .OrderByDescending(x => x.CreatedDate)
                        .Take(needed)
                        .ToListAsync();

                    finalResult.AddRange(MapToCardDto(generalEntities));
                }

                return ProduceSuccessResponse(finalResult);
            }
            catch (Exception ex)
            {
                return ProduceSuccessResponse(new List<TrainingViewCardDto>());
            }
        }

        // =================================================================================================
        // PRIVATE HELPERS
        // =================================================================================================

        private List<TrainingViewCardDto> MapToCardDto(List<Training> entities)
        {
            return entities.Select(x => {
                var priceInfo = CalculatePricing(x);

                return new TrainingViewCardDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    HeaderImage = GetHeaderImage(x.HeaderImage),
                    CategoryId = x.CategoryId ?? 0,
                    ParentCategoryId = x.TrainingCategory != null ? (x.TrainingCategory.MasterCategoryId ?? 0) : 0,
                    CategoryTitle = x.TrainingCategory != null ? x.TrainingCategory.Title : "",
                    InstructorTitle = x.Instructor != null && x.Instructor.User != null
                        ? $"{x.Instructor.User.Name} {x.Instructor.User.SurName}"
                        : "HrHub Eğitmen",
                    InstructorPicturePath = x.Instructor != null ? x.Instructor.PicturePath : null,
                    TrainingLevelTitle = x.TrainingLevel != null ? x.TrainingLevel.Title : "Genel",

                    // Fiyatlar
                    Amount = priceInfo.Amount,
                    CurrentAmount = priceInfo.CurrentAmount,
                    DiscountRate = priceInfo.DiscountRate,

                    CreatedDate = x.CreatedDate,
                    Rating = x.TrainingReviews.Any(r => r.IsActive == true && r.IsApproved == true)
                        ? x.TrainingReviews.Where(r => r.IsActive == true && r.IsApproved == true).Average(r => r.Rating)
                        : 0,
                    ReviewCount = x.TrainingReviews.Count(r => r.IsActive == true && r.IsApproved == true),
                    TotalDurationMinutes = x.TrainingSections
                        .SelectMany(s => s.TrainingContents)
                        .Where(c => c.IsActive == true && c.Time != null)
                        .Sum(c => (int)c.Time.Value.TotalMinutes)
                };
            }).ToList();
        }

        private (decimal Amount, decimal CurrentAmount, decimal DiscountRate) CalculatePricing(Training training)
        {
            if (training.PriceTier == null || training.PriceTier.Details == null || !training.PriceTier.Details.Any())
                return (0, 0, 0);

            // 1. Baz Fiyat (Min:1)
            var baseDetailRule = training.PriceTier.Details
                .Where(d => d.IsActive && d.MinLicenceCount <= 1)
                .OrderByDescending(d => d.MinLicenceCount)
                .FirstOrDefault();

            if (baseDetailRule == null)
            {
                baseDetailRule = training.PriceTier.Details.OrderBy(d => d.MinLicenceCount).FirstOrDefault();
            }

            decimal listPrice = baseDetailRule?.Amount ?? 0;
            decimal sellingPrice = listPrice;
            decimal discountRate = 0;

            // 2. Kampanya Kontrolü
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

        private static string GetHeaderImage(string? dbImage)
        {
            if (string.IsNullOrEmpty(dbImage))
            {
                var defaultImg = AppSettingsHelper.GetData<ApplicationSettings>()?.DefaultCourseImage;
                return defaultImg ?? "none";
            }
            return dbImage;
        }
    }
}