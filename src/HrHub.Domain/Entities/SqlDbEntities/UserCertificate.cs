using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class UserCertificate : CardEntity<long>
    {
        public long CurrAccTrainingUsersId { get; set; }

        public long CertificateTemplateId { get; set; }

        public DateTime? CertificateDate { get; set; }
        public string ConstructorName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string TrainerName { get; set; }
        public string TrainerTitle { get; set; }
        public long NumberOfLecture { get; set; }
        public string ProviderName { get; set; }
        public string ProviderTitle { get; set; }
        public string VerificationURL { get; set; }
        public Guid CertificateId { get; set; }


        [ForeignKey("CertificateTemplateId")]
        public virtual CertificateTemplate CertificateTemplate { get; set; }

        [ForeignKey("CurrAccTrainingUsersId")]
        public virtual CurrAccTrainingUser CurrAccTrainingUser { get; set; }
    }
}
