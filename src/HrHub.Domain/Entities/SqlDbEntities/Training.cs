using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class Training : TypeCardEntity<int>
    {
        public string HeaderImage { get; set; }
        public string LangCode { get; set; }
        public int CategoryId { get; set; }
        public int InstructorId { get; set; }
        public string TrainingType { get; set; }
        public decimal CurrentAmount { get; set; }
        public decimal Amount { get; set; }
        public float DiscountRate { get; set; }
        public float TaxRate { get; set; }
        public float CertificateOfAchievementRate { get; set; }
        public float CertificateOfParticipationRate { get; set; }
        public int CompletionTime { get; set; }
        public int CompletionTimeUnitId { get; set; }
        public int TrainingLevelId { get; set; }
    }
}
