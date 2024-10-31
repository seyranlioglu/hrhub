using HrHub.Cache.Helpers;
using HrHub.Cache.Models;
using HrHub.Cache.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack.Redis;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ServiceStack.Diagnostics.Events;

namespace HrHub.Cache.IoC
{
    public static class CacheContainer
    {
        public static void RegisterMemoryCache(this IServiceCollection services, Action<MemoryCacheOptions> configuration)
        {
            MemoryCacheOptions memCacheptions = new MemoryCacheOptions();
            configuration(memCacheptions);
            services.Configure<MemoryCacheOptions>(options =>
            {
                options = memCacheptions;
            });
            services.AddSingleton<IMemoryCacheService, MemoryCacheService>();
            services.AddSingleton<IMemoryCache, MemoryCacheService>();
            services.AddSingleton<ICacheService, CacheService>();
        }

        public static void RegisterRedisCache(this IServiceCollection services, Action<DistrubutedCacheSettings> configuration = default)
        {
            DistrubutedCacheSettings distrubutedCacheSettings = new DistrubutedCacheSettings();
            configuration?.Invoke(distrubutedCacheSettings);

            string redisHost;
            if (!string.IsNullOrEmpty(distrubutedCacheSettings.Username) && !string.IsNullOrEmpty(distrubutedCacheSettings.Password))
                redisHost = $"redis://{distrubutedCacheSettings.Username}:{distrubutedCacheSettings.Password}@{distrubutedCacheSettings.Host}:{distrubutedCacheSettings.Port}";
            else if (!string.IsNullOrEmpty(distrubutedCacheSettings.Password))
                redisHost = $"redis://:{distrubutedCacheSettings.Password}@{distrubutedCacheSettings.Host}:{distrubutedCacheSettings.Port}";
            else
                redisHost = CacheHelper.GetHost(distrubutedCacheSettings.Host, distrubutedCacheSettings.Port);

            RedisManagerPool redisManagerPool = new RedisManagerPool(redisHost);
            IRedisClient client = redisManagerPool.GetClient();
            if (distrubutedCacheSettings.Timeout.HasValue && distrubutedCacheSettings.Timeout > 0)
            {
                client.ConnectTimeout = distrubutedCacheSettings.Timeout.Value;
            }
            services.AddSingleton((Func<IServiceProvider, IRedisClientsManager>)((IServiceProvider c) => redisManagerPool));
            services.AddSingleton(client);
            services.AddSingleton<IDistributedCacheService, DistributedCacheService>();
            services.AddSingleton<ICacheService, CacheService>();

            #region OldCode

            //DistrubutedCacheSettings distrubutedCacheSettings = new DistrubutedCacheSettings();
            //configuration?.Invoke(distrubutedCacheSettings);
            //RedisManagerPool redisManagerPool = new RedisManagerPool(CacheHelper.GetHost(distrubutedCacheSettings.Host, distrubutedCacheSettings.Port));
            //IRedisClient client = redisManagerPool.GetClient();
            //if (!string.IsNullOrEmpty(distrubutedCacheSettings.Username))
            //{
            //    client.Username = distrubutedCacheSettings.Username;
            //}

            //if (!string.IsNullOrEmpty(distrubutedCacheSettings.Password))
            //{
            //    client.Password = distrubutedCacheSettings.Password;
            //}

            //if (distrubutedCacheSettings.Timeout.HasValue && distrubutedCacheSettings.Timeout > 0)
            //{
            //    client.ConnectTimeout = distrubutedCacheSettings.Timeout.Value;
            //}

            //services.AddSingleton((Func<IServiceProvider, IRedisClientsManager>)((IServiceProvider c) => redisManagerPool));
            //services.AddSingleton(client);
            //services.AddSingleton<IDistributedCacheService, DistributedCacheService>();
            //services.AddSingleton<ICacheService, CacheService>();
            #endregion
        }
    }
}

