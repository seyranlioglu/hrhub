namespace HrHub.Domain.Contracts.Dtos.TrainingContentDtos
{
    public class UpdateTrainingContentDto
    {
        public long Id { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsActive { get; set; }
        public string? Title { get; set; }
        public string? Abbreviation { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public long? TrainingSectionId { get; set; }
        public long? ContentTypeId { get; set; }
        public TimeSpan? Time { get; set; }
        public int? PageCount { get; set; }
        public decimal? CompletedRate { get; set; }
        public string? FilePath { get; set; }
        public bool? Mandatory { get; set; }
        public long? OrderId { get; set; }
        public bool? AllowSeeking { get; set; }
        public int? PartCount { get; set; }
        public int? MinReadTimeThreshold { get; set; }
        public string? ContentLibraryFilePath { get; set; }
        public string? ContentLibraryFileName { get; set; }
    }
}
