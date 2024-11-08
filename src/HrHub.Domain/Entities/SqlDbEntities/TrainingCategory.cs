using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class TrainingCategory : TypeCardEntity<long>
    {
        public long MasterCategoryId { get; set; }
    }
}
