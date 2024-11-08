using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class CartItem : CardEntity<long>
    {
        public long CartId { get; set; }
        public long TrainingId { get; set; }
        public decimal Amount { get; set; }

        public decimal DiscountRate { get; set; }
        public decimal TaxRate { get; set; }
        public decimal CurrentAmount { get; set; }
        public int LicenceCount { get; set; }

        [ForeignKey("TrainingId")]
        public virtual Training Training { get; set; }

        [ForeignKey("CartId")]
        public virtual Cart Cart { get; set; }
    }
}
