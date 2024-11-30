using HrHub.Abstraction.Consts;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Contracts.Dtos.WhatYouWillLearns;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;

namespace HrHub.Application.BusinessRules.WhatYouWillLearnBusinessRule
{
    public class AddWhatYouWillLearnBusinessRule : IAddWhatYouWillLearnBusinessRule
    {
        private readonly IHrUnitOfWork hrUnitOfWork;
        private readonly Repository<Training> trainingRepository;
        private readonly Repository<WhatYouWillLearn> whatYouWillLearnRepository;

        public AddWhatYouWillLearnBusinessRule(IHrUnitOfWork hrUnitOfWork)
        {
            this.hrUnitOfWork = hrUnitOfWork;
            this.whatYouWillLearnRepository = hrUnitOfWork.CreateRepository<WhatYouWillLearn>();
            this.trainingRepository = hrUnitOfWork.CreateRepository<Training>();
        }

        public (bool IsValid, string ErrorMessage) Validate(object value, string fieldName)
        {
            if (value is AddWhatYouWillLearnDto dto && dto is not null)
            {
                var trainingExist = trainingRepository.Exists(predicate: p => p.Id == dto.TrainingId);
                if (!trainingExist)
                    return (false, ValidationMessages.TrainingNotExistsError);

                var isEntityExist = whatYouWillLearnRepository.Exists(predicate: p => p.TrainingId == dto.TrainingId
                                                                                      && p.Code == dto.Code
                                                                                      && p.Title == dto.Title);
                if (isEntityExist)
                    return (false, ValidationMessages.WhatYouWillLearnExistsError);
            }
            return (true, string.Empty);
        }
    }
}
