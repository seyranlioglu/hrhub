using HrHub.Abstraction.Data.Context;
using HrHub.Core.Data.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace HrHub.Core.IoC
{
    public static class UnitOfWorkContainer
    {
        /// <summary>
        /// Modullerde Unitofwork oluştururken bunu kullanacağız.
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        public static void RegisterUnitOfWork<TContext>(this IServiceCollection services)
            where TContext : DbContextBase
        {
            //services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUnitOfWork<TContext>, UnitOfWork<TContext>>();
        }
    }
}
