using HrHub.Abstraction.Attributes;
using HrHub.Abstraction.Enums;
using HrHub.Core.BusinessRules;

namespace HrHub.Domain.Contracts.Dtos.UserDtos
{
    public class ForgotPasswordDto
    {
        [ValidationRules(typeof(NullCheckRule))]
        public string UserName { get; set; }
        [ValidationRules(typeof(NullCheckRule))]
        public SubmissionTypeEnum Type { get; set; }
    }
}
