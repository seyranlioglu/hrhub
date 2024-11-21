namespace HrHub.Abstraction.Contracts.Dtos.CommonDtos
{
    public class CommonTypeEntityDto
    {
        public long Id { get; set; }
        public string? Title { get; set; }
        public string? Abbreviation { get; set; } = null;
        public string? Code { get; set; }
        public string? Description { get; set; }
    }
}
