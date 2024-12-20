using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Responses.ExamResponses
{
    public class GetExamInstructionResponse
    {
        public long ExamVersionId { get; set; }
        public long UserExamId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TimeSpan? ExamTime { get; set; }
        public int TotalQuestionCount { get; set; }
        public List<ExamTopicInstruction> Topics { get; set; }
        public List<string> TopicsNames { get; set; }
    }
}
