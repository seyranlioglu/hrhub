using HrHub.Abstraction.Domain;
using Microsoft.AspNetCore.Identity;

namespace HrHub.Identity.Entities
{
    public class AppRole : IdentityRole<long>, IBaseEntity
    {
    }
}
