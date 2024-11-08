using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class Training : TypeCardEntity<int>
    {

        public Training()
        {
            CartItems = new HashSet<CartItem>();
        }

        public virtual ICollection<CartItem> CartItems { get; set; } = null;
        public string? HeaderImage { get; set; }
        public string? LangCode { get; set; }
        public int CategoryId { get; set; }
        public int InstructorId { get; set; }
        public string? TrainingType { get; set; }
        public decimal CurrentAmount { get; set; }
        public decimal Amount { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal TaxRate { get; set; }
        public decimal CertificateOfAchievementRate { get; set; }
        public decimal CertificateOfParticipationRate { get; set; }
        public int CompletionTime { get; set; }
        public int CompletionTimeUnitId { get; set; }
        public int TrainingLevelId { get; set; }
    }
}
