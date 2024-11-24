namespace HrHub.Domain.Contracts.Dtos.TrainingDtos
{
    public class UpdateTrainingDto
    {
        public long Id { get; set; }
        public string? Title { get; set; }
        public string? Abbreviation { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
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
        public long TrainingStatusId { get; set; }
    }
}
