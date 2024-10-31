using HrHub.Core.Helpers;
using HrHub.Core.Utilties.Encryption;
using HrHub.Worker.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using HrHub.Worker.IoC;
using HrHub.Application.Workers;

namespace ConnectionProvider.Container.Bootstrappers
{
    public static class WorkerBootstrapper
    {
        public static void RegisterHrHubWorker(this IServiceCollection services)
        {
            var workerConfigure = AppSettingsHelper.GetData<WorkerConfigure>();
            var key = ResourceHelper.GetString("TripleDesKey");
            var decryptStr = TripleDesEncryption.Decrypt(workerConfigure.ConnectionString, key);
            services.RegisterWorker(configure: config =>
            {
                config.DatabaseType = workerConfigure.DatabaseType;
                config.ConnectionString = decryptStr;
                config.SchemaName = workerConfigure.SchemaName;
                config.QueuePollInterval = workerConfigure.QueuePollInterval;
                config.CommandBatchMaxTimeout = workerConfigure.CommandBatchMaxTimeout;
                config.SlidingInvisibilityTimeout = workerConfigure.SlidingInvisibilityTimeout;
                config.UseRecommendedIsolationLevel = workerConfigure.UseRecommendedIsolationLevel;
                config.DisableGlobalLocks = workerConfigure.DisableGlobalLocks;
                config.WithJobExpirationTimeout = workerConfigure.WithJobExpirationTimeout;
                config.TransactionTimeout = workerConfigure.TransactionTimeout;
                config.CountersAggregateInterval = workerConfigure.CountersAggregateInterval;
                config.JobExpirationCheckInterval = workerConfigure.JobExpirationCheckInterval;
                config.ServerName = workerConfigure.ServerName;
            });
        }

        public static IApplicationBuilder AddWorkerDashboard(this IApplicationBuilder app)
        {
            var settings = AppSettingsHelper.GetData<HangfireDashboardConfigure>();
            app.UseHangfireDashboard(configure: config =>
            {
                config.AppPath = settings.AppPath;
                config.HangfirePath = settings.HangfirePath;
                config.SchedulePollingInterval = settings.SchedulePollingInterval;
                config.Users = settings.Users;
                config.AutomaticRetryAttemps = settings.AutomaticRetryAttemps;
                config.DashboardTitle = settings.DashboardTitle;
                config.UseAuthentication = settings.UseAuthentication;
                config.WorkerCount = settings.WorkerCount;
            });

            var serviceProvider = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            RecurringJobs recurringJobs = new RecurringJobs(serviceProvider);
            recurringJobs.AddRecurringJobs();
            return app;
        }
    }
}
