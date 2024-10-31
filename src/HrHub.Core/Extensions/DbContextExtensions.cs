using HrHub.Abstraction.Data.Context;
using HrHub.Abstraction.Enums;
using HrHub.Core.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HrHub.Core.Extensions
{
    public static class DbContextExtensions
    {
        public static void AddBackendDataEF<TContext>(this IServiceCollection services, ContextConfiguration config) where TContext : DbContextBase
        {
            services.AddDbContext<TContext>(optionsBuilder =>
            {
                var databaseType = config.DatabaseType;
                switch (databaseType)
                {
                    case DatabaseType.Postgre:
                        optionsBuilder.UseNpgsql(config.ConnectionString);
                        break;
                    default:
                        throw new NotSupportedException($"Name {config.DatabaseType.ToString()}:is not supported");
                }
            }).AddScoped<DbContextBase, TContext>();
        }
    }
}
