using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Dtos.ExamDtos
{
    public class AddExamTopicDto
    {
        public string Title { get; set; }
        public string ImgPath { get; set; }

        public List<AddExamQuestionDto> ExamQuestions { get; set; } = null;
    }
}
