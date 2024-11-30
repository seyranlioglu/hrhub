using HrHub.Abstraction.Attributes;
using HrHub.Abstraction.Enums;
using HrHub.Core.BusinessRules;

namespace HrHub.Domain.Contracts.Dtos.UserDtos
{
    public class VerifySendDto
    {

        [ValidationRules(typeof(NullCheckRule))] 
        public string Receiver { get; set; }
        [ValidationRules(typeof(NullCheckRule))] 
        public SubmissionTypeEnum Type { get; set; }
    }
}
