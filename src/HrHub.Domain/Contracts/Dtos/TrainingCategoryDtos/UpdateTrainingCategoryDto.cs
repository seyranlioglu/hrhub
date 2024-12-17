namespace HrHub.Domain.Contracts.Dtos.TrainingCategoryDtos
{
    public class UpdateTrainingCategoryDto
    {
        public long Id { get; set; }
        public long? MasterCategoryId { get; set; }
        public string? Title { get; set; }
        public string? Abbreviation { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }

    }
}
