using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;
namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class TrainingAnnouncement : TypeCardEntity<long>
    {
        public TrainingAnnouncement()
        {
             TrainingAnnouncementsComments = new HashSet<TrainingAnnouncementsComment>();
        }
        public long TrainingId { get; set; }
        public long UserId { get; set; }

        [ForeignKey("TrainingId")]
        public virtual Training Training { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public virtual ICollection<TrainingAnnouncementsComment> TrainingAnnouncementsComments { get; set; }
    }
}
