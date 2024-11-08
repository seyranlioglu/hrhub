using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class TimeUnit : TypeCardEntity<int> {
        public string LangCode { get; set; }
    }
}
