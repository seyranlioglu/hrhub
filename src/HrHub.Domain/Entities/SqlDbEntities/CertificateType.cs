using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class CertificateType : TypeCardEntity<int>
    {
        public CertificateType()
        {
            CertificateTemplates = new HashSet<CertificateTemplate>();
        }
        public string? LangCode { get; set; }


        public virtual ICollection<CertificateTemplate> CertificateTemplates { get; set; }
    }
}
