namespace HrHub.Domain.Contracts.Enums
{
    public enum CertificateStatus
    {
        /// <summary>
        /// Sertifika oluşturma talebi alındı, kuyrukta veya işleniyor.
        /// </summary>
        Preparing = 1,

        /// <summary>
        /// İşlem başarıyla bitti, PDF oluşturuldu ve mail atıldı.
        /// </summary>
        Completed = 2,

        /// <summary>
        /// Bir hata oluştu (Örn: HTML render hatası).
        /// </summary>
        Failed = 3
    }
}