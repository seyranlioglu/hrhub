namespace HrHub.Domain.Contracts.Dtos.ContentTypes
{
    public class ContentTypeDto
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string LangCode { get; set; }
        public string Icon { get; set; }
    }
}
