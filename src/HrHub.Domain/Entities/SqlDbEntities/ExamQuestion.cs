using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HrHub.Domain.Entities.SqlDbEntities;
public class ExamQuestion : TypeCardEntity<long>
{
    public ExamQuestion()
    {
        QuestionOptions = new HashSet<QuestionOption>();
        UserAnswers = new HashSet<UserAnswer>();
    }
    public long ExamTopicId { get; set; }
    public string QuestionText { get; set; }
    public decimal Score { get; set; }

    [ForeignKey("ExamTopicId")]
    public virtual ExamTopic ExamTopics { get; set; }

    public virtual ICollection<QuestionOption> QuestionOptions { get; set; } = null;
    public virtual ICollection<UserAnswer> UserAnswers { get; set; } = null;
}
