
namespace HrHub.Domain.Contracts.Dtos.TrainingDtos
{
    public class GetMyTrainingDto
    {
        // --- Eğitim Künyesi ---
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PicturePath { get; set; } // HeaderImage

        // --- Kategorizasyon ve Eğitmen ---
        public string CategoryName { get; set; } // Örn: "Yazılım Geliştirme"
        public string InstructorName { get; set; } // Örn: "Ahmet Yılmaz"
        public string? InstructorTitle { get; set; } // Örn: "Senior .NET Developer" (Instructor tablosunda varsa)

        // --- İlerleme Durumu (Udemy Style) ---
        public int TotalContentCount { get; set; }
        public int CompletedContentCount { get; set; }
        public int ProgressPercentage { get; set; }
        public bool IsCompleted { get; set; }

        // --- Kullanıcı Aksiyonu ---
        public long? LastWatchedContentId { get; set; } // "Devam Et" butonu için
        public DateTime? LastAccessDate { get; set; }   // "En son 2 gün önce izledin"
        public DateTime? AssignDate { get; set; }       // "Kütüphaneye eklenme tarihi"

        // Opsiyonel: Bitiş tarihi varsa (Kurumsal atamalarda olur)
        public DateTime? DueDate { get; set; }
    }
}