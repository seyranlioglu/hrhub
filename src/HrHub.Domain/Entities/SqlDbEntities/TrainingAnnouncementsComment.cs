using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class TrainingAnnouncementsComment : TypeCardEntity<int>
    {
        [ForeignKey("TrainingAnnouncementsId")]
        public int TrainingAnnouncementsId { get; set; }
        
        public int UserId { get; set; }

        public virtual TrainingAnnouncement TrainingAnnouncement { get; set; }
    }
}
