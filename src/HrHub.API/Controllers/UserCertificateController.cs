using HrHub.Abstraction.Result;
using HrHub.Application.Managers.UserCertificateManagers; // Manager Namespace
using HrHub.Core.Controllers; // Base Controller Namespace
using HrHub.Domain.Contracts.Dtos.CertificateDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HrHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCertificateController : ApiControllerBase
    {
        private readonly IUserCertificateManager _certificateManager;

        public UserCertificateController(IUserCertificateManager certificateManager)
        {
            _certificateManager = certificateManager;
        }

        /// <summary>
        /// Belirtilen eğitim için sertifika talebi oluşturur.
        /// Eğer şartlar sağlanıyorsa (Hangfire) arka planda üretim başlar.
        /// </summary>
        [HttpPost("[Action]/{trainingId}")]
        public async Task<Response<UserCertificateDto>> CreateCertificateRequest(long trainingId)
        {
            // Frontend'de "Eğitimi Bitir" veya "Sertifikamı Al" butonuna basınca burası çağrılır.
            return await _certificateManager.CreateCertificateRequestAsync(trainingId).ConfigureAwait(false);
        }

        /// <summary>
        /// Sertifika ID'sine göre detayları getirir.
        /// </summary>
        [HttpGet("[Action]/{certificateId}")]
        public async Task<Response<UserCertificateDto>> GetById(Guid certificateId)
        {
            return await _certificateManager.GetCertificateAsync(certificateId).ConfigureAwait(false);
        }

        /// <summary>
        /// Kullanıcının belirtilen eğitimdeki sertifikasını getirir.
        /// </summary>
        [HttpGet("[Action]/{trainingId}")]
        public async Task<Response<UserCertificateDto>> GetByTrainingId(long trainingId)
        {
            return await _certificateManager.GetCertificateByTrainingIdAsync(trainingId).ConfigureAwait(false);
        }

        /// <summary>
        /// Giriş yapmış kullanıcının kazandığı TÜM sertifikaları listeler.
        /// (Dashboard'daki "Sertifikalarım" tablosu burayı kullanacak)
        /// </summary>
        [HttpGet("[Action]")]
        public async Task<Response<List<UserCertificateDto>>> GetMyCertificates()
        {
            return await _certificateManager.GetMyAllCertificatesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Şirket/Kurum içindeki tüm sertifikaları listeler (Admin/Yönetici paneli için).
        /// </summary>
        [HttpGet("[Action]")]
        // [Authorize(Roles = "Admin,Manager")] // Gerekirse rol kontrolü ekleyebilirsin
        public async Task<Response<List<UserCertificateDto>>> GetCompanyCertificates()
        {
            return await _certificateManager.GetAllCurrAccCertificatesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Sertifika doğrulama sayfası veya QR kod okutulduğunda çalışacak endpoint.
        /// Genelde login gerektirmez (AllowAnonymous).
        /// </summary>
        [HttpGet("[Action]/{certificateId}")]
        [AllowAnonymous] // Dışarıdan doğrulama yapılacaksa login şartı kaldırılır
        public async Task<Response<UserCertificateDto>> Verify(Guid certificateId)
        {
            return await _certificateManager.VerifyCertificateAsync(certificateId).ConfigureAwait(false);
        }
    }
}