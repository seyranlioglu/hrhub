using System;
using System.Collections.Generic;

namespace HrHub.Abstraction.Contracts.Dtos.TrainingDtos
{
    public class TrainingPublicDetailDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string HeaderImage { get; set; }
        public string? PreviewVideoPath { get; set; } // Veritabanındaki 'Trailer' alanı
        public string Language { get; set; }
        public string CategoryName { get; set; }
        public string LevelName { get; set; }

        // İstatistikler
        public double Rating { get; set; }
        public int ReviewCount { get; set; }
        public int StudentCount { get; set; }
        public DateTime LastUpdateDate { get; set; }

        // Fiyat Bilgisi
        public decimal Amount { get; set; }
        public decimal CurrentAmount { get; set; }
        public decimal DiscountRate { get; set; }
        public long? PriceTierId { get; set; }

        // Eğitmen Bilgileri
        public long InstructorId { get; set; }
        public string InstructorName { get; set; }
        public string InstructorTitle { get; set; }
        public string InstructorImage { get; set; }
        public string InstructorBio { get; set; }
        public double InstructorRating { get; set; }
        public int InstructorTotalStudents { get; set; }
        public int InstructorTotalCourses { get; set; }

        // Müfredat Listesi
        public List<PublicSectionDto> Sections { get; set; }

        // Neler Öğreneceksiniz Listesi
        public List<string> WhatYouWillLearn { get; set; }

        // Öne Çıkan Yorumlar
        public List<PublicReviewDto> TopReviews { get; set; }
    }

    public class PublicSectionDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public int OrderId { get; set; }
        public List<PublicContentDto> Contents { get; set; }
    }

    public class PublicContentDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public bool IsPreview { get; set; } // Senin eklediğin yeni alan
        public int DurationMinutes { get; set; }
        public string Type { get; set; } // Video, Document, Quiz etc.
    }

    public class PublicReviewDto
    {
        public string UserName { get; set; }
        public string UserImage { get; set; }
        public double Rating { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }
    }
}