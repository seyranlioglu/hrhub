using HrHub.Abstraction.Enums;
using HrHub.Identity.Abstraction;
using HrHub.Identity.Context;
using HrHub.Identity.Entities;
using HrHub.Identity.Helpers;
using HrHub.Identity.Options;
using HrHub.Identity.Security.Token;
using HrHub.Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Identity.IoC
{
    public static class IdentityContainer
    {
        public static void RegisterIdentityDll(this IServiceCollection services, Action<IdentityConfigurations> configuration)
        {
            var option = new IdentityConfigurations();
            configuration(option);

            TokenHelper.TokenHelperConfigure(option.TokenOptions);
            services.AddDbContext<AppIdentityDbContext>(opts =>
            { 

                switch (option.DatabaseType)
                {
                    case DatabaseType.SqlServer:
                        break;
                    case DatabaseType.Oracle:
                        break;
                    case DatabaseType.Postgre:
                        opts.UseNpgsql(option.ConnectionString);
                        break;
                    case DatabaseType.SqlLite:
                        break;
                    default:
                        break;
                }
            });

            services.AddIdentity<AppUser, AppRole>(opts =>
            {
                //https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.useroptions.allowedusernamecharacters?view=aspnetcore-2.2

                opts.User.RequireUniqueEmail = true;
                opts.User.AllowedUserNameCharacters = "abcçdefgğhıijklmnoçpqrsştuüvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@";
               
                opts.Password.RequiredLength = 4;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
                opts.SignIn.RequireConfirmedEmail = true;
                opts.Lockout.AllowedForNewUsers = true;
                opts.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(30);
                opts.Lockout.MaxFailedAccessAttempts = 5;
            }).AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders(); 

            services.AddAuthentication(opts => {
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;


            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, jwtbeareroptions =>
            {
                jwtbeareroptions.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = option.TokenOptions.Issuer,
                    ValidAudience = option.TokenOptions.Audience,
                    IssuerSigningKey = SignHandler.GetSecurityKey(option.TokenOptions.SecurityKey),
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = true,
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireSA", policy =>
                {
                    policy.RequireRole("sa");
                });
                options.AddPolicy("DefaultUser", policy =>
                {
                    policy.RequireRole("sa", "admin", "user");
                });
            });

            services.AddScoped<ITokenHandler, TokenHandler>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IAppUserService, AppUserService>();
            services.AddScoped<IAppRoleService, AppRoleService>();
        }
        //public static void RegisterIdentity(this IServiceCollection services, IConfiguration configuration)
        //{
        //    var tokenOptions = configuration.GetSection("TokenOptions").Get<TokenOptions>();
        //    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        //    {
        //        options.RequireHttpsMetadata = false;
        //        options.SaveToken = true;
        //        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        //        {
        //            ValidateIssuer = true,
        //            ValidateAudience = true,
        //            ValidAudience = tokenOptions.Audience,
        //            ValidIssuer = tokenOptions.Issuer,
        //            IssuerSigningKey = SignHandler.GetSecurityKey(tokenOptions.SecurityKey),
        //        };
                
        //    });
        //    services.AddScoped<ITokenHandler, TokenHandler>();
        //}

    }
}
