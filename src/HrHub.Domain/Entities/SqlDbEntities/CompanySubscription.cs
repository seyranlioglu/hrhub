using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    [Table("CompanySubscriptions")]
    public class CompanySubscription : CardEntity<long>
    {
        public long CurrAccId { get; set; }
        public long SubscriptionPlanId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int TotalCredit { get; set; }
        public int UsedCredit { get; set; }

        public decimal PricePaid { get; set; }
        public string Currency { get; set; }

        // Navigation Properties
        [ForeignKey("CurrAccId")]
        public virtual CurrAcc CurrAcc { get; set; }

        [ForeignKey("SubscriptionPlanId")]
        public virtual SubscriptionPlan SubscriptionPlan { get; set; }

        public virtual ICollection<SubscriptionUsage> SubscriptionUsages { get; set; }
    }
}