using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class TimeUnit : TypeCardEntity<long> {
        public string LangCode { get; set; }
    }
}
