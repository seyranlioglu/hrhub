using HrHub.Identity.Model;
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

        [HttpPost("[Action]")]
        public async Task<IActionResult> SignIn([FromBody]SignInViewModelResource signInQuery)
        {
            var token = await authenticationService.SignIn(signInQuery);
            return Ok(token);
        }
    }
}
