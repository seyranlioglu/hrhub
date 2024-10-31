using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Worker.Settings
{
    public class RecurringJobItem<T>
    {
        public string JobId { get; set; }
        public string CronExpression { get; set; }
        public TimeZoneInfo TimeZone { get; set; }
        public Expression<Func<T, Task>> methodCall { get; set; }
    }
}
