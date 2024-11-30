using HrHub.Application.Managers.ExamOperationManagers;
using HrHub.Application.Managers.UserManagers;
using Microsoft.Extensions.DependencyInjection;

namespace HrHub.Container.Bootstrappers
{
    public static class ManagerBootstrapper
    {
        public static void RegisterManagers(this IServiceCollection services)
        {
            services.AddScoped<IUserManager, UserManager>();
        }
    }
}