using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class User : CardEntity<long>
    {
        public User()
        {
            ConfirmCarts = new HashSet<Cart>();
            CommentVotes = new HashSet<CommentVote>();
            ContentComments = new HashSet<ContentComment>();
            AddCartUser = new HashSet<Cart>();
            ContentNotes = new HashSet<ContentNote>();
            CurrAccTrainingUsers = new HashSet<CurrAccTrainingUser>();
            Instructors = new HashSet<Instructor>();
            Reviews = new HashSet<Review>();
            TrainingAnnouncements = new HashSet<TrainingAnnouncement>();
            TrainingAnnouncementsComments = new HashSet<TrainingAnnouncementsComment>();
            CurrAccTrainings = new HashSet<CurrAccTraining>();
        }

        public long CurrAccId { get; set; }

        [ForeignKey("CurrAccId")]
        public virtual CurrAcc CurrAcc { get; set; }

        public virtual ICollection<Cart> ConfirmCarts { get; set; }
        public virtual ICollection<Cart> AddCartUser { get; set; }
        public virtual ICollection<CommentVote> CommentVotes { get; set; }
        public virtual ICollection<ContentComment> ContentComments { get; set; }
        public virtual ICollection<ContentNote> ContentNotes { get; set; }
        public virtual ICollection<CurrAccTrainingUser> CurrAccTrainingUsers { get; set; }
        public virtual ICollection<Instructor> Instructors { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<TrainingAnnouncement> TrainingAnnouncements { get; set; }
        public virtual ICollection<TrainingAnnouncementsComment> TrainingAnnouncementsComments { get; set; }
        public virtual ICollection<CurrAccTraining> CurrAccTrainings { get; set; }
    }
}
