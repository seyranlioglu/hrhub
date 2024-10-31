using HrHub.Abstraction.Exceptions;
using HrHub.Abstraction.Settings;
using HrHub.Abstraction.StatusCodes;
using HrHub.Core.Configurations;
using HrHub.Core.Extensions;
using HrHub.Core.Helpers;
using HrHub.Core.Utilties.Encryption;
using HrHub.Domain.Contexts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Container.Bootstrappers
{
    public static class DbBootstrappers
    {
        public static void AddDbContext(this IServiceCollection services)
        {
            DatabaseSettings dbSettings = AppSettingsHelper.GetData<DatabaseSettings>();
            string key = ResourceHelper.GetString("TripleDesKey");
            if (string.IsNullOrEmpty(key))
                throw new BusinessException(HrStatusCodes.Status111DataNotFound, "Triple Des Key Not Found!");

            string decryptStr = TripleDesEncryption.Decrypt(dbSettings.ConnectionString, key);
            services.AddBackendDataEF<HrHubDbContext>(new ContextConfiguration
            {
                ConnectionString = decryptStr,
                DatabaseType = dbSettings.DatabaseType
            });
        }

        public static void AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            var mongodbConfig = configuration.GetSection("Databases:MongoDbSettings").Get<MongoDbSettings>(); ;
            services.Configure<MongoDbSettings>(config =>
            {
                config = mongodbConfig;
            });

            services.AddSingleton<IMongoDbSettings>(serviceProvider =>
                serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value);
        }
    }
}
