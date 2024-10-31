using HrHub.Worker.Settings;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Worker.Helpers
{
    public static class WorkerHelper
    {
        [AutomaticRetry(Attempts = 3)]
        public static void AddRecurringJob<T>(RecurringJobItem<T> recurringJob, bool enable = true)
        {
            RecurringJob.RemoveIfExists(recurringJob.JobId);

            if (enable)
            {
                RecurringJob.AddOrUpdate
                (recurringJobId: recurringJob.JobId,
                methodCall: recurringJob.methodCall,
                cronExpression: recurringJob.CronExpression,
                new RecurringJobOptions
                {
                    TimeZone = recurringJob.TimeZone
                });
            }
        }
    }
}
