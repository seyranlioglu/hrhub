using System.Collections;

namespace HrHub.Cache.Services
{
    public interface IDistributedCacheService : ICacheService
    {
        Task<IEnumerable<T>> QueryAsync<T>(string key, Func<T, bool> predicate, CancellationToken token = default) where T : class, new();
    }
}
