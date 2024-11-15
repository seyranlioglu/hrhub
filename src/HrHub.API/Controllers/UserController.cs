using HrHub.Identity.Services;
using Microsoft.AspNetCore.Mvc;

namespace HrHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IAppUserService appUserService;
        private readonly IAuthenticationService authenticationService;
        private readonly IAppRoleService appRoleService;
        public UserController(IAuthenticationService authenticationService, IAppRoleService appRoleService, IAppUserService appUserService)
        {
            this.authenticationService = authenticationService;
            this.appRoleService = appRoleService;
            this.appUserService = appUserService;
        }

        [HttpGet("[Action]")]
        public async Task<IActionResult> SignIn()
        {

            //await appUserService.SignUpAsync(new Identity.Model.SignUpDto
            //{
            //    Email = "osman.burnak@asiselektronik.com.tr",
            //    AuthCode = Guid.NewGuid().TrimHyphen(),
            //    IsMainUser = true,
            //    Name = "Osman",
            //    Password = "Osman123**",
            //    PhoneNumber = "5374311810",
            //    SurName = "Burnak"

            //});

            //var rolee =(await appRoleService.GetRoleList()).ToList();
            //var role = new AppRole
            //{
            //    Id = rolee[0].Id,
            //    Name = rolee[0].Name
            //};
            //var user = await appUserService.GetUserByIdAsync(6);
            //await appUserService.AddUserRole(user, role);
            //var role2 = new AppRole
            //{
            //    Id = rolee[1].Id,
            //    Name = rolee[1].Name
            //};
            //await appUserService.AddUserRole(user, role2);
            var token = await authenticationService.SignIn(new Identity.Model.SignInViewModelResource { Email = "osman.burnak@asiselektronik.com.tr", Password = "Osman123**" });

            return Ok(token);
        }
    }
}
