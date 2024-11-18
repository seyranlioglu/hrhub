using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class UserAnswer : CardEntity<long>
    {
        public long UserExamId { get; set; }
        public long QuestionId { get; set; }
        public long OptionId { get; set; }
        public DateTime? AnswerDate { get; set; }
        public DateTime? SuccessRate { get; set; }

        [ForeignKey("UserExamId")]
        public virtual UserExam UserExam { get; set; }

        [ForeignKey("QuestionId")]
        public virtual ExamQuestion Question { get; set; }

        [ForeignKey("OptionId")]
        public virtual ExamOptions ExamOption { get; set; }
    }
}
