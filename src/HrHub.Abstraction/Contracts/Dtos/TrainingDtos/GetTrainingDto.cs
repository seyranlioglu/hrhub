namespace HrHub.Abstraction.Contracts.Dtos.TrainingDtos
{
    public class GetTrainingDto
    {
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
        public DateTime? CompletionTime { get; set; }
        public long CompletionTimeUnitId { get; set; }
        public long TrainingLevelId { get; set; }
    }
}
