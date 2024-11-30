namespace HrHub.Domain.Contracts.Dtos.WhatYouWillLearns
{
    public class UpdateWhatYouWillLearnDto
    {
        public long Id { get; set; }
        public bool IsActive { get; set; }
        public string? Title { get; set; }
        public string? Abbreviation { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public long TrainingId { get; set; }
    }
}
