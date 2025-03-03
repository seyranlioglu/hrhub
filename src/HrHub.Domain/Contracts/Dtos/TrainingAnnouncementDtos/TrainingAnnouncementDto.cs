namespace HrHub.Domain.Contracts.Dtos.TrainingAnnouncementDtos
{
    public class TrainingAnnouncementDto
    {
        public long Id { get; set; }
        public long TrainingId { get; set; }
        public long UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Editable { get; set; }
    }
}
