namespace HrHub.Domain.Contracts.Dtos.TrainingDtos
{
    public class RejectTrainingDto
    {
        public long TrainingId { get; set; }
        public string Reason { get; set; } // Ret sebebi
    }
}