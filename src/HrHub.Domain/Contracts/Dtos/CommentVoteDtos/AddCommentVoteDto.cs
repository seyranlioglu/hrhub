using HrHub.Abstraction.Attributes;
using HrHub.Core.BusinessRules;

namespace HrHub.Domain.Contracts.Dtos.CommentVoteDtos
{
    public class AddCommentVoteDto
    {
        [ValidationRules(typeof(ZeroCheckRule))]
        public long ContentCommentId { get; set; }
        [ValidationRules(typeof(ZeroCheckRule))]
        public long UserId { get; set; }
        public bool Positive { get; set; }
    }
}
