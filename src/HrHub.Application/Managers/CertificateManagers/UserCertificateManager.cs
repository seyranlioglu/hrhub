using AutoMapper;
using Hangfire;
using HrHub.Abstraction.Result;
using HrHub.Abstraction.Settings;
using HrHub.Abstraction.StatusCodes;
using HrHub.Application.Helpers;
using HrHub.Core.Base;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Contracts.Dtos.CertificateDtos;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HrHub.Application.Managers.UserCertificateManagers
{
    public class UserCertificateManager : ManagerBase, IUserCertificateManager
    {
        private readonly IHrUnitOfWork _hrUnitOfWork;
        private readonly IWebHostEnvironment _env; // Eklendi
        private readonly IMapper _mapper;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly CertificateSettings _certSettings;

        // Repositories
        private readonly Repository<UserCertificate> _userCertificateRepository;
        private readonly Repository<TrainingContent> _trainingContentRepository;
        private readonly Repository<UserContentsViewLog> _userViewLogRepository;
        private readonly Repository<UserExam> _userExamRepository;
        private readonly Repository<CertificateType> _certificateTypeRepository;
        private readonly Repository<CurrAccTrainingUser> _currAccTrainingUserRepository;
        private readonly Repository<CertificateTemplate> _certificateTemplateRepository;

        public UserCertificateManager(
            IHttpContextAccessor httpContextAccessor,
            IHrUnitOfWork hrUnitOfWork,
            IWebHostEnvironment env, // Inject edildi
            IMapper mapper,
            IBackgroundJobClient backgroundJobClient,
            IOptions<CertificateSettings> certSettings) : base(httpContextAccessor)
        {
            _hrUnitOfWork = hrUnitOfWork;
            _env = env;
            _mapper = mapper;
            _backgroundJobClient = backgroundJobClient;
            _certSettings = certSettings.Value;

            _userCertificateRepository = _hrUnitOfWork.CreateRepository<UserCertificate>();
            _trainingContentRepository = _hrUnitOfWork.CreateRepository<TrainingContent>();
            _userViewLogRepository = _hrUnitOfWork.CreateRepository<UserContentsViewLog>();
            _userExamRepository = _hrUnitOfWork.CreateRepository<UserExam>();
            _certificateTypeRepository = _hrUnitOfWork.CreateRepository<CertificateType>();
            _currAccTrainingUserRepository = _hrUnitOfWork.CreateRepository<CurrAccTrainingUser>();
            _certificateTemplateRepository = _hrUnitOfWork.CreateRepository<CertificateTemplate>();
        }

        // 1. Trigger Method (API veya Manager çağırır)
        public async Task<Response<UserCertificateDto>> CreateCertificateRequestAsync(long trainingId)
        {
            var currentUserId = GetCurrentUserId();

            // 1. Kullanıcı-Eğitim ilişkisini bul
            var trainingUser = await _currAccTrainingUserRepository.GetAsync(
                predicate: x => x.UserId == currentUserId &&
                                x.CurrAccTrainings.TrainingId == trainingId &&
                                x.IsActive == true,
                include: i => i.Include(x => x.CurrAccTrainings)
            );

            if (trainingUser == null)
            {
                return ProduceFailResponse<UserCertificateDto>("Eğitim ataması bulunamadı.", HrStatusCodes.Status404NotFound);
            }

            // 2. Mevcut sertifika kontrolü
            var existingCert = await _userCertificateRepository.GetAsync(
                predicate: x => x.CurrAccTrainingUsersId == trainingUser.Id && x.IsDelete == false
            );

            if (existingCert != null)
            {
                return ProduceSuccessResponse(_mapper.Map<UserCertificateDto>(existingCert));
            }

            // 3. Sertifikayı "İşleniyor" durumunda oluştur
            var newCertId = Guid.NewGuid();
            var entity = new UserCertificate
            {
                CertificateId = newCertId,
                CurrAccTrainingUsersId = trainingUser.Id,
                CreateUserId = currentUserId,
                CreatedDate = DateTime.UtcNow,
                IsActive = false,
                IsDelete = false,
                VerificationURL = "Processing..."
            };

            await _userCertificateRepository.AddAsync(entity);
            await _hrUnitOfWork.SaveChangesAsync();

            // 4. Hangfire'a at
            _backgroundJobClient.Enqueue<IUserCertificateManager>(x => x.ProcessCertificateGenerationAsync(newCertId));

            return ProduceSuccessResponse(_mapper.Map<UserCertificateDto>(entity));
        }

        // 2. Worker Method (Hangfire çalıştırır)
        public async Task ProcessCertificateGenerationAsync(Guid certificateId)
        {
            var certEntity = await _userCertificateRepository.GetAsync(
                predicate: x => x.CertificateId == certificateId,
                include: i => i.Include(x => x.CurrAccTrainingUser).ThenInclude(u => u.User)
                               .Include(x => x.CurrAccTrainingUser).ThenInclude(ctu => ctu.CurrAccTrainings).ThenInclude(ct => ct.Training)
            );

            if (certEntity == null) return;

            try
            {
                long userId = certEntity.CurrAccTrainingUser.UserId;
                long trainingId = certEntity.CurrAccTrainingUser.CurrAccTrainings.TrainingId;

                // --- A. ZORUNLU İÇERİK KONTROLÜ ---
                if (!await CheckMandatoryContents(userId, trainingId))
                {
                    await SetCertificateFailed(certEntity, "Zorunlu içerikler tamamlanmadı.");
                    return;
                }

                // --- B. PUAN HESAPLAMA ---
                double finalScore = await CalculateWeightedScore(userId, trainingId);
                certEntity.Score = finalScore;

                // --- C. PUANA GÖRE TİP BULMA (AKILLI SEÇİM) ---

                // 1. Önce tam aralık eşleşmesi ara
                var certType = await _certificateTypeRepository.GetAsync(
                    predicate: x => finalScore >= x.MinScore && finalScore <= x.MaxScore,
                    include: i => i.Include(t => t.CertificateTemplates)
                );

                CertificateTemplate? selectedTemplate = null;

                // 2. Tam eşleşme yoksa, EN YAKIN tipi bul (Örn: Puan 95 ama Max 90 var, Gold ver)
                if (certType == null)
                {
                    // Puanı en yüksek olan ama bizim puanımızdan küçük olanı bul (En üst limit)
                    // Veya bizim puanımızdan büyük en küçük (Alt limit)
                    // Basit mantık: Puanımız yüksekse en iyi sertifikayı hak etmiştir.

                    var allTypes = await _certificateTypeRepository.GetListAsync(
                        include: i => i.Include(t => t.CertificateTemplates)
                    );

                    if (allTypes.Any())
                    {
                        // Puana en yakın olanı seç (MinScore'a göre sırala, bize en yakın olanı al)
                        // Bu basit bir yaklaşım: Hangi aralığa en yakın? 
                        // Örn: Puan 20. Aralıklar: 50-70, 71-100. -> 50-70'e daha yakın.
                        certType = allTypes.OrderBy(x => Math.Abs(x.MinScore - finalScore)).FirstOrDefault();
                    }
                }

                string templateFileName = "";

                if (certType != null && certType.CertificateTemplates.Any(t => t.IsActive == true))
                {
                    // DB'den tip ve şablon bulundu
                    selectedTemplate = certType.CertificateTemplates.FirstOrDefault(t => t.IsActive == true);
                    if (selectedTemplate != null)
                    {
                        templateFileName = selectedTemplate?.Title;
                        certEntity.CertificateTemplateId = selectedTemplate.Id;
                    }

                }
                else
                {
                    // 3. FALLBACK: DB'de hiç tip yok veya şablon yok -> APPSETTINGS KULLAN
                    // DefaultCertificateInfo null kontrolü yapılmalı
                    if (_certSettings.DefaultCertificate != null)
                    {
                        templateFileName = _certSettings.DefaultCertificate.TemplateFileName;
                        // TemplateId null kalabilir veya "Default" diye bir kayıt varsa o atanabilir.
                        // certEntity.CertificateTemplateId = null; 
                    }
                    else
                    {
                        await SetCertificateFailed(certEntity, "Sertifika tipi bulunamadı ve Default ayar yok.");
                        return;
                    }
                }

                // --- D. HTML OKUMA ---
                string templateFolderPath = Path.Combine(_env.WebRootPath, "CertificateTemplate");
                string templateFilePath = Path.Combine(templateFolderPath, templateFileName);

                if (!File.Exists(templateFilePath))
                {
                    await SetCertificateFailed(certEntity, $"Şablon dosyası diskte yok: {templateFileName}");
                    return;
                }

                string htmlBody = await File.ReadAllTextAsync(templateFilePath);

                // --- E. PDF DATASI HAZIRLAMA (AYARLARDAN METİNLERLE) ---
                var studentName = $"{certEntity.CurrAccTrainingUser.User.Name} {certEntity.CurrAccTrainingUser.User.SurName}";
                var trainingName = certEntity.CurrAccTrainingUser.CurrAccTrainings.Training.Title;

                // Body text içindeki dinamik alanları (Örn: {{StudentName}}) önce burada replace ediyoruz
                // Çünkü bu metin HTML içine {{BodyText}} olarak gidecek.
                string processedBodyText = _certSettings.Texts.BodyText
                    .Replace("{{StudentName}}", studentName)
                    .Replace("{{TrainingName}}", trainingName)
                    .Replace("{{Score}}", finalScore.ToString("F0"));

                var pdfData = new CertificateDataModel
                {
                    StudentName = studentName,
                    TrainingName = trainingName,
                    CompletionDate = DateTime.Now.ToString("dd.MM.yyyy"),
                    Score = finalScore,
                    VerificationCode = certificateId.ToString(),

                    // AppSettings'den gelen metinler
                    Title = _certSettings.Texts.Title,
                    Subtitle = _certSettings.Texts.Subtitle,
                    BodyText = processedBodyText, // İşlenmiş metin
                    ScoreLabel = _certSettings.Texts.ScoreLabel,
                    DateLabel = _certSettings.Texts.DateLabel,
                    SignerName = _certSettings.Texts.SignerName,
                    SignerTitle = _certSettings.Texts.SignerTitle
                };

                // Helper Çağrısı
                string filledHtml = CertificateHelper.PrepareHtmlContent(htmlBody, pdfData);
                byte[] pdfBytes = CertificateHelper.GeneratePdfBytes(filledHtml);
                string fileName = $"{certificateId}.pdf";

                // Kaydetme
                var saveResult = CertificateHelper.SaveCertificate(pdfBytes, fileName, _certSettings);

                if (!saveResult.IsSuccess())
                {
                    await SetCertificateFailed(certEntity, saveResult.Header.Msg);
                    return;
                }

                // --- F. BAŞARILI SONUÇ ---
                certEntity.GeneratedFilePath = saveResult.Body; // Kaydedilen yol
                certEntity.VerificationURL = $"https://hrhub.com/verify/{certificateId}";
                certEntity.IsActive = true;
                certEntity.UpdateDate = DateTime.UtcNow;

                _userCertificateRepository.Update(certEntity);
                await _hrUnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await SetCertificateFailed(certEntity, $"Hata: {ex.Message}");
            }
        }

        #region Private Helpers

        private async Task<bool> CheckMandatoryContents(long userId, long trainingId)
        {
            // 1. Bu eğitimin TÜM AKTİF içeriklerini bul (IsMandatory olmadığı için hepsi zorunlu)
            var contentIds = await _trainingContentRepository.GetListAsync(
                predicate: x => x.TrainingSection.TrainingId == trainingId &&
                                x.IsActive == true,
                selector: x => x.Id
            );

            if (!contentIds.Any()) return true; // İçerik yoksa şart sağlanmış sayılır.

            // 2. Kullanıcının izlediklerini kontrol et
            // UserContentsViewLog tablosunda 'TrainingContentId' ve 'IsCompleted' alanları var.
            var watchedCount = await _userViewLogRepository.CountAsync(
                predicate: x => x.CurrAccTrainingUser.UserId == userId &&
                                contentIds.Contains(x.TrainingContentId) &&
                                x.IsCompleted == true
            );

            // Tüm içerikler izlendi mi?
            return watchedCount >= contentIds.Count();
        }

        private async Task<double> CalculateWeightedScore(long userId, long trainingId)
        {
            // 1. SINAVLARI BUL (ExamId olan Content'ler)
            var examIdsInTraining = await _trainingContentRepository.GetListAsync(
                predicate: x => x.TrainingSection.TrainingId == trainingId &&
                                x.ExamId != null &&
                                x.IsActive == true,
                selector: x => x.ExamId.Value
            );

            double examScore = 0;
            if (examIdsInTraining.Any())
            {
                // UserExam içinde ExamId doğrudan var.
                // IsSucces (Tek s ile) kontrolü.
                var userExams = await _userExamRepository.GetListAsync(
                    predicate: x => x.CurrAccTrainingUser.UserId == userId &&
                                    examIdsInTraining.Contains(x.ExamVersion.ExamId) &&
                                    x.IsSuccess == true,
                    selector: x => x.ExamScore
                );

                if (userExams.Any())
                {
                    // Null puanları 0 say, ortalama al.
                    examScore = Convert.ToDouble(userExams.Average(s => s ?? 0));
                }
            }
            else
            {
                examScore = 100; // Sınavsız eğitim
            }

            // 2. İZLEME ORANI
            // CountAsync kullanıyoruz (GetCountAsync değil)
            var totalContents = await _trainingContentRepository.CountAsync(
                predicate: x => x.TrainingSection.TrainingId == trainingId && x.IsActive == true
            );

            if (totalContents == 0) return examScore;

            var watchedContents = await _userViewLogRepository.CountAsync(
                predicate: x => x.CurrAccTrainingUser.UserId == userId &&
                                x.IsCompleted == true &&
                                x.TrainingContent.TrainingSection.TrainingId == trainingId
            );

            double watchRatio = (double)watchedContents / totalContents;
            if (watchRatio > 1) watchRatio = 1;

            return examScore * watchRatio;
        }

        private async Task SetCertificateFailed(UserCertificate entity, string reason)
        {
            entity.IsActive = false;
            entity.VerificationURL = $"Failed: {reason}";
            _userCertificateRepository.Update(entity);
            await _hrUnitOfWork.SaveChangesAsync();
        }

        #endregion

        public async Task<Response<UserCertificateDto>> GetCertificateAsync(Guid certificateId)
        {
            var entity = await _userCertificateRepository.GetAsync(
                predicate: x => x.CertificateId == certificateId && x.IsDelete != true,
                include: i => i.Include(x => x.CertificateTemplate)
                               .Include(x => x.CurrAccTrainingUser).ThenInclude(u => u.CurrAccTrainings).ThenInclude(t => t.Training)
            );

            if (entity == null)
            {
                return ProduceFailResponse<UserCertificateDto>(
                    "Sertifika bulunamadı.",
                    HrStatusCodes.Status111DataNotFound
                );
            }

            var dto = _mapper.Map<UserCertificateDto>(entity);
            return ProduceSuccessResponse(dto);
        }

        public async Task<Response<UserCertificateDto>> GetCertificateByTrainingIdAsync(long trainingId)
        {
            var currentUserId = GetCurrentUserId();

            var entity = await _userCertificateRepository.GetAsync<UserCertificate>(
                predicate: x => x.CurrAccTrainingUser.UserId == currentUserId &&
                                x.CurrAccTrainingUser.CurrAccTrainings.TrainingId == trainingId &&
                                x.IsDelete != true,
                include: i => i.Include(x => x.CertificateTemplate)
                               .Include(x => x.CurrAccTrainingUser).ThenInclude(u => u.CurrAccTrainings).ThenInclude(t => t.Training)
            );

            if (entity == null)
            {
                return ProduceFailResponse<UserCertificateDto>(
                    "Bu eğitim için sertifikanız bulunmamaktadır.",
                    HrStatusCodes.Status111DataNotFound
                );
            }

            var dto = _mapper.Map<UserCertificateDto>(entity);
            return ProduceSuccessResponse(dto);
        }

        public async Task<Response<List<UserCertificateDto>>> GetMyAllCertificatesAsync()
        {
            var currentUserId = GetCurrentUserId();

            var list = await _userCertificateRepository.GetListAsync(
                predicate: x => x.CurrAccTrainingUser.UserId == currentUserId && x.IsDelete != true,
                include: i => i.Include(x => x.CertificateTemplate)
                               .Include(x => x.CurrAccTrainingUser).ThenInclude(u => u.CurrAccTrainings).ThenInclude(t => t.Training)
            );

            var dtoList = _mapper.Map<List<UserCertificateDto>>(list);
            return ProduceSuccessResponse(dtoList);
        }

        public async Task<Response<List<UserCertificateDto>>> GetAllCurrAccCertificatesAsync()
        {
            var currAccId = GetCurrAccId(); // BaseManager'dan gelir

            var list = await _userCertificateRepository.GetListAsync(
                predicate: x => x.CurrAccTrainingUser.CurrAccTrainings.CurrAccId == currAccId && x.IsDelete != true,
                include: i => i.Include(x => x.CertificateTemplate)
                               .Include(x => x.CurrAccTrainingUser).ThenInclude(u => u.CurrAccTrainings).ThenInclude(t => t.Training)
            );

            var dtoList = _mapper.Map<List<UserCertificateDto>>(list);
            return ProduceSuccessResponse(dtoList);
        }

        public async Task<Response<UserCertificateDto>> VerifyCertificateAsync(Guid certificateId)
        {
            // Doğrulama işlemi genelde login gerektirmez (Public Endpoint olabilir)
            // Ancak Manager içinde olduğumuz için DB kontrolü yapıyoruz.

            var entity = await _userCertificateRepository.GetAsync(
                predicate: x => x.CertificateId == certificateId && x.IsDelete != true,
                include: i => i.Include(x => x.CertificateTemplate)
                               .Include(x => x.CurrAccTrainingUser).ThenInclude(u => u.User) // Sertifika kime ait?
                               .Include(x => x.CurrAccTrainingUser).ThenInclude(u => u.CurrAccTrainings).ThenInclude(t => t.Training)
            );

            if (entity == null)
            {
                return ProduceFailResponse<UserCertificateDto>(
                    "Geçersiz Sertifika ID.",
                    HrStatusCodes.Status111DataNotFound
                );
            }

            var dto = _mapper.Map<UserCertificateDto>(entity);
            return ProduceSuccessResponse(dto);
        }
    }
}