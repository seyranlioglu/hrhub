using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class ContentType : TypeCardEntity<int>
    {
        public string LangCode { get; set; }
    }
}
