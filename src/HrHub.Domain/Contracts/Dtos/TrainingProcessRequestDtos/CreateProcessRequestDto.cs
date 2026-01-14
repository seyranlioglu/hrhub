using HrHub.Domain.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Dtos.TrainingProcessRequestDtos
{
    public class CreateProcessRequestDto
    {
        public long TrainingId { get; set; }
        public TrainingRequestType RequestType { get; set; }
        public long? CurrAccTrainingUserId { get; set; }
        public string? Note { get; set; }
    }
}
