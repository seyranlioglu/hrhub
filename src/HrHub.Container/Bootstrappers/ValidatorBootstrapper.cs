using HrHub.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Container.Bootstrappers
{
    public static class ValidatorBootstrapper
    {
        public static void RegisterValidator(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var iserviceProvider = serviceProvider.GetRequiredService<IServiceProvider>();
            ValidationHelper.ValidatorHelperConfigure(iserviceProvider);
        }
    }
}
