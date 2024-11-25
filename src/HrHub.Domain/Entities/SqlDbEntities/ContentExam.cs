using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class ContentExam : CardEntity<long>
    {
        public long ExamId { get; set; }
        public long TrainingContentId { get; set; }
        public long ContentExamActionId { get; set; }


        [ForeignKey("ExamId")]
        public virtual Exam Exam { get; set; }

        [ForeignKey("TrainingContentId")]
        public virtual TrainingContent TrainingContent { get; set; }

        [ForeignKey("ContentExamActionId")]
        public virtual ContentExamAction ContentExamAction { get; set; }
    }
}
