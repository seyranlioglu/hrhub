using HrHub.Core.Domain.Entity;
using HrHub.Domain.Contracts.Enums;
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
        public string? GeneratedFilePath { get; set; }
        /// <summary>
        /// Sertifikanın anlık durumu (Hazırlanıyor, Tamamlandı, Hata)
        /// </summary>
        public CertificateStatus Status { get; set; } = CertificateStatus.Preparing; // Default değer

        /// <summary>
        /// Eğer Status = Failed ise, hatanın ne olduğunu buraya yazarız.
        /// </summary>
        public string? ErrorMessage { get; set; }




        [ForeignKey("CertificateTemplateId")]
        public virtual CertificateTemplate CertificateTemplate { get; set; }

        [ForeignKey("CurrAccTrainingUsersId")]
        public virtual CurrAccTrainingUser CurrAccTrainingUser { get; set; }
    }
}
