using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class User : CardEntity<int>
    {
        public long CurrAccId { get; set; }
    }
}
