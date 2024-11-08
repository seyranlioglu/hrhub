using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class UserCertificate : CardEntity<int>
    {
        [ForeignKey("CurrAccTrainingUsersId")]
        public int CurrAccTrainingUsersId { get; set; }

        [ForeignKey("CertificateTemplateId")]
        public int CertificateTemplateId { get; set; }

        public DateTime? CertificateDate { get; set; }
        public string ConstructorName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string TrainerName { get; set; }
        public string TrainerTitle { get; set; }
        public int NumberOfLecture { get; set; }
        public string ProviderName { get; set; }
        public string ProviderTitle { get; set; }
        public string VerificationURL { get; set; }
        public Guid CertificateId { get; set; }


        public virtual CertificateTemplate CertificateTemplate { get; set; }
        public virtual CurrAccTrainingUser CurrAccTrainingUser { get; set; }
    }
}
