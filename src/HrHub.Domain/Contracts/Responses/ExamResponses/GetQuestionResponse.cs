using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Responses.ExamResponses
{
    public class GetQuestionResponse
    {
        public long Id { get; set; }
        public string QuestionText { get; set; }
        public List<GetQuestionOptionsResponse> Options { get; set; }
    }
}
