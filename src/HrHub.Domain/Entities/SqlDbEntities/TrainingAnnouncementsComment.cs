using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class TrainingAnnouncementsComment : TypeCardEntity<long>
    {
        [ForeignKey("TrainingAnnouncementsId")]
        public long TrainingAnnouncementsId { get; set; }
        
        public long UserId { get; set; }

        public virtual TrainingAnnouncement TrainingAnnouncement { get; set; }
    }
}
