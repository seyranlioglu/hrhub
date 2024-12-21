using HrHub.Abstraction.Consts;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Contracts.Dtos.ContentTypes;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;

namespace HrHub.Application.BusinessRules.ContentTypeBusinessRules
{
    public class AddContentTypeBusinessRule : IAddContentTypeBusinessRule
    {
        private readonly IHrUnitOfWork unitOfWork;
        private readonly Repository<ContentType> contentTypeRepository;

        public AddContentTypeBusinessRule(IHrUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.contentTypeRepository = unitOfWork.CreateRepository<ContentType>();
        }

        public (bool IsValid, string ErrorMessage) Validate(object value, string fieldName)
        {
            if (value is AddContentTypeDto contentTypeDto && contentTypeDto is not null)
            {
                var isContentTypeExist = contentTypeRepository
                   .Exists(
                   predicate: p => p.Title == contentTypeDto.Title
                                   && p.Description == contentTypeDto.Description
                                   && p.Code == contentTypeDto.Code
                                   && p.Abbreviation == contentTypeDto.Abbreviation
                                   && p.LangCode == contentTypeDto.LangCode);
                if (isContentTypeExist)
                    return (false, ValidationMessages.DataAlreadyExists);

            }
            return (true, string.Empty);
        }
    }
}
