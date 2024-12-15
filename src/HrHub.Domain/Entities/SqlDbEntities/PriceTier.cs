using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class PriceTier : TypeCardEntity<long>
    {
        public PriceTier()
        {
                Trainings = new HashSet<Training>();
        }
        public virtual ICollection<Training> Trainings { get; set; }
    }
}
