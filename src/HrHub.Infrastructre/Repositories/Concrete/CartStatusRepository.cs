using HrHub.Abstraction.Data; // IUnitOfWork vb.
using HrHub.Abstraction.Data.Context;
using HrHub.Application.Repositories.CartStatusRepositories;
using HrHub.Core.Data.Repository;
using HrHub.Core.Data.UnitOfWork;
using HrHub.Domain.Contexts;
using HrHub.Domain.Entities.SqlDbEntities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HrHub.Application.Repositories.CartStatusRepositories
{
    public class CartStatusRepository : EntityRepository<CartStatus>, ICartStatusRepository
    {
        public CartStatusRepository(HrHubDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<long> GetIdByCodeAsync(string code)
        {
            // AsNoTracking performans içindir, sadece okuma yapıyoruz.
            var status = await GetAsync(x => x.Code == code && x.IsActive == true && x.IsDelete != true);

            return status != null ? status.Id : 0;
        }
    }
}