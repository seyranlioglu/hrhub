using HrHub.Core.Data.Repository;
using HrHub.Domain.Contexts;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.Repositories.Abstract;

namespace HrHub.Infrastructre.Repositories.Concrete
{
    public class CurrAccRepository : EntityRepository<CurrAcc>, ICurrAccRepository
    {
        public CurrAccRepository(HrHubDbContext dbContext) : base(dbContext)
        {
        }
    }
}
