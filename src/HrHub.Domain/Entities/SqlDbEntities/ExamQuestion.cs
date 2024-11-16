using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HrHub.Domain.Entities.SqlDbEntities;
public class ExamQuestion : TypeCardEntity<long>
{
    public ExamQuestion()
    {
        ExamAnswers = new HashSet<ExamAnswer>();
        UserAnswers = new HashSet<UserAnswer>();
    }
    public long ExamTopicId { get; set; }
    public long Question { get; set; }

    [ForeignKey("ExamTopicId")]
    public virtual ExamTopic ExamTopics { get; set; }

    public virtual ICollection<ExamAnswer> ExamAnswers { get; set; }
    public virtual ICollection<UserAnswer> UserAnswers { get; set; }
}
