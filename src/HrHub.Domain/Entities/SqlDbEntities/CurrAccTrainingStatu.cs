using HrHub.Core.Domain.Entity;
using MongoDB.Driver.Search;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class CurrAccTrainingStatu : TypeCardEntity<long>
    {
        public CurrAccTrainingStatu()
        {
            CurrAccTrainings = new HashSet<CurrAccTraining>();
        }

        public virtual ICollection<CurrAccTraining> CurrAccTrainings { get; set; } = null;
    }
}
