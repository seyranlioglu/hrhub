using HrHub.Abstraction.Attributes;
using HrHub.Core.BusinessRules;

namespace HrHub.Domain.Contracts.Dtos.UserDtos
{
    public class UserSignUpDto
    {
        [ValidationRules(typeof(NullCheckRule))]
        public string? Email { get; set; }
        [ValidationRules(typeof(NullCheckRule))] 
        public string? PhoneNumber { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Title { get; set; }
        public string? Address { get; set; }
        public string? TaxNumber { get; set; }
        public string? IdentityNumber { get; set; }
        public string? PostalCode { get; set; }
        [ValidationRules(typeof(NullCheckRule))] 
        public long CurrAccTypeId { get; set; }
        public bool? IsMainUser { get; set; }
    }
}
