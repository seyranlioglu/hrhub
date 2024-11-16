using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class TrainingStatus : TypeCardEntity<long>
    {
        public string? LangCode { get; set; }
    }
}
