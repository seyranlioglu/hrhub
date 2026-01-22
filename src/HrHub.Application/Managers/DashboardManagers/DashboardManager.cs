using HrHub.Abstraction.Result;
using HrHub.Abstraction.Settings; // Ayarlar için
using HrHub.Abstraction.StatusCodes;
using HrHub.Core.Base;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Contracts.Dtos.DashboardDtos;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options; // IOptions için

namespace HrHub.Application.Managers.DashboardManagers
{
    public class DashboardManager : ManagerBase, IDashboardManager
    {
        private readonly IHrUnitOfWork unitOfWork;
        private readonly Repository<Training> trainingRepository;
        private readonly Repository<CurrAccTrainingUser> currAccTrainingUserRepository;
        private readonly Repository<UserContentsViewLog> userContentsViewLogRepository;

        // AppSettings'i dependency injection ile alıyoruz
        private readonly ApplicationSettings _appSettings;

        public DashboardManager(IHttpContextAccessor httpContextAccessor,
                                IHrUnitOfWork unitOfWork,
                                IOptions<ApplicationSettings> appSettings) : base(httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this._appSettings = appSettings.Value;

            // Repository Init
            this.trainingRepository = unitOfWork.CreateRepository<Training>();
            this.currAccTrainingUserRepository = unitOfWork.CreateRepository<CurrAccTrainingUser>();
            this.userContentsViewLogRepository = unitOfWork.CreateRepository<UserContentsViewLog>();
        }

        // YARDIMCI METOD: Resim Yolu Düzeltici
        private string GetHeaderImage(string? dbImage)
        {
            if (string.IsNullOrEmpty(dbImage))
            {
                // AppSettings'ten gelen default resim
                return _appSettings.DefaultCourseImage ?? "assets/images/courses/default.jpg";
            }
            return dbImage;
        }

        // 1. HERO SECTION
        public async Task<Response<ContinueTrainingDto>> GetLastActiveTrainingAsync()
        {
            try
            {
                long userId = GetCurrentUserId();

                // SORGULAMA: Entity yapına göre revize edildi.
                // Log -> CurrAccTrainingUser (Atama) -> CurrAccTrainings (Şirket Eğitimi) -> Training (Eğitim Detayı)
                var lastLog = await userContentsViewLogRepository.GetAsync<UserContentsViewLog>(
                    predicate: x => x.CurrAccTrainingUser.UserId == userId,
                    orderBy: o => o.OrderByDescending(x => x.CreatedDate),
                    include: i => i.Include(x => x.CurrAccTrainingUser)
                                   .ThenInclude(u => u.CurrAccTrainings)
                                   .ThenInclude(ct => ct.Training)
                                   .Include(x => x.TrainingContent) // Son kalınan içerik adı için
                );

                if (lastLog == null || lastLog.CurrAccTrainingUser?.CurrAccTrainings?.Training == null)
                {
                    // Log yoksa, atanmış ilk eğitimi getir
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
                            Title = trainingData.Title, // Entity: TypeCardEntity -> Title
                            ImageUrl = GetHeaderImage(trainingData.HeaderImage), // Entity: HeaderImage
                            Progress = 0,
                            LastLessonName = "Eğitime Başla"
                        });
                    }

                    return ProduceFailResponse<ContinueTrainingDto>("Henüz atanmış bir eğitim bulunamadı.", HrStatusCodes.Status111DataNotFound);
                }

                var activeTraining = lastLog.CurrAccTrainingUser.CurrAccTrainings.Training;

                // TODO: İlerleme hesaplaması (Mock: %10)
                int progress = 10;

                var result = new ContinueTrainingDto
                {
                    TrainingId = activeTraining.Id,
                    Title = activeTraining.Title,
                    ImageUrl = GetHeaderImage(activeTraining.HeaderImage), // Helper metod kullanımı
                    Progress = progress,
                    // Log -> TrainingContent -> Title (TypeCardEntity'den gelir)
                    LastLessonName = lastLog.TrainingContent?.Title ?? "Son İzlenen Bölüm"
                };

                return ProduceSuccessResponse(result);
            }
            catch (Exception exp)
            {

                return ProduceFailResponse<ContinueTrainingDto>("Bilinmeyen Hata. Hata Mesajı : " + exp.ToJson(), HrStatusCodes.Status111DataNotFound);
            }
        }

        // 2. İSTATİSTİKLER
        public async Task<Response<DashboardStatsDto>> GetUserStatsAsync()
        {
            long userId = GetCurrentUserId();

            // Kullanıcıya atanmış eğitimleri çek (Include Certificate)
            var userTrainings = await currAccTrainingUserRepository.GetListAsync(
                predicate: x => x.UserId == userId && x.IsActive == true,
                include: i => i.Include(x => x.UserCertificates) // Sertifika kontrolü için
            );

            var stats = new DashboardStatsDto
            {
                // Tamamlananlar: Sertifikası olanlar (Basit mantık)
                CompletedTrainingsCount = userTrainings.Count(x => x.UserCertificates != null && x.UserCertificates.Any()),

                // Devam Edenler: Sertifikası olmayanlar
                InProgressTrainingsCount = userTrainings.Count(x => x.UserCertificates == null || !x.UserCertificates.Any()),

                // Toplam Sertifika
                TotalCertificates = userTrainings.Sum(x => x.UserCertificates?.Count ?? 0)
            };

            return ProduceSuccessResponse(stats);
        }

        // 3. ATANAN EĞİTİMLER (Zorunlu / Devam Eden)
        public async Task<Response<List<TrainingCardDto>>> GetAssignedTrainingsAsync()
        {
            long userId = GetCurrentUserId();
            var predicateBuilder = PredicateBuilder.New<CurrAccTrainingUser>();

            predicateBuilder = predicateBuilder.And(x => x.UserId == userId);
            predicateBuilder = predicateBuilder.And(x => x.IsActive == true);
            // Henüz sertifika almamış (bitmemiş) eğitimleri getir
            predicateBuilder = predicateBuilder.And(x => !x.UserCertificates.Any());

            var assignedList = await currAccTrainingUserRepository.GetListAsync(
                predicate: predicateBuilder,
                // Include Yolu: UserAssignment -> CompanyTraining -> Training -> Instructor
                include: i => i.Include(x => x.CurrAccTrainings)
                               .ThenInclude(ct => ct.Training)
                               .ThenInclude(t => t.Instructor)
            );

            var dtoList = assignedList.Select(x => new TrainingCardDto
            {
                Id = x.CurrAccTrainings.Training.Id,
                Title = x.CurrAccTrainings.Training.Title, // TypeCardEntity.Title
                Description = x.CurrAccTrainings.Training.Description, // TypeCardEntity.Description
                ImageUrl = GetHeaderImage(x.CurrAccTrainings.Training.HeaderImage), // Helper Metod
                InstructorName = x.CurrAccTrainings.Training.Instructor != null
                    ? $"{x.CurrAccTrainings.Training.Instructor.User.Name} {x.CurrAccTrainings.Training.Instructor.User.SurName}" // Instructor -> User -> Name
                    : "T1 Akademi",
                IsAssigned = true,
                // Rating entity'de yoksa null geçiyoruz veya Review tablosundan hesaplanmalı
                Rating = 4.8
            }).ToList();

            return ProduceSuccessResponse(dtoList);
        }

        // 4. ÖNERİLEN EĞİTİMLER
        public async Task<Response<List<TrainingCardDto>>> GetRecommendedTrainingsAsync()
        {
            long userId = GetCurrentUserId();

            // 1. Zaten atanmış eğitimlerin 'TrainingId'lerini bul
            // Dikkat: CurrAccTrainingUser -> CurrAccTrainings -> TrainingId
            var myAssignments = await currAccTrainingUserRepository.GetListWithNoLockAsync<CurrAccTrainingUser>(
                predicate: x => x.UserId == userId,
                include: i => i.Include(x => x.CurrAccTrainings)
            );

            var myTrainingIds = myAssignments
                                .Where(x => x.CurrAccTrainings != null)
                                .Select(x => x.CurrAccTrainings.TrainingId)
                                .ToList();

            // 2. Filtrele (IsActive ve benim sahip olmadıklarım)
            var predicateBuilder = PredicateBuilder.New<Training>();
            predicateBuilder = predicateBuilder.And(x => x.IsActive == true);

            if (myTrainingIds.Any())
            {
                predicateBuilder = predicateBuilder.And(x => !myTrainingIds.Contains(x.Id));
            }

            // En son eklenen 5 eğitimi getir
            var recommendations = await trainingRepository.GetPagedListWithNoLockAsync(
                predicate: predicateBuilder,
                orderBy: o => o.OrderByDescending(x => x.CreatedDate),
                take : 5,
                include: i => i.Include(t => t.Instructor).ThenInclude(ins => ins.User)
            );

            var dtoList = recommendations.Select(x => new TrainingCardDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                ImageUrl = GetHeaderImage(x.HeaderImage),
                InstructorName = x.Instructor != null && x.Instructor.User != null
                    ? $"{x.Instructor.User.Name} {x.Instructor.User.SurName}"
                    : "T1 Akademi",
                IsAssigned = false,
                Rating = 4.5
            }).ToList();

            return ProduceSuccessResponse(dtoList);
        }
    }
}