using HrHub.Core.Domain.Entity;
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


        public virtual ICollection<TrainingAnnouncementsComment> TrainingAnnouncementsComments { get; set; }
    }
}
