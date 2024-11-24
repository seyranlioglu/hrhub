namespace HrHub.Abstraction.Contracts.Dtos.TrainingDtos
{
    public class GetTrainingDto
    {
        public long Id { get; set; }
        public string? HeaderImage { get; set; }
        public string? LangCode { get; set; }

        // Category Details
        public string? CategoryCode { get; set; }
        public string? CategoryTitle { get; set; }
        public string? CategoryDescription { get; set; }

        // Instructor Details
        public string Email { get; set; }
        public string Title { get; set; }

        public string? TrainingType { get; set; }
        public decimal CurrentAmount { get; set; }
        public decimal Amount { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal TaxRate { get; set; }
        public decimal CertificateOfAchievementRate { get; set; }
        public decimal CertificateOfParticipationRate { get; set; }
        public DateTime? CompletionTime { get; set; }

        // TimeUnit Details
        public string? CompletionTimeUnitCode { get; set; }
        public string? CompletionTimeUnitDescription { get; set; }

        // Training Level Details
        public string? TrainingLevelCode { get; set; }
        public string? TrainingLevelDescription { get; set; }
    }

}
