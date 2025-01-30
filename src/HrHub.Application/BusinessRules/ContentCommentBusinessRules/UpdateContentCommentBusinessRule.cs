using HrHub.Abstraction.BusinessRules;
using HrHub.Abstraction.Consts;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Contracts.Dtos.ContentCommentDtos;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;

namespace HrHub.Application.BusinessRules.ContentCommentBusinessRules
{

    public class UpdateContentCommentBusinessRule : IUpdateContentCommentBusinessRule
    {
        private readonly IHrUnitOfWork hrUnitOfWork;
        private readonly Repository<ContentComment> contentCommentRepository;


        public UpdateContentCommentBusinessRule(IHrUnitOfWork hrUnitOfWork)
        {
            this.hrUnitOfWork = hrUnitOfWork;
            this.contentCommentRepository = hrUnitOfWork.CreateRepository<ContentComment>();
        }

        (bool IsValid, string ErrorMessage) IBusinessRule.Validate(object value, string fieldName)
        {
            if (value is UpdateContentCommentDto contentCommentDto && contentCommentDto is not null)
            {
                var isContentCommentExist = contentCommentRepository
                   .Exists(
                   predicate: p => p.Id == contentCommentDto.Id);
                if (isContentCommentExist)
                    return (false, ValidationMessages.ContentCommentNotFoundError);
            }

            return (true, string.Empty);
        }
    }

}