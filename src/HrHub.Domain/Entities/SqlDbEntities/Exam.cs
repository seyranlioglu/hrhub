using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class Exam : CardEntity<long>
    {
        public Exam()
        {
            ExamVersions = new HashSet<ExamVersion>();
            ExamResults = new HashSet<ExamResult>();
        }
        public string Title { get; set; }
        public string Description { get; set; }
        public long TrainingId { get; set; }
        public TimeSpan ExamTime { get; set; }
        public long SuccesRate { get; set; }
        public decimal PassingScore { get; set; }
        public long ViewQuestionCount { get; set; }
        public long ExamStatusId { get; set; }

        [ForeignKey("TrainingId")]
        public virtual Training Training { get; set; }

        public virtual ICollection<ExamVersion> ExamVersions { get; set; } = null;
        public virtual ICollection<ExamResult> ExamResults { get; set; } = null;
    }
}
