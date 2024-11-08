using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class ContentType : TypeCardEntity<long>
    {
        public string LangCode { get; set; }
    }
}
