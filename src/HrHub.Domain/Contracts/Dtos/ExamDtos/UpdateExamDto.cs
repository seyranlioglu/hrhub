using HrHub.Abstraction.Attributes;
using HrHub.Core.BusinessRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Dtos.ExamDtos
{
    public class UpdateExamDto : AddExamDto
    {
        [ValidationRules(typeof(ZeroCheckRule))]
        public int Id { get; set; }
        //public List<UpdateExamTopicDto> Topics { get; set; }
    }
}
