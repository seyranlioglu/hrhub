using HrHub.Abstraction.Settings;
using HrHub.Core.Helpers;
using HrHub.Core.Utilties.Encryption;
using HrHub.Identity.IoC;
using HrHub.Identity.Options;
using HrHub.Identity.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Container.Bootstrappers
{
    public static class IdentityBootstrapper
    {
        public static void RegisterIdentity(this IServiceCollection services)
        {
            var dbSettings = AppSettingsHelper.GetData<IdentityDbSettings>();
            var tokenOptions = AppSettingsHelper.GetData<AsisTokenOptions>();
            var key = ResourceHelper.GetString("TripleDesKey");
            var decryptStr = TripleDesEncryption.Decrypt(dbSettings.ConnectionString, key);

            services.RegisterIdentityDll(config =>
            {
                config.DatabaseType = dbSettings.DatabaseType;
                config.ConnectionString = decryptStr;

                config.TokenOptions = new AsisTokenOptions
                {
                    AccessTokenExpiration = tokenOptions.AccessTokenExpiration,
                    Audience = tokenOptions.Audience,
                    Issuer = tokenOptions.Issuer,
                    RefreshTokenExpiration = tokenOptions.RefreshTokenExpiration,
                    SecurityKey = tokenOptions.SecurityKey
                };
            });


            services.AddScoped<IAppUserService, AppUserService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IAppRoleService, AppRoleService>();
        }
    }
}
