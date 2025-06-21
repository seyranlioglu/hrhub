using HrHub.Abstraction.Consts;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Contracts.Dtos.TrainingSectionDtos;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;

namespace HrHub.Application.BusinessRules.TrainingSectionBusinessRules
{
    public class UpdateTrainingSectionBusinessRule : IUpdateTrainingSectionBusinessRule
    {
        private readonly IHrUnitOfWork hrUnitOfWork;
        private readonly Repository<TrainingSection> trainingSectionRepository;
        private readonly Repository<Training> trainingRepository;

        public UpdateTrainingSectionBusinessRule(IHrUnitOfWork hrUnitOfWork)
        {
            this.hrUnitOfWork = hrUnitOfWork;
            this.trainingSectionRepository = hrUnitOfWork.CreateRepository<TrainingSection>();
            this.trainingRepository = hrUnitOfWork.CreateRepository<Training>();
        }

        public (bool IsValid, string ErrorMessage) Validate(object value, string fieldName)
        {
            if (value is UpdateTrainingSectionDto trainingSectionDto && trainingSectionDto is not null)
            {
                var isTrainingExist = trainingSectionRepository.Exists(predicate: p => p.Id == trainingSectionDto.Id);
                if (!isTrainingExist)
                    return (false, ValidationMessages.TrainingSectionExistsError);
            }
            return (true, string.Empty);
        }
    }
}
