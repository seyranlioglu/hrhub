using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Responses.ExamResponses
{
    public class CalculateExamResultResponse
    {
        public long UserExamId { get; set; }
        public decimal TotalScore { get; set; }
        public decimal UserScore { get; set; }
        public decimal SuccessRate { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
