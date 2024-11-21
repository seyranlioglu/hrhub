using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Dtos.ExamDtos
{
    public class UpdateExamTopicDto : AddExamTopicDto
    {
        public long Id { get; set; }
        public List<UpdateExamQuestionDto> Questions { get; set; }
    }
}
