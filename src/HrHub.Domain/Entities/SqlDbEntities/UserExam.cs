using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class UserExam : CardEntity<long>
    {
        public UserExam()
        {
            UserAnswers = new HashSet<UserAnswer>();
        }
        public long ExamVersionId { get; set; }
        public long CurrAccTrainingUserId { get; set; }
        public long? TotalAnswer { get; set; }
        public long? TotalCorrectAnswer { get; set; }
        public decimal? ExamScore { get; set; }
        public decimal TotalScore { get; set; }
        [AllowNull]
        public decimal? SuccessRate { get; set; }
        [AllowNull]
        public decimal? PassingScore { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [AllowNull]
        public int? CurrentTopicSeqNumber { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsSuccess { get; set; }



        [ForeignKey("ExamVersionId")]
        public virtual ExamVersion ExamVersion { get; set; }

        [ForeignKey("CurrAccTrainingUserId")]
        public virtual CurrAccTrainingUser CurrAccTrainingUser { get; set; }

        public virtual ICollection<UserAnswer> UserAnswers { get; set; }
    }
}
