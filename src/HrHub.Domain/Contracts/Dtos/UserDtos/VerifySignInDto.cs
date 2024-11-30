using HrHub.Abstraction.Attributes;
using HrHub.Core.BusinessRules;

namespace HrHub.Domain.Contracts.Dtos.UserDtos
{
    public class VerifySignInDto
    {
        [ValidationRules(typeof(NullCheckRule))] 
        public string UserName { get; set; }
        [ValidationRules(typeof(NullCheckRule))]
        public string Code { get; set; }
    }
}
