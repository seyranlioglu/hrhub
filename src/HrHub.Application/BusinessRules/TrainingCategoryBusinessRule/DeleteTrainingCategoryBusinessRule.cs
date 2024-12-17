using HrHub.Abstraction.Consts;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Contracts.Dtos.TrainingCategoryDtos;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;

namespace HrHub.Application.BusinessRules.TrainingCategoryBusinessRule
{
    public class DeleteTrainingCategoryBusinessRule : IDeleteTrainingCategoryBusinessRule
    {
        private readonly IHrUnitOfWork unitOfWork;
        private readonly Repository<TrainingCategory> trainingCategoryRepository;

        public DeleteTrainingCategoryBusinessRule(IHrUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.trainingCategoryRepository = unitOfWork.CreateRepository<TrainingCategory>();
        }

        public (bool IsValid, string ErrorMessage) Validate(object value, string fieldName)
        {
            if (value is DeleteTrainingCategoryDto categoryDto && categoryDto is not null)
            {
                var existingCategory = trainingCategoryRepository.Exists(predicate: p => p.Id == categoryDto.Id);
                if (!existingCategory)
                    return (false, ValidationMessages.CategoryExistsError);
            }
            return (true, string.Empty);
        }
    }
}
