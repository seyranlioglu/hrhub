using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class Exam : CardEntity<int>
    {
        public Exam()
        {
              ExamTopics = new HashSet<ExamTopic>();
        }
        public long TrainingId { get; set; }
        public long ExamTime { get; set; }
        public long SuccesRate { get; set; }
        public long ViewQuestionCount { get; set; }

        public virtual ICollection<ExamTopic> ExamTopics { get; set; }
    }
}
