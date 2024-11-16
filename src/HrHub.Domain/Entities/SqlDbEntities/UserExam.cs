using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class UserExam : CardEntity<long>
    {
        public UserExam()
        {
            UserAnswers = new HashSet<UserAnswer>();
        }
        public long ExamId { get; set; }
        public long CurrAccTrainingUserId { get; set; }
        public long? TotalAnswer { get; set; }
        public long? TotalCorrectAnswer { get; set; }
        public long? ExamScore { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }


        [ForeignKey("ExamId")]
        public virtual Exam Exam { get; set; }

        [ForeignKey("CurrAccTrainingUserId")]
        public virtual CurrAccTrainingUser CurrAccTrainingUser { get; set; }

        public virtual ICollection<UserAnswer> UserAnswers { get; set; }
    }
}
