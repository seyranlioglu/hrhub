using HrHub.Abstraction.Attributes;
using HrHub.Abstraction.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Worker.Settings
{
    [AppSetting("WorkerSettings:DashboardSettings")]
    public class HangfireDashboardConfigure : ISettingsBase
    {
        public string HangfirePath { get; set; } = "/hangfire";
        public string DashboardTitle { get; set; } = "HrHub";
        public string AppPath { get; set; } = "http://onayyazilim.com.tr";
        public bool UseAuthentication { get; set; } = false;
        public List<WorkerUser> Users { get; set; }
        public TimeSpan SchedulePollingInterval { get; set; } = TimeSpan.FromMinutes(1);
        /// <summary>
        /// Enviroment.ProcessorCount * WorkerCount olacak.
        /// </summary>
        public int WorkerCount { get; set; } = 5;
        public int AutomaticRetryAttemps { get; set; } = 3;
    }
}
