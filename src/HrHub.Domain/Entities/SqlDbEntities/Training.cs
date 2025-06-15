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
        public long? CategoryId { get; set; }
        public long? InstructorId { get; set; }
        public long? TrainingTypeId { get; set; }
        public decimal? CurrentAmount { get; set; }
        public decimal? Amount { get; set; }
        public decimal? DiscountRate { get; set; }
        public decimal? TaxRate { get; set; }
        public decimal CertificateOfAchievementRate { get; set; }
        public decimal? CertificateOfParticipationRate { get; set; }
        public DateTime? CompletionTime { get; set; }
        public long? CompletionTimeUnitId { get; set; }
        public long? TrainingLevelId { get; set; }
        public long? TrainingStatusId { get; set; }
        public long? PreconditionId { get; set; }
        public long? ForWhomId { get; set; } = null;
        public string? SubTitle { get; set; }
        public string? Labels { get; set; }
        public string? CourseImage { get; set; }
        public string? Trailer { get; set; }
        public string? WelcomeMessage { get; set; }
        public string? CongratulationMessage { get; set; }
        public long? EducationLevelId { get; set; }
        public long? PriceTierId { get; set; }

        [ForeignKey("TrainingTypeId")]
        public virtual TrainingType TrainingType { get; set; }

        [ForeignKey("CategoryId")]
        public virtual TrainingCategory? TrainingCategory { get; set; }

        [ForeignKey("InstructorId")]
        public virtual Instructor? Instructor { get; set; }

        [ForeignKey("CompletionTimeUnitId")]
        public virtual TimeUnit? TimeUnit { get; set; }

        [ForeignKey("TrainingLevelId")]
        public virtual TrainingLevel? TrainingLevel { get; set; }

        [ForeignKey("TrainingStatusId")]
        public virtual TrainingStatus? TrainingStatus { get; set; }

        [ForeignKey("PreconditionId")]
        public virtual Precondition? Precondition { get; set; }

        [ForeignKey("ForWhomId")]
        public virtual ForWhom? ForWhom { get; set; }

        [ForeignKey("EducationLevelId")]
        public virtual EducationLevel? EducationLevel { get; set; }

        [ForeignKey("PriceTierId")]
        public virtual PriceTier? PriceTier { get; set; }

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
