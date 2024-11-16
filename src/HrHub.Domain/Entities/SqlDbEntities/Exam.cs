using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class Exam : CardEntity<long>
    {
        public Exam()
        {
            ExamTopics = new HashSet<ExamTopic>();
            UserExams = new HashSet<UserExam>();
        }
        public long TrainingId { get; set; }
        public TimeSpan ExamTime { get; set; }
        public long SuccesRate { get; set; }
        public long ViewQuestionCount { get; set; }

        [ForeignKey("TrainingId")]
        public virtual Training Training { get; set; }

        public virtual ICollection<ExamTopic> ExamTopics { get; set; } = null;
        public virtual ICollection<UserExam> UserExams { get; set; } = null;
    }
}
