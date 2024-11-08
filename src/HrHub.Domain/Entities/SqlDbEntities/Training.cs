using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class Training : TypeCardEntity<long>
    {
        public string HeaderImage { get; set; }
        public string LangCode { get; set; }
        public long CategoryId { get; set; }
        public long InstructorId { get; set; }
        public string TrainingType { get; set; }
        public decimal CurrentAmount { get; set; }
        public decimal Amount { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal TaxRate { get; set; }
        public decimal CertificateOfAchievementRate { get; set; }
        public decimal CertificateOfParticipationRate { get; set; }
        public DateTime? CompletionTime { get; set; }
        public long CompletionTimeUnitId { get; set; }
        public long TrainingLevelId { get; set; }
    }
}
