using HrHub.Abstraction.Attributes;
using HrHub.Abstraction.Enums;

namespace HrHub.Abstraction.Settings
{
    [AppSetting("Databases:MainDb")]
    public class DatabaseSettings : ISettingsBase
    {
        public string DbKey { get; set; }
        public DatabaseType DatabaseType { get; set; }
        public string ConnectionString { get; set; }
    }
}
