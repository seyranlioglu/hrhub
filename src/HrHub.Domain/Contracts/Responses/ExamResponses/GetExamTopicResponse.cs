using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Responses.ExamResponses
{
    public class GetExamTopicResponse
    {
        public long Id { get; set; }
        public int QuestionCount { get; set; }
        public string Title { get; set; }
        public string ImgPath { get; set; }
        public List<GetQuestionResponse> Questions { get; set; }
    }
}
