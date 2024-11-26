using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Responses.ExamResponses
{
    public class AddExamVersionReponse
    {
        public long NewVersionId { get; set; }
        public int NewVersionNumber { get; set; }
    }
}
