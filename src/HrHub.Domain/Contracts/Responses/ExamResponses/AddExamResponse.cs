using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Responses.ExamResponses
{
    public class AddExamResponse
    {
        public long Id { get; set; }
        public long ExamVersionId { get; set; }
        public int VersionNumber { get; set; }
    }
}
