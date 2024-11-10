using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class UserContentsViewLog : CardEntity<long>
    {
        public UserContentsViewLog()
        {
            UserContentsViewLogs = new HashSet<UserContentsViewLog>();
        }
        public long TrainingContentId { get; set; }
        public long CurrAccTrainingUserId { get; set; }
        public bool IsActive { get; set; }
        public bool State { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime CompletedDate { get; set; }

        [ForeignKey("TrainingContentId")]
        public virtual TrainingContent TrainingContent { get; set; }

        [ForeignKey("CurrAccTrainingUserId")]
        public virtual CurrAcc CurrAcc { get; set; }


        public virtual ICollection<UserContentsViewLog> UserContentsViewLogs { get; set; } = null;
    }
}
