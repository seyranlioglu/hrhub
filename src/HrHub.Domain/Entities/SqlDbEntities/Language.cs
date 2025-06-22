using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class Language : TypeCardEntity<long>
    {
        public string? LangCode { get; set; }
    }

}
