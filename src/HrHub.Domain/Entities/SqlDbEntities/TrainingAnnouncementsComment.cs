using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class TrainingAnnouncementsComment : TypeCardEntity<long>
    {
        public long TrainingAnnouncementsId { get; set; }
        public long UserId { get; set; }

        [ForeignKey("TrainingAnnouncementsId")]
        public virtual TrainingAnnouncement TrainingAnnouncement { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
