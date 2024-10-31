using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Cache.Services
{
    internal abstract class CacheServiceBase : ICacheService
    {
        public Task<T> GetAsync<T>(CancellationToken token = default)
            where T : class, new()
        {
            return this.GetAsync<T>(typeof(T).FullName, token);
        }

        public abstract Task<T> GetAsync<T>(string key, CancellationToken token = default)
            where T : class, new();

        public Task<(bool keyExists, T cacheItem)> TryGetAsync<T>(
            CancellationToken token = default)
            where T : class, new()
        {
            return this.TryGetAsync<T>(typeof(T).FullName, token);
        }

        public abstract Task<(bool keyExists, T cacheItem)> TryGetAsync<T>(
            string key,
            CancellationToken token = default)
            where T : class, new();

        public abstract (bool keyExists, string cacheItem) TryGetString(string key);

        public abstract Task<(bool keyExists, string cacheItem)> TryGetStringAsync(string key, CancellationToken token = default);

        public Task SetAsync<T>(T cacheItem, CancellationToken token = default)
            where T : class, new()
        {
            return this.SetAsync<T>(cacheItem, typeof(T).FullName, token);
        }

        public abstract Task SetAsync<T>(
            T cacheItem,
            string key,
            CancellationToken token = default)
            where T : class, new();

        public abstract Task SetAsync<T>(T cacheItem, string key, DistributedCacheEntryOptions options, CancellationToken token = default)
            where T : class, new();

        public abstract void SetString(string cacheItem, string key);

        public abstract void SetString(string cacheItem, string key, DistributedCacheEntryOptions options);

        public abstract Task SetStringAsync(string cacheItem, string key, CancellationToken token = default);

        public abstract Task SetStringAsync(string cacheItem, string key, DistributedCacheEntryOptions options, CancellationToken token = default);

        public Task<bool> ExistsAsync<T>(CancellationToken token = default)
            where T : class, new()
        {
            return this.ExistsAsync<T>(typeof(T).FullName, token);
        }

        public abstract Task<bool> ExistsAsync<T>(string key, CancellationToken token = default)
            where T : class, new();

        public Task RemoveAsync<T>(CancellationToken token = default)
            where T : class, new()
        {
            return this.RemoveAsync<T>(typeof(T).FullName, token);
        }

        public abstract Task RemoveAsync<T>(string key, CancellationToken token = default)
            where T : class, new();


        //public Task<IEnumerable<T>> QueryAsync<T>(
        //Func<T, bool> predicate,
        //CancellationToken token = default)
        //where T : class, new()
        //{
        //    return this.QueryAsync<T>(typeof(T).FullName, predicate, token);
        //}

        //public abstract Task<IEnumerable<T>> QueryAsync<T>(
        //string key,
        //Func<T, bool> predicate,
        //CancellationToken token = default)
        //where T : class, new();
    }
}
