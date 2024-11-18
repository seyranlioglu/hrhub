using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Abstraction.Contracts.Dtos.ExamDtos
{
    public class AddExamQuestionOptionDto
    {
        public string OptionText { get; set; }
        public bool IsCorrect { get; set; }
    }
}
