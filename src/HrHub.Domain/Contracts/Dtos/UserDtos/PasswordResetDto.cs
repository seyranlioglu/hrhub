using HrHub.Abstraction.Attributes;
using HrHub.Core.BusinessRules;

namespace HrHub.Domain.Contracts.Dtos.UserDtos
{
    public class PasswordResetDto
    {
        [ValidationRules(typeof(NullCheckRule))]
        public string UserName { get; set; }
        [ValidationRules(typeof(NullCheckRule))]
        public string Password { get; set; }
    }
}
