using HrHub.Abstraction.Attributes;
using HrHub.Core.BusinessRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Dtos.ExamDtos
{
    public class AddExamTopicDto
    {
        [ValidationRules(typeof(ZeroCheckRule))]
        public long ExamVersionId { get; set; }
        [ValidationRules(typeof(NullCheckRule))]
        public string Title { get; set; }
        [ValidationRules(typeof(NullCheckRule))]
        public string ImgPath { get; set; }
    }
}
