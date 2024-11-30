using HrHub.Abstraction.Consts;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Contracts.Dtos.TrainingSectionDtos;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;

namespace HrHub.Application.BusinessRules.TrainingSectionBusinessRules
{
    public class ExistTrainingSectionBusinessRule : IExistTrainingSectionBusinessRule
    {
        private readonly IHrUnitOfWork hrUnitOfWork;
        private readonly Repository<TrainingSection> trainingSectionRepository;
        private readonly Repository<Training> trainingRepository;
        public ExistTrainingSectionBusinessRule(IHrUnitOfWork hrUnitOfWork)
        {
            this.hrUnitOfWork = hrUnitOfWork;
            this.trainingSectionRepository = hrUnitOfWork.CreateRepository<TrainingSection>();
            this.trainingRepository = hrUnitOfWork.CreateRepository<Training>();
        }
        public (bool IsValid, string ErrorMessage) Validate(object value, string fieldName)
        {
            if (value is GetTrainingSectionDto dto && dto is not null)
            {
                var isEntityExist = trainingSectionRepository
                    .Exists(
                    predicate: p => p.Id == dto.Id);

                if (!isEntityExist)
                    return (false, ValidationMessages.TrainingSectionNotExistsError);
            }
            return (false, string.Empty);
        }
    }
}
