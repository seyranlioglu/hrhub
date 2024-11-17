using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Abstraction.Contracts.Dtos.ExamDtos
{
    public class AddExamQuestionDto
    {
        public string QuestionText { get; set; }
        public decimal Score { get; set; }
        public List<AddExamQuestionOptionDto> QuestionOptions { get; set; } = null;
    }
}
