using HrHub.Abstraction.Consts;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Contracts.Dtos.ContentTypes;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;

namespace HrHub.Application.BusinessRules.ContentTypeBusinessRules
{
    public class UpdateContentTypeBusinessRule : IUpdateContentTypeBusinessRule
    {
        private readonly IHrUnitOfWork unitOfWork;
        private readonly Repository<ContentType> contentTypeRepository;

        public UpdateContentTypeBusinessRule(IHrUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.contentTypeRepository = unitOfWork.CreateRepository<ContentType>();
        }

        public (bool IsValid, string ErrorMessage) Validate(object value, string fieldName)
        {
            if (value is UpdateContentTypeDto dto && dto is not null)
            {

                var existingContentTypeDto = contentTypeRepository.Exists(predicate: p => p.Id == dto.Id);
                if (!existingContentTypeDto)
                    return (false, ValidationMessages.DataNotFound);
            }
            return (true, string.Empty);
        }
    }
}
