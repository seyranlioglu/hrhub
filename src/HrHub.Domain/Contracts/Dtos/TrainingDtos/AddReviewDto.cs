namespace HrHub.Domain.Contracts.Dtos.TrainingDtos
{
    public class AddReviewDto
    {
        public long TrainingId { get; set; }
        public int Rating { get; set; } // 1-5 arası
        public string Comment { get; set; }
    }
}