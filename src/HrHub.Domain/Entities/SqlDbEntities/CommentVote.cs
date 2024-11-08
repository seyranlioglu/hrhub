using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class CommentVote : CardEntity<long>
    {
        public long ContentCommentId { get; set; }
        public long UserId { get; set; }


        [ForeignKey("ContentCommentId")]
        public virtual ContentComment ContentComment { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
