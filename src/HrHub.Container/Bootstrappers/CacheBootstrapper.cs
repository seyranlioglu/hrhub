using HrHub.Cache.IoC;
using HrHub.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Container.Bootstrappers
{
    public static class CacheBootstrapper
    {
        public static void RegisterCache(this IServiceCollection services)
        {
            services.RegisterMemoryCache(configuration =>
            {
                configuration.ExpirationScanFrequency = TimeSpan.FromHours(24);
                configuration.SizeLimit = 1000;
            });
        }
    }
}
