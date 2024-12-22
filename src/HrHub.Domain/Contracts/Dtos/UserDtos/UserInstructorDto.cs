using HrHub.Abstraction.Attributes;
using HrHub.Core.BusinessRules;

namespace HrHub.Domain.Contracts.Dtos.UserDtos
{
    public class UserInstructorDto
    {
        [ValidationRules(typeof(NullCheckRule), typeof(ZeroCheckRule))] 
        public long UserId { get; set; }
        public string? Facebook { get; set; }
        public string? Linkedin { get; set; }
        public string? Twitter { get; set; }
        public string? Instagram { get; set; }
        public string? Title { get; set; }
        [ValidationRules(typeof(NullCheckRule), typeof(ZeroCheckRule))] 
        public long InstructorTypeId { get; set; }
    }
}
