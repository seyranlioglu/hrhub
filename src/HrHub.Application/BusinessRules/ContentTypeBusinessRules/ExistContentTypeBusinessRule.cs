using HrHub.Abstraction.Consts;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Contracts.Dtos.ContentTypes;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;

namespace HrHub.Application.BusinessRules.ContentTypeBusinessRules
{
    public class ExistContentTypeBusinessRule : IExistContentTypeBusinessRule
    {
        private readonly IHrUnitOfWork unitOfWork;
        private readonly Repository<ContentType> contentTypeRepository;

        public ExistContentTypeBusinessRule(IHrUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            contentTypeRepository = unitOfWork.CreateRepository<ContentType>();
        }

        public (bool IsValid, string ErrorMessage) Validate(object value, string fieldName)
        {
            if (value is ContentTypeDto dto && dto is not null)
            {
                if (dto is null)
                    return (false, ValidationMessages.TrainingExistsError);
            }

            return (true, string.Empty);
        }
    }
}
