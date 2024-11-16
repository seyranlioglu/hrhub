using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class ContentExamAction : TypeCardEntity<long>
    {
        public ContentExamAction()
        {
                ContentExams = new HashSet<ContentExam>();
        }

        public virtual ICollection<ContentExam> ContentExams { get; set; } = null;
    }
}
