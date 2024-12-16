using HrHub.Abstraction.Attributes;

namespace HrHub.Abstraction.Settings
{
    [AppSetting("SmsServiceSettings")]
    public class SmsServiceSettings : ISettingsBase
    {
        public bool IsActive { get; set; }
        public string BaseAddress { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Originator { get; set; }
        public string Channel { get; set; }
        public string BlackListFilter { get; set; }
        public int IysFilter { get; set; }
        public string BrandCode { get; set; }
        public string RetailerCode { get; set; }
        public string RecipientType { get; set; }

    }
}
