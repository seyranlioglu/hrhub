using HrHub.Abstraction.Domain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Identity.Entities
{
    public class AppUser : IdentityUser<long>, IBaseEntity
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        public string UserShortName { get; set; }
        public string PhoneNumber { get; set; }
        public long DepartmentId { get; set; }
        public long JobTitleId { get; set; }
        public long RoleId { get; set; }
        [AllowNull]
        public string? AuthCode { get; set; }
        public bool AdminFlag { get; set; } = false;
        [AllowNull]
        public int? IncorrectPinCount { get; set; }
        [AllowNull]
        public DateTime? LastLoginDate { get; set; }
        public long UserStatusId { get; set; } = 0;
        public int InactiveDurationDate { get; set; } = 0;
        [AllowNull] 
        public long UserTypeId { get; set; } = 0;
        public DateTime CreatedDate { get; set; }=DateTime.UtcNow;
        public bool IsDelete { get; set; }
    }
}
