using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Abstraction.Contracts.Dtos.ExamDtos
{
    public class AddExamDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public long TrainingId { get; set; }
        public TimeSpan ExamTime { get; set; }
        public long SuccesRate { get; set; }
        public decimal PassingScore { get; set; }
        public long ViewQuestionCount { get; set; }
        public long ExamStatusId { get; set; }
    }
}
