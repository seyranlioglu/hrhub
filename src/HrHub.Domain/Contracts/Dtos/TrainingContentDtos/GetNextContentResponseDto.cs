using HrHub.Abstraction.Contracts.Dtos.TrainingDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Dtos.TrainingContentDtos
{
    public class GetNextContentResponseDto
    {
        public bool IsTrainingFinished { get; set; }
        public TrainingContentDto NextContent { get; set; } // Mevcut ContentDto'nuz
        public string Message { get; set; }
    }
}
