using HrHub.Abstraction.Domain;
using HrHub.Core.Base;
using System.Linq.Expressions;

namespace HrHub.Application.Managers.TypeManagers
{
    public interface ICommonTypeBaseManager<TTypeEntity> : IBaseManager where TTypeEntity : class, IBaseTypeEntity, new()
    {
        Task<TResponse> AddAsync<TSource, TResponse>(TSource data) where TSource : class;
        Task DeleteAsync(long id);
        Task<TResult> GetByCodeAsync<TResult>(string code) where TResult : class;
        Task<TResult> GetByIdAsync<TResult>(long id) where TResult : class;
        Task<TResult> GetByTitleAsync<TResult>(string title) where TResult : class;
        Task<long> GetIdByCodeAsync(string code);
        Task<TResponse> Get<TResponse>(Expression<Func<TTypeEntity, bool>> predicate = null) where TResponse : class;
        Task<IEnumerable<TResponse>> GetList<TResponse>(Expression<Func<TTypeEntity, bool>> predicate = null) where TResponse : class;
        Task UpdateAsync<TData>(long id, TData data);
    }
}