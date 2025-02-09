using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Dtos.ExamDtos
{
    public class GetExamListDto
    {
        public long? TrainingId { get; set; }
        public bool? IsActive { get; set; }
    }
}
