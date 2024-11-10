using HrHub.Core.Data.Repository;
using HrHub.Domain.Contexts;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.Repositories.Abstract;

namespace HrHub.Infrastructre.Repositories.Concrete
{
    public class UserRepository : EntityRepository<User>, IUserRepository
    {
        public UserRepository(HrHubDbContext dbContext) : base(dbContext)
        {
        }
    }
}
