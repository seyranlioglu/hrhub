using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class ExamAnswer : TypeCardEntity<long>
    {
        public long QuestionId { get; set; }
        public bool IsCorrect { get; set; }


        [ForeignKey("QuestionId")]
        public virtual ExamQuestion ExamQuestion { get; set; }
    }
}
