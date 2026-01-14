using HrHub.Domain.Contexts;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.Repositories.Abstract;
using HrHub.Core.Data.Repository;

namespace HrHub.Infrastructre.Repositories.Concrete
{
    public class TrainingProcessRequestRepository : EntityRepository<TrainingProcessRequest>, ITrainingProcessRequestRepository
    {
        public TrainingProcessRequestRepository(HrHubDbContext context) : base(context)
        {
        }
    }
}