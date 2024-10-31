using Microsoft.Extensions.Caching.Distributed;

namespace HrHub.Cache.Services
{
    internal class CacheService : CacheServiceBase, ICacheService
    {
        private readonly IMemoryCacheService memoryCache;

        private readonly IDistributedCacheService distributedCache;

        public CacheService(IMemoryCacheService memoryCache, IDistributedCacheService distributedCache)
        {
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
        }

        public override async Task<T> GetAsync<T>(string key, CancellationToken token = default)
        {
            try
            {
                var (keyExist, cacheItem) = await this.memoryCache.TryGetAsync<T>(key, token).ConfigureAwait(false);
                if (keyExist)
                {
                    return cacheItem;
                }

                cacheItem = await this.distributedCache.GetAsync<T>(key, token).ConfigureAwait(false);
                await this.memoryCache.SetAsync(cacheItem, key, token).ConfigureAwait(false);

                return cacheItem;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public override async Task<(bool keyExists, T cacheItem)> TryGetAsync<T>(string key, CancellationToken token = default)
        {
            try
            {
                var result = await this.memoryCache.TryGetAsync<T>(key, token).ConfigureAwait(false);
                if (result.keyExists)
                {
                    return result;
                }

                result = await this.distributedCache.TryGetAsync<T>(key, token).ConfigureAwait(false);
                if (result.keyExists)
                {
                    await this.memoryCache.SetAsync(result.cacheItem, key, token).ConfigureAwait(false);
                }

                return result;
            }
            catch (Exception)
            {
                return (false, null);
            }
        }

        public override (bool keyExists, string cacheItem) TryGetString(string key)
        {
            try
            {
                var result = this.memoryCache.TryGetString(key);
                if (result.keyExists)
                {
                    return result;
                }

                result = this.distributedCache.TryGetString(key);
                if (result.keyExists)
                {
                    this.memoryCache.SetString(result.cacheItem, key);
                }

                return result;
            }
            catch (Exception)
            {
                return (false, string.Empty);
            }
        }

        public override async Task<(bool keyExists, string cacheItem)> TryGetStringAsync(string key, CancellationToken token = default)
        {
            try
            {
                var result = await this.memoryCache.TryGetStringAsync(key, token).ConfigureAwait(false);
                if (result.keyExists)
                {
                    return result;
                }

                result = await this.distributedCache.TryGetStringAsync(key, token).ConfigureAwait(false);
                if (result.keyExists)
                {
                    this.memoryCache.SetString(result.cacheItem, key);
                }

                return result;
            }
            catch (Exception)
            {
                return (false, string.Empty);
            }
        }

        public override async Task SetAsync<T>(T cacheItem, string key, CancellationToken token = default)
        {
            try
            {
                await this.memoryCache.SetAsync(cacheItem, key, token).ConfigureAwait(false);
                await this.distributedCache.SetAsync(cacheItem, key, token).ConfigureAwait(false);
            }
            catch (Exception)
            {

            }
        }

        public override async Task SetAsync<T>(T cacheItem, string key, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            try
            {
                await this.memoryCache.SetAsync(cacheItem, key, options, token).ConfigureAwait(false);
                await this.distributedCache.SetAsync(cacheItem, key, options, token).ConfigureAwait(false);
            }
            catch (Exception)
            {

            }
        }

        public override void SetString(string cacheItem, string key)
        {
            try
            {
                this.memoryCache.SetString(cacheItem, key);
                this.distributedCache.SetString(cacheItem, key);
            }
            catch (Exception)
            {

            }
        }

        public override void SetString(string cacheItem, string key, DistributedCacheEntryOptions options)
        {
            try
            {
                this.memoryCache.SetString(cacheItem, key, options);
                this.distributedCache.SetString(cacheItem, key, options);
            }
            catch (Exception)
            {

            }
        }

        public override async Task SetStringAsync(string cacheItem, string key, CancellationToken token = default)
        {
            try
            {
                await this.memoryCache.SetStringAsync(cacheItem, key, token).ConfigureAwait(false);
                await this.distributedCache.SetStringAsync(cacheItem, key, token).ConfigureAwait(false);
            }
            catch (Exception)
            {

            }
        }

        public override async Task SetStringAsync(string cacheItem, string key, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            try
            {
                await this.memoryCache.SetStringAsync(cacheItem, key, options, token).ConfigureAwait(false);
                await this.distributedCache.SetStringAsync(cacheItem, key, options, token).ConfigureAwait(false);
            }
            catch (Exception)
            {

            }
        }

        public override async Task<bool> ExistsAsync<T>(string key, CancellationToken token = default)
        {
            try
            {
                return await this.memoryCache.ExistsAsync<T>(key, token).ConfigureAwait(false) || await this.distributedCache.ExistsAsync<T>(key, token).ConfigureAwait(false);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override async Task RemoveAsync<T>(string key, CancellationToken token = default)
        {
            try
            {
                await this.memoryCache.RemoveAsync<T>(key, token).ConfigureAwait(false);
                await this.distributedCache.RemoveAsync<T>(key, token).ConfigureAwait(false);
            }
            catch (Exception)
            {
            }

        }
    }
}
