namespace HrHub.Domain.Contracts.Dtos.TrainingDtos
{
    public class GetPendingApprovalTrainingDto
    {
        public long Id { get; set; } // Training Id
        public string Title { get; set; } // Başlık
        public string Description { get; set; } // Kısa açıklama
        public DateTime CreatedDate { get; set; } // Ne zaman oluşturuldu?

        // Eğitmen Bilgileri
        public long InstructorId { get; set; }
        public string InstructorFullName { get; set; }
        public string InstructorEmail { get; set; } // İletişim için
        public string InstructorRegistrationNumber { get; set; } // User.IdentityNumber veya özel bir alan

        // Kategori
        public string CategoryName { get; set; }
    }
}