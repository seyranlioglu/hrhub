using HrHub.Core.Data.Repository;
using HrHub.Domain.Contexts;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.Repositories.Abstract;

namespace HrHub.Infrastructre.Repositories.Concrete
{

    public class PasswordHistoryRepository : EntityRepository<PasswordHistory>, IPasswordHistoryRepository
    {
        public PasswordHistoryRepository(HrHubDbContext dbContext) : base(dbContext)
        {
        }
    }
}
