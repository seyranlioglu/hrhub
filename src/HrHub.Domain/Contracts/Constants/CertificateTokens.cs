namespace HrHub.Domain.Constants
{
    // Sertifika şablonlarında kullanılacak anahtar kelimeler
    public static class CertificateTokens
    {
        public const string UserName = "{{USER_NAME}}";           // Ad Soyad
        public const string CourseName = "{{COURSE_NAME}}";       // Eğitim Adı
        public const string Date = "{{DATE}}";                    // Sertifika Tarihi
        public const string Duration = "{{DURATION}}";            // Eğitim Süresi (Saat)
        public const string Instructor = "{{INSTRUCTOR}}";        // Eğitmen Adı
        public const string VerificationUrl = "{{VERIFY_URL}}";   // QR Kod veya Link
        public const string CertificateId = "{{CERT_ID}}";        // Tekil ID
    }
}