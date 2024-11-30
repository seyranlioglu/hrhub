using HrHub.Abstraction.Attributes;
using HrHub.Abstraction.Enums;
using HrHub.Core.BusinessRules;

namespace HrHub.Domain.Contracts.Dtos.UserDtos
{
    public class VerifyDto
    {
        [ValidationRules(typeof(NullCheckRule))] 
        public string UserName { get; set; }
        [ValidationRules(typeof(NullCheckRule))] 
        public string CodeParameter { get; set; }
        [ValidationRules(typeof(NullCheckRule))] 
        public string Code { get; set; }
        [ValidationRules(typeof(NullCheckRule))] 
        public SubmissionTypeEnum SubmissionType { get; set; }
    }
}
