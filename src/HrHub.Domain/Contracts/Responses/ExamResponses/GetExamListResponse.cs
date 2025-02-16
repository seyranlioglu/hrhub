using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Responses.ExamResponses
{
    public class GetExamListResponse
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string TrainingTitle { get; set; }
        /// <summary>
        /// Dakika Cinsinden Sınav Süresi
        /// </summary>
        public double? ExamTimeInMin { get; set; }
        public decimal? SuccessRate { get; set; }
        public decimal? PassingScore { get; set; }
        public int? TotalQuestionCount { get; set; }
        public string ExamStatus { get; set; }
        public GetExamVersionListResponse ActiveVersions { get; set; } = null;

    }
}
