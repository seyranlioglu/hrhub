using HrHub.Worker.Abstractions.Enums;
using HrHub.Worker.Settings;
using Hangfire;
using Hangfire.PostgreSql;
using HangfireBasicAuthenticationFilter;
using LinqKit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace HrHub.Worker.IoC
{
    public static class WorkerContainer
    {
        public static void RegisterWorker(this IServiceCollection services, Action<WorkerConfigure> configure = default)
        {

            var hangfireSettings = new WorkerConfigure();
            configure(hangfireSettings); 

            services.AddHangfire(config => 
            {
                config.UseFilter(new AutomaticRetryAttribute { Attempts = 3 });
                switch (hangfireSettings.DatabaseType)
                {
                    case DatabaseType.Postgre:
                        var postgreOptions = new PostgreSqlStorageOptions
                        {
                            PrepareSchemaIfNecessary = true,
                            QueuePollInterval = hangfireSettings.QueuePollInterval,
                            //CommandBatchMaxTimeout = hangfireSettings.CommandBatchMaxTimeout,
                            //SlidingInvisibilityTimeout = hangfireSettings.SlidingInvisibilityTimeout,
                            //UseRecommendedIsolationLevel = hangfireSettings.UseRecommendedIsolationLevel,
                            //DisableGlobalLocks = hangfireSettings.DisableGlobalLocks,
                            //TransactionTimeout = hangfireSettings.TransactionTimeout,
                            CountersAggregateInterval = hangfireSettings.CountersAggregateInterval,
                            JobExpirationCheckInterval = hangfireSettings.JobExpirationCheckInterval,
                        };
                        config.UsePostgreSqlStorage(hangfireSettings.ConnectionString, postgreOptions);
                        break;
                    default: throw new NotSupportedException($"Name {hangfireSettings.DatabaseType}:is not supported");

                };
                //IMonitoringApi monitoringApi = JobStorage.Current.GetMonitoringApi();
                //var serverToRemove = monitoringApi.Servers();
                //foreach (var server in serverToRemove)
                //{
                //    JobStorage.Current.GetConnection().RemoveServer(server.Name);
                //}
                config.UseSerializerSettings(new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            }).AddHangfireServer(options =>
            {
                options.SchedulePollingInterval = TimeSpan.FromMinutes(1);
                options.WorkerCount = Environment.ProcessorCount * 5;
                options.ServerName = hangfireSettings.ServerName;

                var storage = JobStorage.Current;
                if (storage == null)
                {
                    throw new InvalidOperationException("JobStorage.Current is null. Hangfire configuration may not be completed.");
                }

                var servers = storage.GetMonitoringApi().Servers();
                servers.Where(w => w.Name != hangfireSettings.ServerName).ForEach(r => storage.GetConnection().RemoveServer(r.Name));

                if (!servers.Any(w => w.Name == hangfireSettings.ServerName))
                {
                    JobStorage.Current.GetConnection().RemoveTimedOutServers(new TimeSpan(0, 5, 0)); // Remove timed out servers
                                                                                                     //JobStorage.Current.GetConnection().Server
                }
            });
        }

        public static IApplicationBuilder UseHangfireDashboard(this IApplicationBuilder app, Action<HangfireDashboardConfigure> configure)
        {
            HangfireDashboardConfigure hangfireSettings = new HangfireDashboardConfigure();
            configure(hangfireSettings);

            app.UseHangfireDashboard(hangfireSettings.HangfirePath, new DashboardOptions
            {
                DashboardTitle = hangfireSettings.DashboardTitle,
                AppPath = hangfireSettings.AppPath,
                Authorization = hangfireSettings.Users.Select(s => new HangfireCustomBasicAuthenticationFilter
                {
                    User = s.UserName,
                    Pass = s.Password
                }).ToArray(),
            });
            return app;
        }
    }
}
