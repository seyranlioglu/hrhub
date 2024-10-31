using HrHub.Abstraction.Enums;

namespace HrHub.Core.Configurations
{
    public class ContextConfiguration
    {
        public DatabaseType DatabaseType { get; set; }
        public string ConnectionString { get; set; }
    }
}
