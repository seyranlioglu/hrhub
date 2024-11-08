using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class TrainingCategory : TypeCardEntity<int>
    {
        public int MasterCategoryId { get; set; }
    }
}
