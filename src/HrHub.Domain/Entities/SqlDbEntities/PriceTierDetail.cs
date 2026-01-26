using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    [Table("PriceTierDetails")]
    public class PriceTierDetail : CardEntity<long>
    {
        public long PriceTierId { get; set; }

        // Miktar Aralığı (1-5 kişi, 6-10 kişi)
        public int MinLicenceCount { get; set; } = 1;
        public int MaxLicenceCount { get; set; } = int.MaxValue;

        // Fiyatlandırma Stratejisi
        // Eğer Amount > 0 ise bu fiyatı kullanır (Sabit Fiyat).
        // Eğer Amount = 0 ve DiscountRate > 0 ise, Base fiyattan indirim yapar.
        public decimal Amount { get; set; }
        public decimal DiscountRate { get; set; }

        // Tarihçe (Kampanyalar için)
        // Eğer null ise sonsuza kadar geçerli.
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; } = true;

        [ForeignKey("PriceTierId")]
        public virtual PriceTier PriceTier { get; set; }
    }
}