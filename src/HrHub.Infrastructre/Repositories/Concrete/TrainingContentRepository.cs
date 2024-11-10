using HrHub.Core.Data.Repository;
using HrHub.Domain.Contexts;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.Repositories.Abstract;

namespace HrHub.Infrastructre.Repositories.Concrete
{
    public class TrainingContentRepository : EntityRepository<TrainingContent>, ITrainingContentRepository
    {
        public TrainingContentRepository(HrHubDbContext dbContext) : base(dbContext)
        {
        }
    }
}
