using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class TrainingAmount : CardEntity<long> {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long TrainingId { get; set; }
        public int MinLicenceCount { get; set; }
        public int MaxLicenceCount { get; set; }
        public decimal AmountPerLicence { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal TaxRate { get; set; }

        [ForeignKey("TrainingId")]
        public virtual Training Training { get; set; }
    }
}
