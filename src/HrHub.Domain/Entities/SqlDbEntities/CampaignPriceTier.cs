using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    [Table("CampaignPriceTiers")]
    public class CampaignPriceTier : CardEntity<long>
    {
        public long CampaignId { get; set; }
        public long PriceTierId { get; set; }

        // Navigation Properties
        [ForeignKey("CampaignId")]
        public virtual Campaign Campaign { get; set; }

        [ForeignKey("PriceTierId")]
        public virtual PriceTier PriceTier { get; set; }
    }
}