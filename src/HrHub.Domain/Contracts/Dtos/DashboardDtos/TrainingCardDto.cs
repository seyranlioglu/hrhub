using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Dtos.DashboardDtos
{
    public class TrainingCardDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string InstructorName { get; set; } // Eğitmen Adı
        public double? Rating { get; set; } // Puan (Örn: 4.8)
        public int TotalDurationMinutes { get; set; } // Toplam Süre
        public int LessonCount { get; set; } // Ders Sayısı

        // Frontend'de etiketi kırmızı göstermek için
        public bool IsAssigned { get; set; } // Zorunlu mu?
        public DateTime? DueDate { get; set; } // Son tarih
        public int Progress { get; set; } // 0 ile 100 arası değer
    }
}
