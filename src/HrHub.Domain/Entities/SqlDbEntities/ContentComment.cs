using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class ContentComment : TypeCardEntity<long>
    {
        public ContentComment()
        {
            CommentVotes = new HashSet<CommentVote>();
            SubContentComment = new HashSet<ContentComment>();
        }
        public int ContentId { get; set; }
        public bool Pinned { get; set; }
        public long UserId { get; set; }
        public DateTime ContentDate { get; set; }
        public int MasterContentId { get; set; }


        [ForeignKey("MasterContentId")]
        public virtual ContentComment MasterContentComment { get; set; }

        public virtual ICollection<ContentComment> SubContentComment { get; set; } = null;


        [ForeignKey("ContentId")]
        public virtual TrainingContent TrainingContent { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }


        public virtual ICollection<CommentVote> CommentVotes { get; set; } = null;
    }
}
