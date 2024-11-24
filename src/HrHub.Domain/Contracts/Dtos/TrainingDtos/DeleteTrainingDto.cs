using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Dtos.TrainingDtos
{
    public class DeleteTrainingDto
    {
        public long Id { get; set; }
        public bool IsDelete { get; set; }
    }
}
