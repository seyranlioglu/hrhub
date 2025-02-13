using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Responses.ExamResponses
{
    public class GetExamListForLookupResponse
    {
        public List<ExamItem> Exams { get; set; }
    }

    public class ExamItem
    {
        public long ExamId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ExamStatus { get; set; }
    }
}
