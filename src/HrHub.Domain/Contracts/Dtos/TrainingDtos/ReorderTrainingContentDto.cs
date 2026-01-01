using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Dtos.TrainingDtos
{
    public class ReorderTrainingContentDto
    {
        public long TrainingId { get; set; }
        public List<ContentDto> ContentOrderIds { get; set; } = new();
    }
}
