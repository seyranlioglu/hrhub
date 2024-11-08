using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class ExamAnswer : TypeCardEntity<int>
    {
        [ForeignKey("QuestionId")]
        public int QuestionId { get; set; }
        public bool IsCorrect { get; set; }


        public virtual ExamQuestion ExamQuestion { get; set; }
    }
}
