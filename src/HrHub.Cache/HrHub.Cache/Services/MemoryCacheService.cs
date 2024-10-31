using HrHub.Cache.Extensions;
using HrHub.Cache.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Cache.Services
{
    internal class MemoryCacheService : CacheServiceBase, IMemoryCacheService, IMemoryCache
    {
        private readonly IMemoryCache memoryCache;

        public MemoryCacheService(IOptions<MemoryCacheOptions> options)
        {
            this.memoryCache = new MemoryCache(options);
        }

        public override Task<T> GetAsync<T>(string key, CancellationToken token = default)
        {
            return Task.Run(
                () =>
                {
                    var cacheItem = this.memoryCache.Get<CacheItem<T>>(key);
                    if (cacheItem == null)
                    {
                        throw new Exception(key);
                    }

                    return cacheItem.Value;
                }, token);
        }

        public override Task<(bool keyExists, T cacheItem)> TryGetAsync<T>(
            string key,
            CancellationToken token = default)
        {
            return Task.Run(
                () =>
                {
                    var cacheItem = this.memoryCache.Get<CacheItem<T>>(key);
                    return (cacheItem != null, cacheItem?.Value);
                }, token);
        }

        public override (bool keyExists, string cacheItem) TryGetString(string key)
        {
            var cacheItem = this.memoryCache.Get<string?>(key);
            var value = string.IsNullOrEmpty(cacheItem) ? string.Empty : cacheItem!;
            return (!string.IsNullOrEmpty(cacheItem), value);
        }

        public override async Task<(bool keyExists, string cacheItem)> TryGetStringAsync(string key, CancellationToken token = default)
        {
            return await Task.Run(
                () =>
                {
                    var cacheItem = this.memoryCache.Get<string?>(key);
                    string value = string.IsNullOrEmpty(cacheItem) ? string.Empty : cacheItem!;
                    return (!string.IsNullOrEmpty(cacheItem), value);
                }, token);
        }

        public override Task SetAsync<T>(T cacheItem, string key, CancellationToken token = default)
        {
            return Task.Run(() => { this.memoryCache.Set(key, new CacheItem<T>(cacheItem)); }, token);
        }

        public override Task SetAsync<T>(T cacheItem, string key, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            return Task.Run(() => { this.memoryCache.Set(key, new CacheItem<T>(cacheItem), options.ToMemoryCache()); }, token);
        }

        public override void SetString(string cacheItem, string key)
        {
            this.memoryCache.Set(key, cacheItem);
        }

        public override void SetString(string cacheItem, string key, DistributedCacheEntryOptions options)
        {
            this.memoryCache.Set(key, cacheItem, options.ToMemoryCache());
        }

        public override Task SetStringAsync(string cacheItem, string key, CancellationToken token = default)
        {
            return Task.Run(() => { this.memoryCache.Set(key, cacheItem); }, token);
        }

        public override Task SetStringAsync(string cacheItem, string key, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            return Task.Run(() => { this.memoryCache.Set(key, cacheItem, options.ToMemoryCache()); }, token);
        }

        public override Task<bool> ExistsAsync<T>(string key, CancellationToken token = default)
        {
            return Task.Run(
                () =>
                {
                    var cacheItem = this.memoryCache.Get<CacheItem<T>>(key);
                    return cacheItem != null;
                }, token);
        }

        public override Task RemoveAsync<T>(string key, CancellationToken token = default)
        {
            return Task.Run(() => { this.memoryCache.Remove(key); }, token);
        }

        #region IMemoryCache implementation

        public void Dispose()
        {
            this.memoryCache.Dispose();
        }

        public bool TryGetValue(object key, out object value)
        {
            return this.memoryCache.TryGetValue(key, out value);
        }

        public ICacheEntry CreateEntry(object key)
        {
            return this.memoryCache.CreateEntry(key);
        }

        public void Remove(object key)
        {
            this.memoryCache.Remove(key);
        }
        /// <summary>
        /// Generic Tip olarak List kullanılmamalıdır.
        /// Cache list göndermiş bile olsanız sadece base modeli gönderirseniz predicate query içinde onu kullanabilirsiniz. 
        /// Method cache içindeki itemın List olup olmadığını anlayacak ve ona göre bir sorgulama yapacak sonuç dönecektir.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="predicate"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> QueryAsync<T>(string key, Func<T, bool> predicate, CancellationToken token = default)
            where T : class, new()
        {
            if (!memoryCache.TryGetValue(key, out CacheItem<T> cacheItem))
            {
                // Veri yoksa veya boşsa, boş bir koleksiyon döndür
                return Enumerable.Empty<T>();
            }

            var data = cacheItem.Value;

            // Eğer data bir IEnumerable ise
            if (data is IEnumerable<T> enumerableData)
            {
                // Her bir eleman üzerinde predicate'ı uygula
                var filteredItems = enumerableData.Where(predicate).ToList();
                return filteredItems;
            }

            // Data tek bir nesne ise, direkt olarak predicate'ı uygula
            if (predicate(data))
            {
                return new List<T> { data };
            }
            else
            {
                return Enumerable.Empty<T>();
            }
        }

        private Func<object, bool> ItemPredicate<T>(Func<T, bool> predicate)
        {
            return obj => predicate((T)obj);
        }

        //public Task<IEnumerable<T>> QueryAsync<T>(string key, Func<T, bool> predicate, CancellationToken token = default) where T : class, new()
        //{
        //    return Task.Run(
        //    () =>
        //    {
        //        var cacheItems = this.memoryCache.Get<IEnumerable<CacheItem<T>>>(key);

        //        if (cacheItems == null)
        //        {
        //            // Veri yoksa veya boşsa, boş bir koleksiyon döndür
        //            return Enumerable.Empty<T>();
        //        }

        //        // Belirli bir koşulu sağlayan öğeleri seç
        //        var filteredItems = cacheItems.Select(cacheItem => cacheItem.Value).Where(predicate).ToList();

        //        return filteredItems;
        //    }, token);
        //}

        #endregion  IMemoryCache implementation
    }
}
