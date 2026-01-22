using HrHub.Abstraction.Enums;
using System.Collections.Concurrent;
using System.Text;

namespace HrHub.Application.Helpers
{
    public static class MailHelper
    {
        // Şablonları RAM'de tutmak için Cache (Her seferinde diskten okumasın diye)
        // Key: MailType, Value: HTML İçeriği
        private static readonly ConcurrentDictionary<MailType, string> _templateCache = new();

        // Şablonların olduğu klasör yolu (wwwroot/EmailTemplates)
        private static readonly string _baseTemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmailTemplates");

        /// <summary>
        /// Mail tipine göre ilgili HTML dosyasını okur ve ham string olarak döner.
        /// Parametre değişimi yapılmaz.
        /// </summary>
        public static string GetMailBody(MailType mailType, StateEnum? state = null)
        {
            // Cache'te varsa oradan getir, yoksa diskten oku ve cache'e ekle
            return _templateCache.GetOrAdd(mailType, type => LoadTemplateFromDisk(type));
        }

        private static string LoadTemplateFromDisk(MailType mailType)
        {
            string fileName = GetTemplateFileName(mailType);
            string fullPath = Path.Combine(_baseTemplatePath, fileName);

            if (!File.Exists(fullPath))
            {
                // Dosya yoksa log atılabilir veya boş dönülebilir. 
                // Geçici olarak hata fırlatmayıp default bir html dönebiliriz ya da exception fırlatabiliriz.
                throw new FileNotFoundException($"Mail şablon dosyası bulunamadı: {fullPath}");
            }

            return File.ReadAllText(fullPath, Encoding.UTF8);
        }

        private static string GetTemplateFileName(MailType mailType)
        {
            return mailType switch
            {
                MailType.VerifyEmail => "VerifyEmail.html",
                MailType.AddUser => "AddUser.html",
                MailType.ChangePasswordBySuperAdmin => "ChangePassword.html",
                // İleride eklenecek diğer tipler...
                _ => "Default.html"
            };
        }
    }
}