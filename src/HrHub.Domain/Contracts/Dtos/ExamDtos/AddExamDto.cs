using HrHub.Abstraction.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HrHub.Core.BusinessRules;
using HrHub.Core.BusinessRules.TimeSpanBusinessRules;

namespace HrHub.Domain.Contracts.Dtos.ExamDtos
{
    public class AddExamDto
    {
        [ValidationRules(typeof(NullCheckRule))]
        public string Title { get; set; }
        [ValidationRules(typeof(NullCheckRule))]
        public string Description { get; set; }
        [ValidationRules(typeof(ZeroCheckRule))]
        public long TrainingId { get; set; }
        [ValidationRules(typeof(ZeroCheckRule))]
        public long ExamStatusId { get; set; }
        [ValidationRules(typeof(NullCheckRule))]
        public AddExamVersionDto VersionInfo { get; set; }
    }
}
