using HrHub.Identity.Entities;
using HrHub.Identity.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Identity.Services
{
    public class AppRoleService : BaseService, IAppRoleService
    {
        public AppRoleService(UserManager<AppUser> userManager,
                              SignInManager<AppUser> signInManager,
                              RoleManager<AppRole> roleManager) : base(userManager, signInManager, roleManager)
        {
        }


        public async Task<List<RoleModel>> GetRoleList()
        {
            var result = await roleManager.Roles
                        .OrderBy(s => s.Name) 
                        .Select(s => new RoleModel
                        {
                            Id = s.Id,
                            Name = s.Name
                        }).ToListAsync();
            return result;
        }
        public async Task AddRole(RoleModel role)
        {
            try
            {
                var result = await roleManager.CreateAsync(new AppRole { Name = role.Name });
            }
            catch (Exception e)
            {

               
            }
           
        }
    }
}
