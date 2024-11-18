using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class ExamTopic : CardEntity<long>
    {
        public ExamTopic()
        {
            ExamQuestions = new HashSet<ExamQuestion>();
        }
        public long ExamVersionId { get; set; }
        public string Title { get; set; }
        public string ImgPath { get; set; }

        [ForeignKey("ExamVersionId")]
        public virtual ExamVersion ExamVersion { get; set; }
        public virtual ICollection<ExamQuestion> ExamQuestions { get; set; }
    }
}
