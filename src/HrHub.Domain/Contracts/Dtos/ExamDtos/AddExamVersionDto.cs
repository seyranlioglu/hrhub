using HrHub.Abstraction.Attributes;
using HrHub.Core.BusinessRules.TimeSpanBusinessRules;
using HrHub.Core.BusinessRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Dtos.ExamDtos
{
    public class AddExamVersionDto
    {
        [ValidationRules(typeof(NullCheckRule))]
        public string VersionDescription { get; set; }
        [ValidationRules(typeof(ZeroCheckRule))]
        public long ExamId { get; set; }
        [ValidationRules(typeof(ZeroCheckRule))]
        public int VersionNumber { get; set; }
        public bool IsPublished { get; set; }
        public TimeSpan? ExamTime { get; set; }
        public decimal? SuccesRate { get; set; }
        public decimal? PassingScore { get; set; }
        public int? TotalQuestionCount { get; set; }
    }
}
