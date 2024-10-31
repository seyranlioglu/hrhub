using HrHub.Cache.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServiceStack.Redis;
using StackExchange.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Cache.Services
{
    internal class DistributedCacheService : CacheServiceBase, IDistributedCacheService, IDistributedCache
    {
        private readonly IRedisClient client;

        public DistributedCacheService(IRedisClient redisClient)
        {
            this.client = redisClient;
        }

        public override async Task<T> GetAsync<T>(
           string key,
           CancellationToken token = default)
        {
            var data = client.Get<string>(key);
            if (string.IsNullOrEmpty(data))
            {
                throw new Exception(key);
            }

            //return JObject.Parse(data).ToObject<CacheItem<T>>().Value;
            return JsonConvert.DeserializeObject<CacheItem<T>>(data).Value;
        }

        public override async Task<(bool keyExists, T cacheItem)> TryGetAsync<T>(
            string key,
            CancellationToken token = default)
        {
            var data = client.Get<string>(key);
            //return string.IsNullOrEmpty(data)
            //    ? (false, default(T))
            //    : (true, JObject.Parse(data).ToObject<CacheItem<T>>().Value);
            return string.IsNullOrEmpty(data)
                ? (false, default(T))
                : (true, JsonConvert.DeserializeObject<CacheItem<T>>(data).Value);
        }

        public override (bool keyExists, string cacheItem) TryGetString(string key)
        {
            var data = client.Get<string>(key);
            return string.IsNullOrEmpty(data)
                ? (false, string.Empty)
                : (true, data);
        }

        public override async Task<(bool keyExists, string cacheItem)> TryGetStringAsync(string key, CancellationToken token = default)
        {
            return await Task.Run(
                () =>
                {
                    var data = client.Get<string>(key);
                    return string.IsNullOrEmpty(data)
                        ? (false, string.Empty)
                        : (true, data);
                }, token);
        }

        public override Task SetAsync<T>(T cacheItem, string key, CancellationToken token = default)
        {
            var data = JObject.FromObject(new CacheItem<T>(cacheItem)).ToString();
            client.Set(key, data);
            return Task.CompletedTask;
        }

        public override void SetString(string cacheItem, string key)
        {
            client.Set(key, cacheItem);
        }

        public override void SetString(string cacheItem, string key, DistributedCacheEntryOptions options)
        {
            client.Set(key, cacheItem, options.AbsoluteExpirationRelativeToNow.Value);
        }

        public override Task SetStringAsync(string cacheItem, string key, CancellationToken token = default)
        {
            return Task.Run(
                () =>
                {
                    client.Set(key, cacheItem);
                    return Task.CompletedTask;
                }, token);
        }

        public override Task SetStringAsync(string cacheItem, string key, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            return Task.Run(
                () =>
                {
                    client.Set(key, cacheItem, options.AbsoluteExpirationRelativeToNow.Value);
                    return Task.CompletedTask;
                }, token);
        }

        public override async Task<bool> ExistsAsync<T>(string key, CancellationToken token = default)
        {
            var data = client.Get<string>(key);
            return !string.IsNullOrEmpty(data);
        }

        public override Task RemoveAsync<T>(string key, CancellationToken token = default)
        {
            client.Remove(key);
            return Task.CompletedTask;
        }

        public override Task SetAsync<T>(T cacheItem, string key, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            var data = JObject.FromObject(new CacheItem<T>(cacheItem)).ToString();
            client.Set(key, data, options.AbsoluteExpirationRelativeToNow.Value);
            return Task.CompletedTask;
        }

        #region IDistributedCache implementation

        //public byte[] Get(string key)
        //{
        //    using (var client = manager.GetClient())
        //    {
        //        return client.Get(key);
        //    }
        //}

        //public Task<byte[]> GetAsync(string key, CancellationToken token = default)
        //{
        //    using (var client = manager.GetClient())
        //    {
        //        return Task.FromResult(client.Get(key));
        //    }
        //}

        public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            client.Set(key, value, options.AbsoluteExpirationRelativeToNow.Value);
        }

        public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            client.Set(key, value, options.AbsoluteExpirationRelativeToNow.Value);
            return Task.CompletedTask;
        }

        public void Refresh(string key)
        {
            client.ExpireEntryAt(key, DateTime.Now.AddMinutes(20));
        }

        public Task RefreshAsync(string key, CancellationToken token = default)
        {
            client.ExpireEntryAt(key, DateTime.Now.AddMinutes(20));
            return Task.CompletedTask;
        }

        public void Remove(string key)
        {
            client.Remove(key);
        }

        public Task RemoveAsync(string key, CancellationToken token = default)
        {
            client.Remove(key);
            return Task.CompletedTask;
        }

        public byte[]? Get(string key)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]?> GetAsync(string key, CancellationToken token = default)
        {
            throw new NotImplementedException();
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
            var data = client.Get<string>(key);

            if (string.IsNullOrEmpty(data))
            {
                // Veri yoksa veya boşsa, boş bir koleksiyon döndür
                return Enumerable.Empty<T>();
            }

            var jdata = JObject.Parse(data);

            if (jdata is not null)
            {
                var valueObj = jdata["Value"];
                if (valueObj.Type is JTokenType.Array)
                {
                    var cacheItems = jdata["Value"].ToObject<List<CacheItem<T>>>();
                    var filteredItems = cacheItems.Select(cacheItem => cacheItem.Value).Where(predicate).ToList();
                    return filteredItems;
                }
                else
                {
                    var cacheItem = jdata["Value"].ToObject<CacheItem<T>>();
                    List<T> list = new List<T>();
                    list.Add(cacheItem.Value);
                    var filteredItems = list.Where(predicate).ToList();
                    return filteredItems;
                }
            }
            else
            {
                return Enumerable.Empty<T>();
            }
        }

        #endregion IDistributedCache implementation
    }
}

    //   public async Task<IEnumerable<T>> QueryAsync<T>(string key, Func<T, bool> predicate, CancellationToken token = default)
    //where T : class, new()
    //   {
    //       try
    //       {
    //           // Redis Lua skripti
    //           var luaScript = @"
    //               local key = KEYS[1]
    //               local items = redis.call('HGETALL', key)

    //               local filteredItems = {}

    //               for i = 1, #items, 2 do
    //                   local field = items[i]
    //                   local value = items[i + 1]

    //                   -- Uygulanan koşullara göre alan ve değeri predicate fonksiyonu ile seç
    //                   if (function_field_selector(field, value)) then
    //                       table.insert(filteredItems, field)
    //                       table.insert(filteredItems, value)
    //                   end
    //               end

    //               return filteredItems
    //           ";

    //           // Lua skripti için parametreler
    //           var parameters = new
    //           {
    //               key,
    //               fieldSelectorScript = GenerateFieldSelectorScript(predicate, typeof(T))
    //           };

    //           // Lua skripti ile Redis üzerinde filtreleme işlemini gerçekleştir
    //           var luaResult = await client.ScriptEvaluateAsync(
    //               LuaScript.Prepare(luaScript),
    //               new { keys = new RedisKey[] { key }, args = new RedisValue[] { JsonConvert.SerializeObject(parameters) } });

    //           // Lua skripti tarafından döndürülen sonucu işle
    //           var luaResultArray = ((string[])luaResult).ToList();
    //           var filteredItems = new List<T>();

    //           for (int i = 0; i < luaResultArray.Count; i += 2)
    //           {
    //               var cacheItem = new CacheItem<T> { Value = JsonConvert.DeserializeObject<T>(luaResultArray[i + 1]) };
    //               filteredItems.Add(cacheItem.Value);
    //           }

    //           return filteredItems;
    //       }
    //       catch (Exception)
    //       {
    //           return Enumerable.Empty<T>();
    //       }
    //   }

    //   private string GenerateFieldSelectorScript<T>(Func<T, bool> predicate, Type itemType) where T : class, new()
    //   {
    //       // T türündeki öğe için field ve value seçen predicate fonksiyonunu döndür
    //       var fieldSelector = new Func<string, string, bool>((field, value) =>
    //       {
    //           var cacheItem = new CacheItem<T> { Value = JsonConvert.DeserializeObject<T>(value) };
    //           return predicate(cacheItem.Value);
    //       });

    //       // Eğer T bir koleksiyon ise, koleksiyonun elemanlarına uygulanan özel predicate fonksiyonunu döndür
    //       if (typeof(IEnumerable<>).IsAssignableFrom(itemType))
    //       {
    //           var itemTypeArgument = itemType.GetGenericArguments().FirstOrDefault();
    //           if (itemTypeArgument != null)
    //           {
    //               var collectionItemSelectorScript = GenerateFieldSelectorScript(predicate, itemTypeArgument);
    //               var scriptStr = $@"
    //                   return function(field, value)
    //                       local items = redis.call('JSON.ARRAYSIZE', value)
    //                       for i = 1, items do
    //                           local itemValue = redis.call('JSON.GET', value, i)
    //                           if ({collectionItemSelectorScript}('', itemValue)) then
    //                               return true
    //                           end
    //                       end
    //                       return false
    //                   end
    //               ";
    //               return scriptStr;
    //           }
    //       }

    //       // Lua skripti olarak dönüştür
    //       var script = LuaScript.Prepare(fieldSelector.Method);
    //       return script;
    //   }
