using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class ExamOptions : TypeCardEntity<long>
    {
        public ExamOptions()
        {
            UserAnswers = new HashSet<UserAnswer>();
        }
        public long QuestionId { get; set; }
        public string OptionText { get; set; }
        public bool IsCorrect { get; set; }


        [ForeignKey("QuestionId")]
        public virtual ExamQuestion ExamQuestion { get; set; }


        public virtual ICollection<UserAnswer> UserAnswers { get; set; } = null;
    }
}
