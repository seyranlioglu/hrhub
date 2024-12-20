using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class UserAnswer : CardEntity<long>
    {
        public long UserExamId { get; set; }
        public long QuestionId { get; set; }
        [AllowNull]
        public long? SelectedOptionId { get; set; }
        public DateTime? AnswerDate { get; set; }
        public int SeqNumber { get; set; }

        [ForeignKey("UserExamId")]
        public virtual UserExam UserExam { get; set; }

        [ForeignKey("QuestionId")]
        public virtual ExamQuestion Question { get; set; }

        [ForeignKey("SelectedOptionId")]
        public virtual QuestionOption ExamOption { get; set; }
    }
}
