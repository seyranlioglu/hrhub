using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Dtos.DashboardDtos
{
    public class TrainingViewCardDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string HeaderImage { get; set; } // veya CourseImage

        // Kategori Bilgisi (Sadece adı yeterli)
        public string CategoryTitle { get; set; }

        // Eğitmen Bilgisi (Sadece adı yeterli)
        public string InstructorTitle { get; set; }
        public string InstructorPicturePath { get; set; }

        // Fiyat ve Puanlama (Kart üzerinde görünmesi muhtemel)
        public decimal? Amount { get; set; }
        public decimal? CurrentAmount { get; set; }
        public decimal? DiscountRate { get; set; }

        // UI için Meta Veriler
        public string TrainingLevelTitle { get; set; } // Başlangıç, İleri vs.
        public int TotalDurationMinutes { get; set; } // Toplam süre (Hesaplanmış)
        public double Rating { get; set; } // Ortalama puan
        public int ReviewCount { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
