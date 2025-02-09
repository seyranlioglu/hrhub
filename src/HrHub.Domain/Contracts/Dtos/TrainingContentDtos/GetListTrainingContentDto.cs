namespace HrHub.Domain.Contracts.Dtos.TrainingContentDtos
{
    public class GetListTrainingContentDto
    {
        public long Id { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsActive { get; set; }
        public string? Title { get; set; }
        public string? Abbreviation { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public TimeSpan? Time { get; set; }
        public int? PageCount { get; set; }
        public decimal? CompletedRate { get; set; }
        public string? FilePath { get; set; }
        public bool? Mandatory { get; set; }
        public long? OrderId { get; set; }
        public bool? AllowSeeking { get; set; }
        public int? PartCount { get; set; }
        public int? MinReadTimeThreshold { get; set; }


        //TrainingSection
        public string? TrainingSectionTitle { get; set; }
        public string? TrainingSectionAbbreviation { get; set; }
        public string? TrainingSectionCode { get; set; }
        public string? TrainingSectionDescription { get; set; }

        //ContentType
        public string? TrainingContentTitle { get; set; }
        public string? TrainingContentAbbreviation { get; set; }
        public string? TrainingContentCode { get; set; }
        public string? TrainingContentDescription { get; set; }
        public string? TrainingContentLangCode { get; set; }


        //ContentLibrary
        public List<GetContentLibraryDto> ContentLibraries { get; set; }

    }
    public class GetContentLibraryDto
    {
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public string? Thumbnail { get; set; }
        public TimeSpan? VideoDuration { get; set; }
    }
}
