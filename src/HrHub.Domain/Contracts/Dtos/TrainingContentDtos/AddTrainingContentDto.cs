using Microsoft.AspNetCore.Http;

namespace HrHub.Domain.Contracts.Dtos.TrainingContentDtos
{
    public class AddTrainingContentDto
    {
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
        //public string? FilePath { get; set; }
        public bool? Mandatory { get; set; }
        public long? OrderId { get; set; }
        public bool? AllowSeeking { get; set; }
        public int? PartCount { get; set; }
        public int? MinReadTimeThreshold { get; set; }

        public long? ContentLibraryId { get; set; } 
        public IFormFile? File { get; set; } 

        //public string? ContentLibraryFilePath { get; set; }
        //public string? ContentLibraryFileName { get; set; }
    }
}
