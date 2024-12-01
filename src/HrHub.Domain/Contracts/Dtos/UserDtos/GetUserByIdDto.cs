using HrHub.Abstraction.Attributes;
using HrHub.Core.BusinessRules;

namespace HrHub.Domain.Contracts.Dtos.UserDtos
{
    public class GetUserByIdDto
    {
        [ValidationRules(typeof(ZeroCheckRule), typeof(NullCheckRule))]
        public long Id { get; set; }
    }
}
