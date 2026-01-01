using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class UserContentsViewLog : CardEntity<long>
    {
        public UserContentsViewLog()
        {
            UserContentsViewLogDetails = new HashSet<UserContentsViewLogDetail>();
        }
        public long TrainingContentId { get; set; }
        public long CurrAccTrainingUserId { get; set; }
        public bool IsActive { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime StartDate { get; set; }
        [AllowNull]
        public DateTime? CompletedDate { get; set; }

        [ForeignKey("TrainingContentId")]
        public virtual TrainingContent TrainingContent { get; set; }

        [ForeignKey("CurrAccTrainingUserId")]
        public virtual CurrAccTrainingUser CurrAccTrainingUser { get; set; }


        public virtual ICollection<UserContentsViewLogDetail> UserContentsViewLogDetails { get; set; } = null;
    }
}
