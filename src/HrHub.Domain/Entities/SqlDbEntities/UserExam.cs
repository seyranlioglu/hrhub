using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class UserExam : CardEntity<int>
    {
        public UserExam()
        {
            UserAnswers = new HashSet<UserAnswer>();
        }
        public int ExamId { get; set; }
        public int CurrAccTrainingUserId { get; set; }
        public int? TotalAnswer { get; set; }
        public int? TotalCorrectAnswer { get; set; }
        public int? ExamScore { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public virtual ICollection<UserAnswer> UserAnswers { get; set; }
    }
}
