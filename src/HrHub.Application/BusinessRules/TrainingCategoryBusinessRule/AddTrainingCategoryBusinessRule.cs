using HrHub.Abstraction.Consts;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Contracts.Dtos.TrainingCategoryDtos;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;

namespace HrHub.Application.BusinessRules.TrainingCategoryBusinessRule
{
    public class AddTrainingCategoryBusinessRule : IAddTrainingCategoryBusinessRule
    {
        private readonly IHrUnitOfWork unitOfWork;
        private readonly Repository<TrainingCategory> trainingCategoryRepository;

        public AddTrainingCategoryBusinessRule(IHrUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.trainingCategoryRepository = unitOfWork.CreateRepository<TrainingCategory>();
        }

        public (bool IsValid, string ErrorMessage) Validate(object value, string fieldName)
        {
            if (value is AddTrainingCategoryDto categoryDto && categoryDto is not null)
            {
                var isCategoryExist = trainingCategoryRepository
                    .Exists(
                    predicate: p => p.Title == categoryDto.Title
                                    && p.Description == categoryDto.Description
                                    && p.Code == categoryDto.Code
                                    && p.Abbreviation == categoryDto.Abbreviation
                                    && p.MasterCategoryId == categoryDto.MasterCategoryId);
                if (isCategoryExist)
                    return (false, ValidationMessages.CategoryExistsError);

                if (categoryDto.MasterCategoryId.HasValue)
                {
                    var isMasterCategoryIdExist = trainingCategoryRepository.Exists(predicate: p => p.Id == categoryDto.MasterCategoryId);

                    if (isMasterCategoryIdExist)
                        return (false, ValidationMessages.CategoryExistsError);

                }
            }
            return (true, string.Empty);
        }
    }
}
