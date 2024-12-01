using HrHub.Abstraction.Attributes;
using HrHub.Core.BusinessRules;

namespace HrHub.Domain.Contracts.Dtos.UserDtos
{
    public class SetUserStatusDto
    {
        [ValidationRules(typeof(NullCheckRule))]
        public long UserId { get; set; }

        [ValidationRules(typeof(NullCheckRule))]
        public bool  IsActive { get; set; }
    }
}
