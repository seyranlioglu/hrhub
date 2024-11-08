using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class Exam : CardEntity<int>
    {
        public Exam()
        {
              ExamTopics = new HashSet<ExamTopic>();
        }
        public int TrainingId { get; set; }
        public int ExamTime { get; set; }
        public int SuccesRate { get; set; }
        public int ViewQuestionCount { get; set; }

        public virtual ICollection<ExamTopic> ExamTopics { get; set; }
    }
}
