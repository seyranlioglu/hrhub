using System.Collections.Generic;

namespace HrHub.Domain.Contracts.Dtos.PricingDtos
{
    // Listeleme ve Detay Görüntüleme için (GET)
    public class PriceTierDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public bool IsActive { get; set; }

        // İlişkili detayları (Matris satırlarını) da dönüyoruz
        public List<PriceTierDetailDto> Details { get; set; } = new();
    }

    // PriceTierDto'nun içinde kullanılan detay satırı
    public class PriceTierDetailDto
    {
        public long Id { get; set; }
        public int MinLicenceCount { get; set; }
        public int MaxLicenceCount { get; set; }
        public decimal Amount { get; set; }
        public decimal DiscountRate { get; set; }
    }

    // Yeni Tier Oluşturma (POST)
    public class CreatePriceTierDto
    {
        public string Title { get; set; }
        public string Code { get; set; } // Örn: TIER_1
        public string Description { get; set; }
        public string Currency { get; set; } = "TRY";
    }

    // Tier Güncelleme (PUT)
    public class UpdatePriceTierDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }

    public class TierDetailDto
    {
        public long Id { get; set; } // 0 ise Ekleme, >0 ise Güncelleme
        public long PriceTierId { get; set; }

        public int MinLicenceCount { get; set; }
        public int MaxLicenceCount { get; set; }

        public decimal Amount { get; set; }       // Sabit Fiyat
        public decimal DiscountRate { get; set; } // Veya İndirim Oranı
    }

    public class SubscriptionPlanDto
    {
        public long Id { get; set; }
        public string Title { get; set; }       // Paket Adı (Gold, Silver)
        public string Description { get; set; } // Kısa açıklama

        public decimal Price { get; set; }
        public string Currency { get; set; }

        public string UserCountRange { get; set; } // "1-10 Kullanıcı" (Formatted string)
        public int MonthlyCredit { get; set; }
        public string DurationText { get; set; }   // "12 Ay" (Formatted string)

        // JSON string değil, liste olarak dönüyoruz
        public List<string> FeaturesList { get; set; } = new();
    }

    // Yeni Plan Oluşturma (POST)
    public class CreateSubscriptionPlanDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;

        public int MinUserCount { get; set; }
        public int MaxUserCount { get; set; }

        public decimal Price { get; set; }
        public string Currency { get; set; } = "TRY";

        public int TotalMonthlyCredit { get; set; }
        public int DurationMonths { get; set; } = 12;

        // Özellikleri liste olarak alıyoruz (Manager'da JSON'a çevrilecek)
        public List<string> FeaturesList { get; set; } = new();
    }

    // NOT: Manager'da UpdateSubscriptionPlanAsync kullanmıştık,
    // listende yoktu ama lazım olacağı için buraya ekliyorum:
    public class UpdateSubscriptionPlanDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int TotalMonthlyCredit { get; set; }
        public List<string> FeaturesList { get; set; }
    }
}