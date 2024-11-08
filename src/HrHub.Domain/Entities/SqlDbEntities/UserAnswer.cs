using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class UserAnswer : CardEntity<int>
    {
        [ForeignKey("UserExamId")]
        public long UserExamId { get; set; }
        public long QuestionId { get; set; }
        public long AnswerId { get; set; }
        public DateTime? AnswerDate { get; set; }
        public DateTime? SuccessRate { get; set; }

        public virtual UserExam UserExam { get; set; }
    }
}
