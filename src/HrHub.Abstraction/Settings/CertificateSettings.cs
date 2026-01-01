namespace HrHub.Abstraction.Settings
{
    public class CertificateSettings
    {
        public string DefaultTemplatePath { get; set; }
        public string EmailTemplatePath { get; set; }
        public string SmsTemplatePath { get; set; } // SMS template eklendi
        public string OutputFolder { get; set; }
    }
}