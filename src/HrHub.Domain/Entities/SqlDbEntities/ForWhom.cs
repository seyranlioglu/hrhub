using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class ForWhom : TypeCardEntity<long>
    {
        public ForWhom()
        {
            Trainings = new HashSet<Training>();
        }

        public virtual ICollection<Training> Trainings { get; set; } = null;
    }
}
