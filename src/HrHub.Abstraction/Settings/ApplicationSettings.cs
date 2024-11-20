using HrHub.Abstraction.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Abstraction.Settings
{
    [AppSetting("ApplicationSettings")]
    public class ApplicationSettings : ISettingsBase
    {
        /// <summary>
        /// Day
        /// </summary>
        public int ExamValidityTime { get; set; }
    }
}
