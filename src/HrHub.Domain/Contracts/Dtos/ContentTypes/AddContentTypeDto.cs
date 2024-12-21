namespace HrHub.Domain.Contracts.Dtos.ContentTypes
{
    public class AddContentTypeDto
    {
        public string? Title { get; set; }
        public string? Abbreviation { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public string? LangCode { get; set; }
        public bool IsActive { get; set; }
    }
}
