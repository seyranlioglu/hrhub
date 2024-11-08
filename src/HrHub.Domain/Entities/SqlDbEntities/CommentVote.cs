using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class CommentVote : CardEntity<int>
    {
        public long CommentId { get; set; }
        public long UserId { get; set; }
    }
}
