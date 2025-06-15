using HrHub.Abstraction.Consts;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Contracts.Dtos.WhatYouWillLearns;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;

namespace HrHub.Application.BusinessRules.WhatYouWillLearnBusinessRule
{
    public class UpdateWhatYouWillLearnBusinessRule : IUpdateWhatYouWillLearnBusinessRule
    {
        private readonly IHrUnitOfWork hrUnitOfWork;
        private readonly Repository<Training> trainingRepository;
        private readonly Repository<WhatYouWillLearn> whatYouWillLearnRepository;

        public UpdateWhatYouWillLearnBusinessRule(IHrUnitOfWork hrUnitOfWork
                                               )
        {
            this.hrUnitOfWork = hrUnitOfWork;
            this.trainingRepository = hrUnitOfWork.CreateRepository<Training>();
            this.whatYouWillLearnRepository = hrUnitOfWork.CreateRepository<WhatYouWillLearn>();
        }

        public (bool IsValid, string ErrorMessage) Validate(object value, string fieldName)
        {
            if (value is UpdateWhatYouWillLearnDto dto && dto is not null)
            {
                //var trainingExist = trainingRepository.Exists(predicate: p => p.Id == dto.TrainingId);
                //if (!trainingExist)
                //    return (false, ValidationMessages.TrainingNotExistsError);

                var isEntityExist = whatYouWillLearnRepository.Exists(predicate: p => p.TrainingId == dto.TrainingId);
                if (!isEntityExist)
                    return (false, ValidationMessages.WhatYouWillLearnNotExistsError);
            }
            return (true, string.Empty);
        }
    }
}
