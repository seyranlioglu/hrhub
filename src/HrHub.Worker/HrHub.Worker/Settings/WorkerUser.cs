using HrHub.Abstraction.Attributes;
using HrHub.Abstraction.Settings;

namespace HrHub.Worker.Settings
{
    [AppSetting("WorkerSettings:DashboardSettings")]
    public class WorkerUser : ISettingsBase
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
