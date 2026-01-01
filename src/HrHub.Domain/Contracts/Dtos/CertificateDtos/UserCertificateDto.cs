using System;

namespace HrHub.Domain.Contracts.Dtos.CertificateDtos
{
    public class UserCertificateDto
    {
        public Guid CertificateId { get; set; } // Dışarıya dönmesi gereken ID

        // İlişkili tablolardan gelen veriler (Mapper ile doldurulacak)
        public string TemplateTitle { get; set; }
        public string TrainingName { get; set; }

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
        public bool IsActive { get; set; }
    }
}