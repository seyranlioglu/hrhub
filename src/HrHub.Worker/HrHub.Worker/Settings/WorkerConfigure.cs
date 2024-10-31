using HrHub.Abstraction.Attributes;
using HrHub.Abstraction.Settings;
using HrHub.Worker.Abstractions.Enums;

namespace HrHub.Worker.Settings
{
    [AppSetting("WorkerSettings:WorkerConfigure")]
    public class WorkerConfigure : ISettingsBase
    {
        public DatabaseType DatabaseType { get; set; }
        public string ServerName { get; set; }
        public string ConnectionString { get; set; }
        public string SchemaName { get; set; }
        public TimeSpan QueuePollInterval { get; set; } = TimeSpan.FromSeconds(2);
        public TimeSpan CommandBatchMaxTimeout { get; set; } = TimeSpan.FromMinutes(5);
        public TimeSpan SlidingInvisibilityTimeout { get; set; } = TimeSpan.FromMinutes(5);
        public bool UseRecommendedIsolationLevel { get; set; } = true;
        public bool DisableGlobalLocks { get; set; } = true;
        public TimeSpan WithJobExpirationTimeout { get; set; } = TimeSpan.FromMinutes(60);
        public TimeSpan TransactionTimeout { get; set; } = TimeSpan.FromMinutes(5);
        public TimeSpan CountersAggregateInterval { get; set; } = TimeSpan.FromMinutes(5);
        public TimeSpan JobExpirationCheckInterval { get; set; } = TimeSpan.FromMinutes(5);
    }
}
