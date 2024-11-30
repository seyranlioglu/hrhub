using HrHub.Abstraction.Consts;
using HrHub.Abstraction.Contracts.Dtos.TrainingDtos;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;

namespace HrHub.Application.BusinessRules.TrainingBusinessRules
{

    public class ExistTrainingBusinessRule : IExistTrainingBusinessRule
    {
        private readonly IHrUnitOfWork unitOfWork;
        private readonly Repository<Training> trainingRepository;

        public ExistTrainingBusinessRule(IHrUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            trainingRepository = unitOfWork.CreateRepository<Training>();
        }

        public (bool IsValid, string ErrorMessage) Validate(object value, string fieldName)
        {
            if (value is GetTrainingDto trainingDto && trainingDto is not null)
            {
                if (trainingDto is null)
                    return (false, ValidationMessages.TrainingExistsError);
            }

            return (true, string.Empty);
        }
    }
}
