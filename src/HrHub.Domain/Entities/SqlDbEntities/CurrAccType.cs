using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class CurrAccType : TypeCardEntity<long>
    {
        public CurrAccType()
        {
            CurrAccs = new HashSet<CurrAcc>();

        }
        public string? LangCode { get; set; }
        public bool  EnterpriseAcc { get; set; }
        public virtual ICollection<CurrAcc> CurrAccs { get; set; } = null;
    }
}
