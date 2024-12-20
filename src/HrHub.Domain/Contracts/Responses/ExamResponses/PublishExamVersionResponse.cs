using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Responses.ExamResponses
{
    public class PublishExamVersionResponse
    {
        public bool Result { get; set; }
        public string ExamError { get; set; }
        public string ExamVersionError { get; set; }
        public List<string> ExamTopicErrors { get; set; } = new List<string>();
        public List<string> ExamQuestionErrors { get; set; } = new List<string>();
    }
}
