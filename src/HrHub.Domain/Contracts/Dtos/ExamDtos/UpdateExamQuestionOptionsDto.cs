using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Dtos.ExamDtos
{
    public class UpdateExamQuestionOptionsDto : AddExamQuestionOptionDto
    {
        public long Id { get; set; }
    }
}
