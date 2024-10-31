using AutoMapper;
using HrHub.Core.Mapper;
using Microsoft.Extensions.DependencyInjection;

namespace HrHub.Core.IoC
{
    public static class MapperContainer
    {
        public static void RegisterMapper<TProfile>(this IServiceCollection services)
            where TProfile : MapperProfileBase, new()
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new TProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
