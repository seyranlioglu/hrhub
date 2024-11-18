using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class Training : TypeCardEntity<long>
    {

        public Training()
        {
            CartItems = new HashSet<CartItem>();
            Exams = new HashSet<Exam>();
            Reviews = new HashSet<Review>();
            TrainingAnnouncements = new HashSet<TrainingAnnouncement>();
            TrainingSections = new HashSet<TrainingSection>();
            WhatYouWillLearns = new HashSet<WhatYouWillLearn>();
            CurrAccTrainings = new HashSet<CurrAccTraining>();
            TrainingAmounts = new HashSet<TrainingAmount>();
        }

        public string? HeaderImage { get; set; }
        public string? LangCode { get; set; }
        public int CategoryId { get; set; }
        public int InstructorId { get; set; }
        public string? TrainingType { get; set; }
        public decimal CurrentAmount { get; set; }
        public decimal Amount { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal TaxRate { get; set; }
        public decimal CertificateOfAchievementRate { get; set; }
        public decimal CertificateOfParticipationRate { get; set; }
        public DateTime? CompletionTime { get; set; }
        public long CompletionTimeUnitId { get; set; }
        public long TrainingLevelId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual ICollection<TrainingCategory> TrainingCategories { get; set; }

        [ForeignKey("InstructorId")]
        public virtual ICollection<Instructor> Instructors { get; set; }

        [ForeignKey("CompletionTimeUnitId")]
        public virtual ICollection<TimeUnit> TimeUnits { get; set; }

        [ForeignKey("TrainingLevelId")]
        public virtual ICollection<TrainingLevel> TrainingLevels { get; set; }



        public virtual ICollection<CartItem> CartItems { get; set; } = null;
        public virtual ICollection<Exam> Exams { get; set; } = null;
        public virtual ICollection<Review> Reviews { get; set; } = null;
        public virtual ICollection<TrainingAnnouncement> TrainingAnnouncements { get; set; } = null;
        public virtual ICollection<TrainingSection> TrainingSections { get; set; } = null;
        public virtual ICollection<WhatYouWillLearn> WhatYouWillLearns { get; set; } = null;
        public virtual ICollection<CurrAccTraining> CurrAccTrainings { get; set; } = null;
        public virtual ICollection<TrainingAmount> TrainingAmounts { get; set; } = null;
    }
}
