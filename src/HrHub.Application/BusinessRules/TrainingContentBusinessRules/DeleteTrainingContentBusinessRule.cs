using HrHub.Abstraction.Consts;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Contracts.Dtos.TrainingContentDtos;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;

namespace HrHub.Application.BusinessRules.TrainingContentBusinessRules
{
    public class DeleteTrainingContentBusinessRule : IDeleteTrainingContentBusinessRule
    {
        private readonly IHrUnitOfWork hrUnitOfWork;
        private readonly Repository<TrainingContent> trainingContentRepository;

        public DeleteTrainingContentBusinessRule(IHrUnitOfWork hrUnitOfWork
                                            )
        {
            this.hrUnitOfWork = hrUnitOfWork;
            this.trainingContentRepository = hrUnitOfWork.CreateRepository<TrainingContent>();
        }

        public (bool IsValid, string ErrorMessage) Validate(object value, string fieldName)
        {
            if (value is DeleteTrainingContentDto trainingContentDto && trainingContentDto is not null)
            {
                
                var isTrainingContentExist = trainingContentRepository.Exists(predicate: p => p.Id == trainingContentDto.Id);
                if (!isTrainingContentExist)
                    return (false, ValidationMessages.TrainingContentNotExistsError);
            }
            return (true, string.Empty);
        }
    }
}
