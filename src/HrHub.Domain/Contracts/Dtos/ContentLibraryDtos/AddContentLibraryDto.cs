namespace HrHub.Domain.Contracts.Dtos.ContentLibraryDtos
{
    public class AddContentLibraryDto
    {
        public long Id { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public long FileTypeId { get; set; }
        public long? TrainingContentId { get; set; }
        public long? CreateUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public long? UpdateUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public long? DeleteUserId { get; set; }
        public DateTime? DeleteDate { get; set; }
        public bool? IsDelete { get; set; }
        public bool IsActive { get; set; }
        public string? Title { get; set; }
        public string? Abbreviation { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
    }
}
