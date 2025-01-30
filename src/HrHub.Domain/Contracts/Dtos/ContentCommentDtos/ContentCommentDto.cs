namespace HrHub.Domain.Contracts.Dtos.ContentCommentDtos
{
    public class ContentCommentDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long ContentId { get; set; }
        public bool Pinned { get; set; }
        public long UserId { get; set; }
        public long? MasterContentId { get; set; }
        public int StarCount { get; set; }
        public bool Editable { get; set; }
    }
}
