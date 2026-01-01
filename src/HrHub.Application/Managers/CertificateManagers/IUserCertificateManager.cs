using HrHub.Abstraction.Result;
using HrHub.Core.Base;
using HrHub.Domain.Contracts.Dtos.CertificateDtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrHub.Application.Managers.UserCertificateManagers
{
    public interface IUserCertificateManager : IBaseManager
    {
        /// <summary>
        /// Kullanıcı (UI) tetikler. 
        /// DB'ye 'Hazırlanıyor' statüsünde kayıt atar ve Hangfire Job'ını kuyruğa ekler.
        /// </summary>
        Task<Response<UserCertificateDto>> CreateCertificateRequestAsync(long trainingId);

        /// <summary>
        /// SADECE HANGFIRE TARAFINDAN ÇAĞRILIR.
        /// Sertifika hesaplamalarını yapar, PDF oluşturur ve kaydı aktif eder.
        /// </summary>
        Task ProcessCertificateGenerationAsync(Guid certificateId);

        /// <summary>
        /// Sertifikanın benzersiz ID'sine (Guid) göre detayını getirir.
        /// </summary>
        Task<Response<UserCertificateDto>> GetCertificateAsync(Guid certificateId);

        /// <summary>
        /// Login olmuş kullanıcının, verilen eğitim ID'sine (Long) ait sertifikasını getirir.
        /// </summary>
        Task<Response<UserCertificateDto>> GetCertificateByTrainingIdAsync(long trainingId);

        /// <summary>
        /// Login olmuş kullanıcının tüm sertifikalarını getirir.
        /// </summary>
        Task<Response<List<UserCertificateDto>>> GetMyAllCertificatesAsync();

        /// <summary>
        /// Kurum yöneticisi (MainUser) ise, kurumdaki dağıtılmış tüm sertifikaları listeler.
        /// </summary>
        Task<Response<List<UserCertificateDto>>> GetAllCurrAccCertificatesAsync();

        /// <summary>
        /// EKSTRA: Sertifikanın geçerliliğini doğrulamak (QR kod veya Link için) kullanılır.
        /// </summary>
        Task<Response<UserCertificateDto>> VerifyCertificateAsync(Guid certificateId);

    }
}