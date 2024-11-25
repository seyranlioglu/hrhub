using HrHub.Application.Managers.TypeManagers;
using Microsoft.Extensions.DependencyInjection;

namespace HrHub.Container.Bootstrappers
{
    public static class TypeManagerBootstrapper
    {
        public static void RegisterTypeManagers(this IServiceCollection services)
        {
            services.AddScoped(typeof(ICommonTypeBaseManager<>), typeof(CommonTypeBaseManager<>));

        }
    }
}
