using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Responses.ExamResponses
{
    public class GetExamVersionListResponse
    {
        public long VersionId { get; set; }
        public long ExamId { get; set; }
        public int VersionNo { get; set; }
        public double? ExamTimeInMin { get; set; }
        public decimal? SuccessRate { get; set; }
        public decimal? PassingScore { get; set; }
        public int? TotalQuestionCount { get; set; }

    }
}
