using HrHub.Abstraction.Attributes;
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
            Reviews = new HashSet<Review>();
            TrainingAnnouncements = new HashSet<TrainingAnnouncement>();
            TrainingAnnouncementsComments = new HashSet<TrainingAnnouncementsComment>();
            CurrAccTrainings = new HashSet<CurrAccTraining>();
            ExamResults = new HashSet<ExamResult>();
            Exams = new HashSet<Exam>();
        }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string UserShortName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? NormalizedUserName { get; set; }
        public string? NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? PasswordHash { get; set; }
        public string? SecurityStamp { get; set; }
        public string? ConcurrencyStamp { get; set; }
        public  bool PhoneNumberConfirmed { get; set; }
        public  bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public  bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public bool IsMainUser { get; set; }
        public string? AuthCode { get; set; }
        public long CurrAccId { get; set; }
        [ForeignKey("CurrAccId")]
        public virtual CurrAcc CurrAcc { get; set; }

        public virtual ICollection<Cart> ConfirmCarts { get; set; } = null;
        public virtual ICollection<Cart> AddCartUser { get; set; } = null;
        public virtual ICollection<CommentVote> CommentVotes { get; set; } = null;
        public virtual ICollection<ContentComment> ContentComments { get; set; } = null;
        public virtual ICollection<ContentNote> ContentNotes { get; set; } = null;
        public virtual ICollection<CurrAccTrainingUser> CurrAccTrainingUsers { get; set; } = null;
        public virtual Instructor Instructor { get; set; } = null;
        public virtual ICollection<Review> Reviews { get; set; } = null;
        public virtual ICollection<TrainingAnnouncement> TrainingAnnouncements { get; set; } = null;
        public virtual ICollection<TrainingAnnouncementsComment> TrainingAnnouncementsComments { get; set; } = null;
        public virtual ICollection<CurrAccTraining> CurrAccTrainings { get; set; } = null;
        public virtual ICollection<ExamResult> ExamResults { get; set; } = null;
        public virtual ICollection<Exam> Exams { get; set; } = null;
    }
}
