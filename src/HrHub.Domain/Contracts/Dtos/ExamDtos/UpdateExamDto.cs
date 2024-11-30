using HrHub.Abstraction.Attributes;
using HrHub.Core.BusinessRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Dtos.ExamDtos
{
    public class UpdateExamDto
    {
        [ValidationRules(typeof(ZeroCheckRule))]
        public int Id { get; set; }
        [ValidationRules(typeof(NullCheckRule))]
        public string Title { get; set; }
        public string Description { get; set; }
        [ValidationRules(typeof(ZeroCheckRule))]
        public long TrainingId { get; set; }
        [ValidationRules(typeof(ZeroCheckRule))]
        public long ExamStatusId { get; set; }
        public long ActionId { get; set; }
        [ValidationRules(typeof(NullCheckRule))]
        public UpdateExamVersionDto VersionInfo { get; set; }
    }
}
