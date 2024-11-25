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
        public bool IsMainUser { get; set; }
        public string? AuthCode { get; set; }
        public long CurAccId { get; set; }
    }
}
