using HrHub.Core.Domain.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    [Table("PriceTiers")]
    public class PriceTier : TypeCardEntity<long>
    {
        public PriceTier()
        {
            Details = new HashSet<PriceTierDetail>();
            Trainings = new HashSet<Training>();
            CampaignPriceTiers = new HashSet<CampaignPriceTier>(); // <--- BURASI ŞART
        }

        // Amount YOK! Fiyatlar Details tablosunda.
        public string Currency { get; set; } = "TRY";

        // Navigation Properties
        public virtual ICollection<PriceTierDetail> Details { get; set; }
        public virtual ICollection<Training> Trainings { get; set; }

        // Kampanya İlişkisi
        public virtual ICollection<CampaignPriceTier> CampaignPriceTiers { get; set; }
    }
}