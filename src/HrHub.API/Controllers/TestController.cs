using HrHub.Abstraction.Enums;
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

        [Authorize(Roles = "admin,user", Policy = "MainUser,Instractor")]
        [HttpGet("[Action]")]
        public IActionResult OnlyAdminAndMainUser()
        {

            return Ok();
        }

        // user id ile Instructor içinde varsa eğitim tnımlama yetkisini yapabilecek bir policy eklenecek
        //Instructor lar ilkaçılışta cache atılacak ve devamında cache de yapılacak
        [Authorize(Roles = "admin,user")]
        [HttpGet("[Action]")]
        public IActionResult OnlyAdmin()
        {

            return Ok();
        }

    }
}
