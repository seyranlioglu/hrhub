using HrHub.Core.Base;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;
using Microsoft.AspNetCore.Http;

namespace HrHub.Application.Managers.TrainingSections
{
    public class TrainingSectionManager : ManagerBase, ITrainingSectionManager
    {
        private readonly IHrUnitOfWork hrUnitOfWork;
        private readonly Repository<TrainingSection> trainingSectionRepository;

        public TrainingSectionManager(IHttpContextAccessor httpContextAccessor,
                                      IHrUnitOfWork hrUnitOfWork) : base(httpContextAccessor)
        {
            this.hrUnitOfWork = hrUnitOfWork;
            this.trainingSectionRepository = hrUnitOfWork.CreateRepository<TrainingSection>();
        }



    }
}
