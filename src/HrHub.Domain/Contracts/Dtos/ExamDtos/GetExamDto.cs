using HrHub.Abstraction.Attributes;
using HrHub.Core.BusinessRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Dtos.ExamDtos
{
    public class GetExamDto
    {
        [ValidationRules(typeof(ZeroCheckRule))]
        public long ExamId { get; set; }
    }
}
