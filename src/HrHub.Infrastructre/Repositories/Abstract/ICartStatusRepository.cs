using HrHub.Abstraction.Data.EfCore.Repository;
using HrHub.Core.Data.Repository; // IRepository<T> buradan geliyor
using HrHub.Domain.Entities.SqlDbEntities;
using System.Threading.Tasks;

namespace HrHub.Application.Repositories.CartStatusRepositories
{
    public interface ICartStatusRepository : IRepository<CartStatus>
    {
        Task<long> GetIdByCodeAsync(string code);
    }
}