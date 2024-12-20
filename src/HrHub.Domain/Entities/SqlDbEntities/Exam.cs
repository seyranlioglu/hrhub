using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class Exam : CardEntity<long>
    {
        public Exam()
        {
            ExamVersions = new HashSet<ExamVersion>();
            ExamResults = new HashSet<ExamResult>();
            TrainingContents = new HashSet<TrainingContent>();
        }
        public string Title { get; set; }
        [AllowNull]
        public string? Description { get; set; }

        public long TrainingId { get; set; }
        public long ExamStatusId { get; set; }
        public long InstructorId { get; set; }
        public long ActionId { get; set; }


        [ForeignKey("ActionId")]
        public virtual ExamAction ExamAction { get; set; }
        [ForeignKey("InstructorId")]
        public User Instructor { get; set; }
        [ForeignKey("TrainingId")]
        public virtual Training Training { get; set; }
        [ForeignKey("ExamStatusId")]
        public virtual ExamStatus ExamStatus { get; set; }
        public virtual ICollection<ExamVersion> ExamVersions { get; set; } = null;
        public virtual ICollection<ExamResult> ExamResults { get; set; } = null;
        public virtual ICollection<TrainingContent> TrainingContents { get; set; } = null;
    }
}
