using HrHub.Core.Domain.Entity;
using HrHub.Domain.Contracts.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    [Table("CampaignRules")]
    public class CampaignRule : CardEntity<long>
    {
        public long CampaignId { get; set; }

        public RuleType RuleType { get; set; }

        // İlişkili ID (CategoryId, PriceTierId, SubscriptionPlanId vb.)
        public string RelatedId { get; set; }

        [ForeignKey("CampaignId")]
        public virtual Campaign Campaign { get; set; }
    }
}
