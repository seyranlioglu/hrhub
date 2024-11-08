using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class UserAnswer : CardEntity<int>
    {
        [ForeignKey("UserExamId")]
        public int UserExamId { get; set; }
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
        public DateTime? AnswerDate { get; set; }
        public DateTime? SuccessRate { get; set; }

        public virtual UserExam UserExam { get; set; }
    }
}
