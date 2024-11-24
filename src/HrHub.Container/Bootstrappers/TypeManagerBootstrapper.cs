using HrHub.Application.Managers.ExamOperationManagers;
using HrHub.Application.Managers.TypeManagers;
using HrHub.Domain.Entities.SqlDbEntities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
