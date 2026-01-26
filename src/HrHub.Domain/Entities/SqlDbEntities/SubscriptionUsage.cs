using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    [Table("SubscriptionUsages")]
    public class SubscriptionUsage : CardEntity<long>
    {
        public long CompanySubscriptionId { get; set; }
        public long TrainingId { get; set; }
        public DateTime UsageDate { get; set; }
        public long ProcessedByUserId { get; set; } // İşlemi yapan admin/yetkili

        // Navigation Properties
        [ForeignKey("CompanySubscriptionId")]
        public virtual CompanySubscription CompanySubscription { get; set; }

        [ForeignKey("TrainingId")]
        public virtual Training Training { get; set; }
    }
}