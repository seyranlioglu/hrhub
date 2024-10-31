using HrHub.Abstraction.Attributes;
using HrHub.Abstraction.Settings;

namespace HrHub.Worker.Settings
{
    [AppSetting("WorkerSettings:Workers")]
    public class WorkerSettings : ISettingsBase
    {
        public string JobId { get; set; }
        public string CronExpression { get; set; }
        public bool Enabled { get; set; }
        public int LastDaysDataToMove { get; set; }
        public int Threshold { get; set; }
        public int TimeoutForOperationInMinutes { get; set; }

    }
}
