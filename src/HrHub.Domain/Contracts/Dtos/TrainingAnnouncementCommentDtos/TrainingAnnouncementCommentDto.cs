namespace HrHub.Domain.Contracts.Dtos.TrainingAnnouncementCommentDtos
{
    public class TrainingAnnouncementCommentDto
    {
        public long Id { get; set; }
        public long TrainingAnnouncementsId { get; set; }
        public long UserId { get; set; }
        public string Description { get; set; }
        public bool Editable { get; set; }
    }
}
