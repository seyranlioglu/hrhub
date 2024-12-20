using HrHub.Abstraction.Consts;
using HrHub.Abstraction.Result;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Contracts.Dtos.TrainingCategoryDtos;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Threading;

namespace HrHub.Application.BusinessRules.TrainingCategoryBusinessRule
{
    public class UpdateTrainingCategoryBusinessRule : IUpdateTrainingCategoryBusinessRule
    {
        private readonly IHrUnitOfWork unitOfWork;
        private readonly Repository<TrainingCategory> trainingCategoryRepository;

        public UpdateTrainingCategoryBusinessRule(IHrUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.trainingCategoryRepository = unitOfWork.CreateRepository<TrainingCategory>();
        }

        public (bool IsValid, string ErrorMessage) Validate(object value, string fieldName)
        {
            if (value is UpdateTrainingCategoryDto categoryDto && categoryDto is not null)
            {

                var existingCategory = trainingCategoryRepository.Exists(predicate: p => p.Id == categoryDto.Id);
                if (!existingCategory)
                    return (false, ValidationMessages.CategoryExistsError);

                if (categoryDto.MasterCategoryId.HasValue)
                {
                    // MasterCategoryId geçerli mi?
                    var masterCategoryExists = trainingCategoryRepository.Exists(predicate: p => p.Id == categoryDto.MasterCategoryId);
                    if (!masterCategoryExists)
                        return (false, ValidationMessages.MasterCategoryNotFoundError);

                    // Recursive kontrol: Kategori kendi altına atanıyor mu?
                    if (categoryDto.MasterCategoryId == categoryDto.Id)
                        return (false, ValidationMessages.RecursiveCategoryError);

                }


            }
            return (true, string.Empty);
        }
    }
}
