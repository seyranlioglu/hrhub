using HrHub.Abstraction.Domain;
using HrHub.Identity.Model;
using Microsoft.AspNetCore.Identity;

namespace HrHub.Identity.Services
{
    public interface IAppRoleService 
    {
        Task<List<RoleModel>> GetRoleList();
    }
}