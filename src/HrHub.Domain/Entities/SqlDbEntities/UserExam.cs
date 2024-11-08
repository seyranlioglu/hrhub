using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class UserExam : CardEntity<int>
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

        public virtual ICollection<UserAnswer> UserAnswers { get; set; }
    }
}
