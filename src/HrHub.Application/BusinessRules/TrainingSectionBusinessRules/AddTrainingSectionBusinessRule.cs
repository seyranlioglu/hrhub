using HrHub.Abstraction.Consts;
using HrHub.Abstraction.Contracts.Dtos.TrainingDtos;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Contracts.Dtos.TrainingSectionDtos;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;
using ServiceStack.Script;

namespace HrHub.Application.BusinessRules.TrainingSectionBusinessRules
{
    public class AddTrainingSectionBusinessRule : IAddTrainingSectionBusinessRule
    {
        private readonly IHrUnitOfWork hrUnitOfWork;
        private readonly Repository<TrainingSection> trainingSectionRepository;
        private readonly Repository<Training> trainingRepository;
        public AddTrainingSectionBusinessRule(IHrUnitOfWork hrUnitOfWork)
        {
            this.hrUnitOfWork = hrUnitOfWork;
            this.trainingSectionRepository = hrUnitOfWork.CreateRepository<TrainingSection>();
            this.trainingRepository = hrUnitOfWork.CreateRepository<Training>();
        }

        public (bool IsValid, string ErrorMessage) Validate(object value, string fieldName)
        {
            if (value is AddTrainingSectionDto trainingSectionDto && trainingSectionDto is not null)
            {
                var isTrainingExist = trainingRepository.Exists(predicate: p => p.Id == trainingSectionDto.TrainingId);
                if (!isTrainingExist)
                    return (false, ValidationMessages.TrainingNotExistsError);


                var isTrainingSectionExist = trainingSectionRepository.Exists(predicate: p => p.Code == trainingSectionDto.Code
                                                                                             && p.Title == trainingSectionDto.Title
                                                                                             && p.Description == trainingSectionDto.Description
                                                                                             && p.TrainingId == trainingSectionDto.TrainingId
                                                                                             && p.LangCode == trainingSectionDto.LangCode
                                                                                             && p.RowNumber == trainingSectionDto.RowNumber);
                if (isTrainingSectionExist)
                    return (false, ValidationMessages.TrainingSectionExistsError);
            }
            return (true, string.Empty);
        }
    }
}
