using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class CommentVote : CardEntity<int>
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }
    }
}
