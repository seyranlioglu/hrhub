using System.Collections.Generic;

namespace HrHub.Domain.Contracts.Dtos.TrainingDtos
{
    // Ana DTO
    public class TrainingDetailDto
    {
        // -- Üst Başlık ve Genel Bilgiler --
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PicturePath { get; set; } // HeaderImage
        public string InstructorName { get; set; }

        // -- İlerleme Durumu --
        public int ProgressPercentage { get; set; }
        public long? LastWatchedContentId { get; set; } // "Devam Et" butonu için

        // -- Genel Bakış Sekmesi Verileri --
        public string LangCode { get; set; }  // Örn: "TR"
        public string LevelName { get; set; } // Örn: "Başlangıç Seviyesi"
        public bool HasCertificate { get; set; } // Sertifika var mı?

        // -- Müfredat Ağacı --
        public List<TrainingSectionForUserDto> Sections { get; set; }
    }

    // Bölüm (Section) DTO
    public class TrainingSectionForUserDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public long OrderId { get; set; }

        public List<TrainingContentListItemDto> Contents { get; set; }
    }

    // İçerik (Video/Döküman) DTO
    public class TrainingContentListItemDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public int Time { get; set; } // Süre (Dakika veya Saniye)
        public long OrderId { get; set; }

        // UI Durumları
        public bool IsActive { get; set; }
        public bool IsCompleted { get; set; } // Yanına yeşil tik koymak için
    }
}