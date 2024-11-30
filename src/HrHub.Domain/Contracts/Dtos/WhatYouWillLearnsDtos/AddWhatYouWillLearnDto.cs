namespace HrHub.Domain.Contracts.Dtos.WhatYouWillLearns;

public class AddWhatYouWillLearnDto
{
    public bool IsActive { get; set; }
    public string? Title { get; set; }
    public string? Abbreviation { get; set; }
    public string? Code { get; set; }
    public string? Description { get; set; }
    public long TrainingId { get; set; }

}
