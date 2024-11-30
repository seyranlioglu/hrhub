using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Responses.ExamResponses
{
    public class GetExamResponse
    {
        public long ExamId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TimeSpan ExamTime { get; set; }
        public decimal SuccessRate { get; set; }
        public decimal PassingScore { get; set; }
        public int TotalQuestionCount { get; set; }
        public List<GetExamTopicResponse> Topics { get; set; }
    }
}
