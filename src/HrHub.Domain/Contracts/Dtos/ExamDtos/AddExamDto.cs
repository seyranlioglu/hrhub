using HrHub.Abstraction.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HrHub.Core.Rules;

namespace HrHub.Domain.Contracts.Dtos.ExamDtos
{
    [ValidationRules(typeof(EmailFormatRule))]
    public class AddExamDto
    {
        [ValidationRules(typeof(NullCheckRule))]
        public string Title { get; set; }
        public string Description { get; set; }
        public long TrainingId { get; set; }
        public TimeSpan ExamTime { get; set; }
        [ValidationRules(typeof(NullCheckRule),typeof(ZeroCheckRule))]
        public long SuccesRate { get; set; }
        [ValidationRules(typeof(ZeroCheckRule))]
        public decimal PassingScore { get; set; }
        [ValidationRules(typeof(ZeroCheckRule))]
        public long ViewQuestionCount { get; set; }
        public long ExamStatusId { get; set; }
    }
}
