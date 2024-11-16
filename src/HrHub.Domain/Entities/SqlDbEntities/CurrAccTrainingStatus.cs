using HrHub.Core.Domain.Entity;
using MongoDB.Driver.Search;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class CurrAccTrainingStatus : TypeCardEntity<long>
    {
        public CurrAccTrainingStatus()
        {
            CurrAccTrainings = new HashSet<CurrAccTraining>();
        }

        public virtual ICollection<CurrAccTraining> CurrAccTrainings { get; set; } = null;
    }
}
