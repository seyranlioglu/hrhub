using HrHub.Abstraction.Attributes;

namespace HrHub.Abstraction.Settings
{
    [AppSetting("MailServiceSettings")]
    public class MailServiceSettings : ISettingsBase
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public int SSLPort { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
