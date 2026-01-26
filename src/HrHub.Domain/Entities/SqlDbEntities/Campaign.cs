using HrHub.Core.Domain.Entity;
using HrHub.Domain.Contracts.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{

    [Table("Campaigns")]
    public class Campaign : TypeCardEntity<long>
    {
        public Campaign()
        {
            CampaignParticipants = new HashSet<CampaignParticipant>();
            CampaignRules = new HashSet<CampaignRule>();
            CampaignPriceTiers = new HashSet<CampaignPriceTier>(); // <--- BURASI EKLENDİ
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public CampaignType Type { get; set; }
        public CampaignScope Scope { get; set; }
        public TargetAudience Target { get; set; }

        // Değerler (Örn: %20 veya 50 TL)
        public decimal Value { get; set; }

        // Koşullar (Min sepet tutarı vb.)
        public decimal MinConditionAmount { get; set; } = 0;
        public int MinConditionQuantity { get; set; } = 0;

        // Navigation Properties
        public virtual ICollection<CampaignParticipant> CampaignParticipants { get; set; }
        public virtual ICollection<CampaignRule> CampaignRules { get; set; }

        // EKSİK OLAN BAĞLANTI LİSTESİ
        public virtual ICollection<CampaignPriceTier> CampaignPriceTiers { get; set; }
    }
}