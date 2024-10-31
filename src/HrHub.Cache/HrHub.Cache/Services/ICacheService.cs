using Microsoft.Extensions.Caching.Distributed;

namespace HrHub.Cache.Services
{
    public interface ICacheService
    {
        Task<T> GetAsync<T>(CancellationToken token = default)
             where T : class, new();

        Task<T> GetAsync<T>(string key, CancellationToken token = default)
            where T : class, new();

        Task<(bool keyExists, T cacheItem)> TryGetAsync<T>(CancellationToken token = default)
            where T : class, new();

        Task<(bool keyExists, T cacheItem)> TryGetAsync<T>(string key, CancellationToken token = default)
            where T : class, new();

        (bool keyExists, string cacheItem) TryGetString(string key);

        Task<(bool keyExists, string cacheItem)> TryGetStringAsync(string key, CancellationToken token = default);

        Task SetAsync<T>(T cacheItem, CancellationToken token = default)
            where T : class, new();

        Task SetAsync<T>(T cacheItem, string key, CancellationToken token = default)
            where T : class, new();

        Task SetAsync<T>(T cacheItem, string key, DistributedCacheEntryOptions options,
            CancellationToken token = default)
            where T : class, new();

        void SetString(string cacheItem, string key);

        void SetString(string cacheItem, string key, DistributedCacheEntryOptions options);

        Task SetStringAsync(string cacheItem, string key, CancellationToken token = default);

        Task SetStringAsync(string cacheItem, string key, DistributedCacheEntryOptions options, CancellationToken token = default);

        Task<bool> ExistsAsync<T>(CancellationToken token = default)
            where T : class, new();

        Task<bool> ExistsAsync<T>(string key, CancellationToken token = default)
            where T : class, new();

        Task RemoveAsync<T>(CancellationToken token = default)
            where T : class, new();

        Task RemoveAsync<T>(string key, CancellationToken token = default)
            where T : class, new();

    }
}
