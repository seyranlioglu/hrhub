using System.Diagnostics.CodeAnalysis;

namespace HrHub.Domain.Contracts.Dtos.CommonDtos
{
    public class CommonTypeDto
    {
        public string? Title { get; set; }
        public string? Abbreviation { get; set; } = null;
        public string? Code { get; set; }
        public string? Description { get; set; }
    }
}
