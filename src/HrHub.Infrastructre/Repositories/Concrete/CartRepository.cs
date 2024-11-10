using HrHub.Abstraction.Data.Context;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.Repositories.Abstract;

namespace HrHub.Infrastructre.Repositories.Concrete
{
    public class CartRepository : EntityRepository<Cart>, ICartRepository
    {
        public CartRepository(DbContextBase dbContext) : base(dbContext)
        {
        }
    }
}
