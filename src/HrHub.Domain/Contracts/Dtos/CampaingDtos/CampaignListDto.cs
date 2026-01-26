using HrHub.Domain.Contracts.Enums;
using HrHub.Domain.Entities.SqlDbEntities; // Enum'lar için (Veya Enumları Common'a taşıdıysan orayı ekle)
using System;
using System.Collections.Generic;

namespace HrHub.Domain.Contracts.Dtos.CampaignDtos
{
    // Admin Panelinde Listeleme İçin (GET)
    public class CampaignListDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }

        // Enumları frontend'e int veya string olarak dönebilirsin.
        // Burada int olarak bırakıyorum, frontend map'leyebilir.
        public CampaignType Type { get; set; }
        public CampaignScope Scope { get; set; }
        public TargetAudience Target { get; set; }

        public decimal Value { get; set; } // İndirim Oranı veya Tutarı
    }

    // Eğitmen Panelinde "Katılabileceğim Kampanyalar" Listesi İçin (GET)
    public class CampaignOpportunityDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public decimal DiscountValue { get; set; } // Örn: %20

        public bool IsJoined { get; set; } // Eğitmen buna zaten katılmış mı?
    }

    public class CreateCampaignDto
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public CampaignType Type { get; set; } // 1:Yüzde, 2:Tutar, 3:BOGO
        public CampaignScope Scope { get; set; } // 1:Global, 2:OptIn
        public TargetAudience Target { get; set; } // 1:Perakende, 2:Abonelik

        public decimal Value { get; set; } // Örn: 20 (%20 için)

        // Opsiyonel Koşullar
        public decimal MinConditionAmount { get; set; } = 0;
        public int MinConditionQuantity { get; set; } = 0;

        // Hangi Tier'lar için geçerli? (CampaignPriceTier tablosunu doldurmak için)
        // Eğer boş gelirse "Tüm Tier'lar" olarak kabul edilebilir veya validation ile zorunlu tutulabilir.
        public List<long> TargetPriceTierIds { get; set; } = new();
    }

    // Eğitmenin Kampanyaya Katılması (POST - Instructor)
    public class JoinCampaignDto
    {
        public long CampaignId { get; set; }

        // Eğer null gönderilirse, eğitmenin "Tüm Eğitimleri" dahil olur.
        // Dolu gönderilirse sadece spesifik bir eğitimi için katılır.
        public long? TrainingId { get; set; }
    }
}