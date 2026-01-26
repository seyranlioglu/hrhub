using System;
using System.Collections.Generic;

namespace HrHub.Domain.Contracts.Dtos.DashboardDtos
{
    public class TrainingCardDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        // Frontend ve Manager "HeaderImage" bekliyor
        public string HeaderImage { get; set; }

        // --- KATEGORİ ---
        public long CategoryId { get; set; }
        public long ParentCategoryId { get; set; }
        public string CategoryTitle { get; set; }

        // --- EĞİTMEN ---
        public string InstructorName { get; set; }
        public string InstructorPicturePath { get; set; } // EKLENDİ (Resim için)

        // --- META VERİLER ---
        public double? Rating { get; set; }
        public int ReviewCount { get; set; } // EKLENDİ (Yorum sayısı için)
        public int TotalDurationMinutes { get; set; }
        public int LessonCount { get; set; }
        public string TrainingLevelTitle { get; set; } // EKLENDİ (Başlangıç/İleri vs.)

        // --- FİYAT ---
        public decimal? Amount { get; set; } // EKLENDİ (Orijinal Fiyat)
        public decimal? CurrentAmount { get; set; } // EKLENDİ (Satış Fiyatı)
        public decimal? DiscountRate { get; set; } // EKLENDİ (İndirim)

        // --- ROZETLER & KULLANICI DURUMU ---
        public bool IsAssigned { get; set; } // Zorunlu/Satın Alınmış mı?
        public bool IsFavorite { get; set; } // EKLENDİ (İstek listesi kalp ikonu)
        public bool IsBestseller { get; set; } // EKLENDİ (Çok Satan rozeti)
        public bool IsNew { get; set; } // EKLENDİ (Yeni rozeti)

        // --- DASHBOARD ÖZEL ---
        public DateTime? DueDate { get; set; }
        public int Progress { get; set; }
        public DateTime CreatedDate { get; set; } // EKLENDİ (Sıralama ve 'Yeni' kontrolü için)

        // --- HOVER POPOVER ---
        // Mouse üzerine gelince çıkan maddeler
        public List<string> WhatYouWillLearn { get; set; } = new List<string>();

        public string ImageUrl { get; set; }
    }
}