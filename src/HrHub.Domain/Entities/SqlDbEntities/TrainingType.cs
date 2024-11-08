using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class TrainingType : TypeCardEntity<int>
    {
        public string LangCode { get; set; }
    }
}
