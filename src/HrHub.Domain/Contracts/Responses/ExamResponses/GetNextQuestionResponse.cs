using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Responses.ExamResponses
{
    public class GetNextQuestionResponse
    {
        public long UserExamId { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsLastQuestion { get; set; }
        public string ExamEndMessage { get; set; }
        public GetQuestionResponse CurrentQuestion { get; set; }
        public string TopicTitle { get; set; } // Added Topic Title
        public string TopicImgPath { get; set; } // Added Topic Image Path
    }
}
