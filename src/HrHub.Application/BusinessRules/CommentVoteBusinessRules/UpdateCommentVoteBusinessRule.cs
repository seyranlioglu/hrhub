using HrHub.Abstraction.BusinessRules;
using HrHub.Abstraction.Consts;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Contracts.Dtos.CommentVoteDtos;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;

namespace HrHub.Application.BusinessRules.CommentVoteBusinessRules
{

    public class UpdateCommentVoteBusinessRule : IUpdateCommentVoteBusinessRule
    {
        private readonly IHrUnitOfWork hrUnitOfWork;
        private readonly Repository<CommentVote> CommentVoteRepository;


        public UpdateCommentVoteBusinessRule(IHrUnitOfWork hrUnitOfWork)
        {
            this.hrUnitOfWork = hrUnitOfWork;
            this.CommentVoteRepository = hrUnitOfWork.CreateRepository<CommentVote>();
        }

        (bool IsValid, string ErrorMessage) IBusinessRule.Validate(object value, string fieldName)
        {
            if (value is UpdateCommentVoteDto CommentVoteDto && CommentVoteDto is not null)
            {
                var isCommentVoteExist = CommentVoteRepository
                   .Exists(
                   predicate: p => p.Id == CommentVoteDto.Id);
                if (isCommentVoteExist)
                    return (false, ValidationMessages.CommentVoteNotFoundError);
            }

            return (true, string.Empty);
        }
    }

}