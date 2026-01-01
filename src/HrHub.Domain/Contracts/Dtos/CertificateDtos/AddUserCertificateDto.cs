using System;

namespace HrHub.Domain.Contracts.Dtos.CertificateDtos
{
    public class AddUserCertificateDto
    {
        public Guid CertificateId { get; set; } // Client gönderirse kullanacağız, göndermezse üreteceğiz.

        public long TrainingId { get; set; } // Hangi eğitim için?
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
    }
}