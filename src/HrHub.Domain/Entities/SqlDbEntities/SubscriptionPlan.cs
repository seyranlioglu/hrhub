using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    [Table("SubscriptionPlans")]
    public class SubscriptionPlan : TypeCardEntity<long>
    {
        // CardEntity'den Title (Name), Description, IsActive geliyor.
        // Title -> Paket Adı (Örn: "Gold Paket")

        public int MinUserCount { get; set; } = 1;
        public int MaxUserCount { get; set; }

        public decimal Price { get; set; }
        public string Currency { get; set; } = "TRY";

        public int TotalMonthlyCredit { get; set; } // Aylık Eğitim Hakkı
        public int DurationMonths { get; set; } = 12; // Süre
        public string? Features { get; set; }
    }
}