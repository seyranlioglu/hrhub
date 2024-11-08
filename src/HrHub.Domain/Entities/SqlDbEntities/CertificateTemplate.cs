using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class CertificateTemplate : TypeCardEntity<int>
    {
        public CertificateTemplate()
        {
            UserCertificates = new HashSet<UserCertificate>();
        }
        
        public long CertificateTypeId { get; set; }
        public string? TemplatePath { get; set; }
        public string? VerificationURL { get; set; }


        [ForeignKey("CertificateTypeId")]
        public virtual CertificateType CertificateType { get; set; } = null;
        public virtual ICollection<UserCertificate> UserCertificates { get; set; }

    }
}
