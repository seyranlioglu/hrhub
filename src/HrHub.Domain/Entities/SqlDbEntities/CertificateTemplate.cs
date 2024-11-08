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
        [ForeignKey("CertificateTypeId")]
        public int CertificateTypeId { get; set; }
        public string TemplatePath { get; set; }
        public string VerificationURL { get; set; }


        public virtual CertificateType CertificateType { get; set; }
        public virtual ICollection<UserCertificate> UserCertificates { get; set; }

    }
}
