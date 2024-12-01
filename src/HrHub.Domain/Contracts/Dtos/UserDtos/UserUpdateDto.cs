using HrHub.Abstraction.Attributes;
using HrHub.Core.BusinessRules;

namespace HrHub.Domain.Contracts.Dtos.UserDtos
{
    public class UserUpdateDto
    {
        [ValidationRules(typeof(NullCheckRule), typeof(ZeroCheckRule))]
        public long Id { get; set; }
        [ValidationRules(typeof(NullCheckRule))]
        public string Name { get; set; }
        public string? SurName { get; set; }
        [ValidationRules(typeof(NullCheckRule))]
        public long CurrAccId { get; set; }
    }
}
