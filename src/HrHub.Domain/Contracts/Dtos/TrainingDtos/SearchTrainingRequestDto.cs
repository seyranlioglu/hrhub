namespace HrHub.Domain.Contracts.Dtos.TrainingDtos
{
    public class SearchTrainingRequestDto
    {
        // Arama
        public string? SearchText { get; set; }

        // Sayfalama
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 12;

        // Filtreler (Çoklu Seçim)
        public List<long>? CategoryIds { get; set; }
        public List<long>? LevelIds { get; set; }
        public List<long>? LanguageIds { get; set; }
        public List<long>? InstructorIds { get; set; }

        // Aralık Filtreleri
        public double? MinRating { get; set; } // 4.0+ puan
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        // Özel Filtreler
        public bool OnlyPrivate { get; set; } = false; // Sadece "Şirketime Özel" olanları getir
        public string? SortBy { get; set; }
    }
}