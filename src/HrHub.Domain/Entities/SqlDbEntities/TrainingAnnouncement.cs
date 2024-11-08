using HrHub.Core.Domain.Entity;
namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class TrainingAnnouncement : TypeCardEntity<int>
    {
        public TrainingAnnouncement()
        {
             TrainingAnnouncementsComments = new HashSet<TrainingAnnouncementsComment>();
        }
        public int TrainingId { get; set; }
        public int UserId { get; set; }


        public virtual ICollection<TrainingAnnouncementsComment> TrainingAnnouncementsComments { get; set; }
    }
}
