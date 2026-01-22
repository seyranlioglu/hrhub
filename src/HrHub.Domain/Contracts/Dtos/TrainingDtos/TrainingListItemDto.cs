namespace HrHub.Domain.Contracts.Dtos.TrainingDtos
{
    public class TrainingListItemDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; } // Liste görünümünde kısa açıklama gerekebilir
        public string HeaderImage { get; set; }

        // Kategori & Eğitmen
        public string CategoryName { get; set; }
        public string InstructorName { get; set; }
        public string InstructorImage { get; set; }

        // Fiyat
        public decimal Amount { get; set; }        // Orijinal Fiyat (Örn: 1000 TL)
        public decimal CurrentAmount { get; set; } // Satış Fiyatı (Örn: 800 TL)
        public decimal DiscountRate { get; set; }  // İndirim Oranı (Örn: %20)

        // Meta Veriler
        public string LevelName { get; set; } // Başlangıç, İleri vs.
        public double Rating { get; set; }
        public int ReviewCount { get; set; }
        public int TotalMinutes { get; set; } // Toplam süre
        public int LessonCount { get; set; }

        public DateTime CreatedDate { get; set; }

        // --- UI ROZETLERİ İÇİN ---
        public bool IsPrivate { get; set; } // True ise UI'da "Kuruma Özel" 🔒 ikonu çıkar
        public bool IsActive { get; set; }
    }
}