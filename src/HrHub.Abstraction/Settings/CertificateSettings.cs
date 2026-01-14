using HrHub.Abstraction.Settings;

namespace HrHub.Abstraction.Settings
{
    public class CertificateSettings : ISettingsBase
    {
        public string StorageType { get; set; } // "Local", "FTP"
        public string LocalPath { get; set; } = "wwwroot/certificates";

        // FTP
        public string FtpHost { get; set; }
        public string FtpUser { get; set; }
        public string FtpPass { get; set; }
        public int FtpPort { get; set; } = 21;

        // Templates & Notification
        public string DefaultTemplatePath { get; set; } // Örn: "default_template.html"
        public string EmailTemplatePath { get; set; }
        public string SmsTemplatePath { get; set; }

        // Fallback Certificate (DB boşsa kullanılacak)
        public DefaultCertificateInfo DefaultCertificate { get; set; }

        // Sertifika Metinleri (Parametrik Yazılar)
        public CertificateTexts Texts { get; set; }
    }

    public class DefaultCertificateInfo
    {
        public string Name { get; set; } = "Katılım Sertifikası";
        public string TemplateFileName { get; set; } = "default.html";
    }

    public class CertificateTexts
    {
        public string Title { get; set; } = "SERTİFİKA";
        public string Subtitle { get; set; } = "BAŞARI BELGESİ";
        public string BodyText { get; set; } = "Sayın {{StudentName}}, {{TrainingName}} eğitimini başarıyla tamamlayarak bu belgeyi almaya hak kazanmıştır.";
        public string ScoreLabel { get; set; } = "Başarı Puanı:";
        public string DateLabel { get; set; } = "Tarih:";
        public string SignerName { get; set; } = "HrHub Yönetim";
        public string SignerTitle { get; set; } = "Eğitim Direktörü";
    }
}