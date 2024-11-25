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
            ContentExams = new HashSet<ContentExam>();
        }
        public string Title { get; set; }
        public string Description { get; set; }
        public long TrainingId { get; set; }
        public long ExamStatusId { get; set; }
        public long InstructorId { get; set; }

        [ForeignKey("InstructorId")]
        public User Instructor { get; set; }
        [ForeignKey("TrainingId")]
        public virtual Training Training { get; set; }
        [ForeignKey("ExamStatusId")]
        public virtual ExamStatus ExamStatus { get; set; }
        public virtual ICollection<ExamVersion> ExamVersions { get; set; } = null;
        public virtual ICollection<ExamResult> ExamResults { get; set; } = null;
        public virtual ICollection<ContentExam> ContentExams { get; set; } = null;
    }
}
