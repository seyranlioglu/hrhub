using System;
using System.Collections.Generic;

namespace HrHub.Domain.Contracts.Dtos.TrainingContentDtos
{
    public class GetContentForPlayerDto
    {
        // --- 1. İÇERİK TEMEL BİLGİLERİ (Sadeleştirilmiş) ---
        public long Id { get; set; }
        public string Title { get; set; } // Content Title
        public string Description { get; set; }
        public int OrderId { get; set; } // Sıralama önemli

        // Frontend'de süre ve tip gösterimi için
        public int DurationMinutes { get; set; } // Time yerine int (dk) daha kolay
        public string ContentType { get; set; } // "Video", "Exam", "Pdf" vb.

        // --- 2. DOSYA / PLAYER BİLGİLERİ ---
        // Sadece oynatıcı için gerekenler
        public string FilePath { get; set; }
        public string ThumbnailPath { get; set; } // Video kapağı varsa
        public string FileName { get; set; }      // İndirme yapacaksa dosya adı
        public double? FileSize { get; set; }     // Döküman boyutu (MB)

        // --- 3. ÖĞRENCİ İLERLEME DURUMU ---
        public bool IsCompleted { get; set; }
        public int LastWatchedPart { get; set; } // Videonun neresinde kaldı (sn)

        // --- 4. ERİŞİM KONTROLÜ (Mandatory Logic) ---
        public bool CanView { get; set; }
        public string BlockMessage { get; set; }
        public List<MissingContentItemDto> MissingContents { get; set; }

        // --- 5. SINAV YÖNETİMİ (Exam Logic) ---
        public bool IsExam { get; set; }
        public string ExamStatus { get; set; } // "NotStarted", "Continue", "Passed", "Failed"
        public string ExamActionDescription { get; set; } // "Kalırsan başa döner" uyarısı

        // Teknik ID'ler (Butonlar için)
        public long? ExamId { get; set; }
        public long? UserExamId { get; set; }

        // Sonuç Ekranı
        public double? UserScore { get; set; }
        public double? PassingScore { get; set; }
        public int AttemptCount { get; set; }

        // Navigasyon / Eğitim Durumu
        public long? NextContentId { get; set; }
        public long? PreviousContentId { get; set; }

        // Eğitim Bitiş & Sertifika (Fail-Safe ve NextContent için)
        public bool IsTrainingFinished { get; set; }
        public string Message { get; set; }

    }

    public class MissingContentItemDto
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public string Title { get; set; }
    }
}