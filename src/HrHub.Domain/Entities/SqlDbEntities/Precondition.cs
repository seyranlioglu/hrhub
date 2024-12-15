using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class Precondition : TypeCardEntity<long>
    {
        public Precondition()
        {
            Trainings = new HashSet<Training>();
        }

        public virtual ICollection<Training> Trainings { get; set; }
    }
}
