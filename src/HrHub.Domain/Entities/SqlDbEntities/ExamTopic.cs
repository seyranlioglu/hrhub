using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class ExamTopic : CardEntity<int>
    {
        public ExamTopic()
        {
            ExamQuestions = new HashSet<ExamQuestion>();
        }
        [ForeignKey("ExamId")]
        public int ExamId { get; set; }
        public string ImgPath { get; set; }

        public virtual ICollection<ExamQuestion> ExamQuestions { get; set; }
        public virtual Exam Exam { get; set; }
    }
}
