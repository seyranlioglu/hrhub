namespace HrHub.Domain.Contracts.Dtos.TrainingSectionDtos
{
    public class UpdateTrainingSectionDto
    {
        public long Id { get; set; }
        public bool? IsActive { get; set; }
        public string? Title { get; set; }
        public string? Abbreviation { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public long? TrainingId { get; set; }
        public long? RowNumber { get; set; }
        public string? LangCode { get; set; }
    }
}
