using HrHub.Abstraction.Consts;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Contracts.Dtos.WhatYouWillLearnsDtos;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;

namespace HrHub.Application.BusinessRules.WhatYouWillLearnBusinessRule
{
    public class ExistWhatYouWillLearnBusinessRule : IExistWhatYouWillLearnBusinessRule
    {
        private readonly IHrUnitOfWork unitOfWork;
        private readonly Repository<WhatYouWillLearn> whatYouWillLearnRepository;

        public ExistWhatYouWillLearnBusinessRule(IHrUnitOfWork unitOfWork
                                                 )
        {
            this.unitOfWork = unitOfWork;
            this.whatYouWillLearnRepository = unitOfWork.CreateRepository<WhatYouWillLearn>();
        }

        public (bool IsValid, string ErrorMessage) Validate(object value, string fieldName)
        {
            if (value is GetWhatYouWillLearnDto dto && dto is not null)
            {
                var isEntityExist = whatYouWillLearnRepository
                    .Exists(
                    predicate: p => p.Id == dto.Id);

                if (!isEntityExist)
                    return (false, ValidationMessages.WhatYouWillLearnNotExistsError);
            }

            return (true, string.Empty);
        }
    }
}
