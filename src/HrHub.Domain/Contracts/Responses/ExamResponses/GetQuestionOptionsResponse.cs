using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Responses.ExamResponses
{
    public class GetQuestionOptionsResponse
    {
        public long Id { get; set; }
        public string OptionText { get; set; }
    }
}
