namespace HrHub.Domain.Contracts.Dtos.TrainingContentDtos
{
    public class DeleteTrainingContentDto
    {
        public long Id { get; set; }
        public bool? IsDelete { get; set; }
        public bool IsActive { get; set; }
    }
}
