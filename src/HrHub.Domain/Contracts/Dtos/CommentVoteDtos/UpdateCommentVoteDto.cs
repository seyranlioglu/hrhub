using HrHub.Abstraction.Attributes;
using HrHub.Core.BusinessRules;

namespace HrHub.Domain.Contracts.Dtos.CommentVoteDtos
{
    public class UpdateCommentVoteDto
    {
        [ValidationRules(typeof(ZeroCheckRule))]
        public long Id { get; set; }
        public bool Positive { get; set; }
    }
}
