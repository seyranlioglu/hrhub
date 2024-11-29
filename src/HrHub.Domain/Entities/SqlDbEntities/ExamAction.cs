using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class ExamAction : TypeCardEntity<long>
    {
        public ExamAction()
        {
                Exams = new HashSet<Exam>();
        }

        public virtual ICollection<Exam> Exams { get; set; } = null;
    }
}
