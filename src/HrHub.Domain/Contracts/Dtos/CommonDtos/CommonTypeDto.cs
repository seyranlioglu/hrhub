using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Dtos.CommonDtos
{
    public class CommonTypeDto
    {
        public string? Title { get; set; }
        [AllowNull]
        public string? Abbreviation { get; set; } = null;
        [AllowNull]
        public string? Code { get; set; }
        [AllowNull]
        public string? Description { get; set; }
    }
}
