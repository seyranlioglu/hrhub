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
        public long? CategoryId { get; set; }
        public long? InstructorId { get; set; }
        public long? TrainingTypeId { get; set; }
        public decimal? CurrentAmount { get; set; }
        public decimal? Amount { get; set; }
        public decimal? DiscountRate { get; set; }
        public decimal? TaxRate { get; set; }
        public decimal? CertificateOfAchievementRate { get; set; }
        public decimal? CertificateOfParticipationRate { get; set; }
        public DateTime? CompletionTime { get; set; }
        public long? CompletionTimeUnitId { get; set; }
        public long TrainingLevelId { get; set; }
        public long? TrainingStatusId { get; set; }
        public long? PreconditionId { get; set; }
        public long? ForWhomId { get; set; }
        public long? EducationLevelId { get; set; }
        public long? PriceTierId { get; set; }
        public long? TrainingContentId { get; set; }
        public string? Trailer { get; set; }
        public string? WelcomeMessage { get; set; }
        public string? CongratulationMessage { get; set; }
        public string? SubTitle { get; set; }
        public string? Labels { get; set; }
        public string? CourseImage { get; set; }
        public List<ContentDto?> ContentOrderIds { get; set; } = new();
    }

    public class ContentDto
    {
        public long SectionId { get; set; } 
        public List<ContentOrderDto> Contents { get; set; } = new(); 
    }

    public class ContentOrderDto
    {
        public long ContentId { get; set; }
    }
}
