using HrHub.Application.Mappers;
using HrHub.Core.IoC;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Container.Bootstrappers
{
    public static class MapperBootstrapper
    {
        public static void RegisterMapper(this IServiceCollection services)
        {
            services.RegisterMapper<MapperProfile>();
        }
    }
}
