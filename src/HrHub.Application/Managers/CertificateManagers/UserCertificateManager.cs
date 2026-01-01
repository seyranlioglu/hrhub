using AutoMapper;
using FluentValidation.Results;
using Hangfire; // Hangfire namespace'i
using HrHub.Abstraction.Extensions;
using HrHub.Abstraction.Result;
using HrHub.Abstraction.StatusCodes;
using HrHub.Application.BusinessRules.CommentVoteBusinessRules;
using HrHub.Application.BusinessRules.UserCertificateBusinessRules;
using HrHub.Core.Base;
using HrHub.Core.Data.Repository;
using HrHub.Core.Helpers;
using HrHub.Domain.Contracts.Dtos.CertificateDtos;
using HrHub.Domain.Contracts.Dtos.CommentVoteDtos;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HrHub.Application.Managers.UserCertificateManagers
{
    public class UserCertificateManager : ManagerBase, IUserCertificateManager
    {
        private readonly IHrUnitOfWork _hrUnitOfWork;
        private readonly IMapper _mapper;
        private readonly Repository<UserCertificate> _userCertificateRepository;
        private readonly Repository<CurrAccTrainingUser> _currAccTrainingUserRepository;
        private readonly Repository<CertificateTemplate> _certificateTemplateRepository;
        private readonly IBackgroundJobClient _backgroundJobClient; // Hangfire Client

        public UserCertificateManager(
            IHttpContextAccessor httpContextAccessor,
            IHrUnitOfWork hrUnitOfWork,
            IMapper mapper,
            IBackgroundJobClient backgroundJobClient) : base(httpContextAccessor)
        {
            _hrUnitOfWork = hrUnitOfWork;
            _mapper = mapper;
            _userCertificateRepository = _hrUnitOfWork.CreateRepository<UserCertificate>();
            _currAccTrainingUserRepository = _hrUnitOfWork.CreateRepository<CurrAccTrainingUser>();
            _certificateTemplateRepository = _hrUnitOfWork.CreateRepository<CertificateTemplate>();
            _backgroundJobClient = backgroundJobClient;
        }

        // 1. ADIM: API'den Gelen İstek (UserId Token'dan alınır)
public async Task<Response<UserCertificateDto>> CreateCertificateRequestAsync(long trainingId)
        {
            var currentUserId = GetCurrentUserId();

            // 1. Eğitim kaydını bul
            var trainingUser = await _currAccTrainingUserRepository.GetAsync(
                predicate: x => x.UserId == currentUserId && x.CurrAccTrainings.TrainingId == trainingId && x.IsActive == true,
                include: i => i.Include(x => x.CurrAccTrainings)
            );

            if (trainingUser == null)
            {
                return ProduceFailResponse<UserCertificateDto>("Eğitim kaydı bulunamadı.", HrStatusCodes.Status404NotFound);
            }

            // 2. BUSINESS RULE KULLANIMI (Eski manuel sorgu yerine)
            // Bu kural, bu kullanıcının bu kaydı (trainingUser.Id) için sertifikası var mı bakar.

            // DÜZELTME BURADA: long yerine DTO kullanılıyor
            var checkDto = new CheckCertificateEligibilityDto
            {
                CurrAccTrainingUsersId = trainingUser.Id
            };

            if (ValidationHelper.RuleBasedValidator<CheckCertificateEligibilityDto>(checkDto, typeof(IUserCertificateBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<UserCertificateDto>();

            // 3. Şablonu Bul
            var template = await _certificateTemplateRepository.GetAsync(
                predicate: x => x.IsActive == true && x.IsDelete != true
            );

            if (template == null)
            {
                return ProduceFailResponse<UserCertificateDto>("Sertifika şablonu bulunamadı.", HrStatusCodes.Status111DataNotFound);
            }

            // 4. Kayıt ve Queue işlemleri (Aynen devam)
            var newCertId = Guid.NewGuid();
            var entity = new UserCertificate
            {
                CertificateId = newCertId,
                CurrAccTrainingUsersId = trainingUser.Id,
                CertificateTemplateId = template.Id,
                CreateUserId = currentUserId,
                CreatedDate = DateTime.UtcNow,
                IsActive = false,
                IsDelete = false,
                VerificationURL = "Processing..."
            };

            await _userCertificateRepository.AddAsync(entity);
            await _hrUnitOfWork.SaveChangesAsync();

            _backgroundJobClient.Enqueue<IUserCertificateManager>(x => x.ProcessCertificateGenerationAsync(newCertId));

            var responseDto = _mapper.Map<UserCertificateDto>(entity);
            return ProduceSuccessResponse(responseDto);
        }

        // 2. ADIM: Worker'ın Çalıştıracağı Method (UserId YOK, HttpContext YOK)
        public async Task ProcessCertificateGenerationAsync(Guid certificateId)
        {
            // Bu method bir HTTP isteği kapsamında çalışmaz. Background Job içindedir.

            // a. Kaydı çek
            var certEntity = await _userCertificateRepository.GetAsync(
                predicate: x => x.CertificateId == certificateId,
                include: i => i.Include(x => x.CurrAccTrainingUser).ThenInclude(u => u.User) // Kullanıcı bilgisi gerekirse buradan relation ile alınır
            );

            if (certEntity == null) return; // Hata loglanabilir.

            try
            {
                // b. PUAN HESAPLAMA SİMÜLASYONU (Burada ExamManager vb. çağrılabilir)
                // var score = _examManager.CalculateScore(certEntity.CurrAccTrainingUser.UserId)...
                // certEntity.Score = score;

                // c. PDF OLUŞTURMA SİMÜLASYONU
                // var pdfPath = _pdfService.GeneratePdf(certEntity);
                // certEntity.PdfUrl = pdfPath;

                // d. Alanları Güncelle ve Aktif Et
                certEntity.VerificationURL = $"https://hrhub.com/verify/{certificateId}";
                certEntity.TrainerName = "Sistem"; // Dinamik doldurulmalı
                certEntity.IsActive = true; // ARTIK GÖRÜNTÜLENEBİLİR

                _userCertificateRepository.Update(certEntity);
                await _hrUnitOfWork.SaveChangesAsync();

                // e. Opsiyonel: Bildirim Gönder (NotificationManager.SendNotificationAsync...)
            }
            catch (Exception ex)
            {
                // Hata durumunda log atılabilir veya certEntity.ErrorDescription doldurulabilir.
                // throw ex; // Hangfire'ın retry mekanizmasını tetiklemek istersen fırlat.
            }
        }

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

        public Task<Response<UserCertificateDto>> CreateCertificateAsync(long userId, long trainingId)
        {
            throw new NotImplementedException();
        }
    }
}