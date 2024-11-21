using HrHub.Abstraction.Attributes;
using HrHub.Core.BusinessRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Dtos.ExamDtos
{
    public class UpdateExamQuestionDto
    {
        public long Id { get; set; }
        [ValidationRules(typeof(NullCheckRule))]
        public string QuestionText { get; set; }
        [ValidationRules(typeof(ZeroCheckRule))]
        public decimal Score { get; set; }
        [ValidationRules(typeof(NullCheckRule))]
        public List<UpdateExamQuestionOptionsDto> QuestionOptions { get; set; } = null;
    }
}
