using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Cache.Services
{
    public interface IMemoryCacheService : ICacheService
    {
        Task<IEnumerable<T>> QueryAsync<T>(string key, Func<T, bool> predicate, CancellationToken token = default) where T : class, new();
    }
}
