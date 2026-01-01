using HrHub.Abstraction.Attributes;
using HrHub.Core.BusinessRules;
using HrHub.Core.BusinessRules.TimeSpanBusinessRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Dtos.ExamDtos
{
    public class UpdateExamVersionDto
    {
        [ValidationRules(typeof(ZeroCheckRule))]
        public int Id { get; set; }
        [ValidationRules(typeof(NullCheckRule))]
        public string VersionDescription { get; set; }
        [ValidationRules(typeof(ZeroCheckRule))]
        public long ExamId { get; set; }
        [ValidationRules(typeof(ZeroCheckRule))]
        public int VersionNumber { get; set; }
        public bool IsPublished { get; set; }
        [ValidationRules(typeof(PositiveTimeSpanRule))]
        public string ExamTime { get; set; }
        [ValidationRules(typeof(ZeroCheckRule))]
        public decimal SuccesRate { get; set; }
        [ValidationRules(typeof(ZeroCheckRule))]
        public decimal PassingScore { get; set; }
        [ValidationRules(typeof(ZeroCheckRule))]
        public int TotalQuestionCount { get; set; }
        public long StatusId { get; set; }
    }
}
