using HrHub.Abstraction.Extensions;
using HrHub.Identity.Entities;
using HrHub.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HrHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        [Authorize(Roles = "admin,user", Policy = "MainUser")]
        [HttpGet("[Action]")]
        public IActionResult OnlyAdminAndMainUser()
        {

            return Ok();
        }


        [Authorize(Roles = "admin")]
        [HttpGet("[Action]")]
        public IActionResult OnlyAdmin()
        {

            return Ok();
        }

    }
}
