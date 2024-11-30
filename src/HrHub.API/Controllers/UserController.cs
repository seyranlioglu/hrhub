using HrHub.Abstraction.Result;
using HrHub.Application.Managers.UserManagers;
using HrHub.Domain.Contracts.Dtos.UserDtos;
using HrHub.Domain.Contracts.Responses.UserResponses;
using Microsoft.AspNetCore.Mvc;

namespace HrHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserManager userManager;
        public UserController(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost("[Action]")]
        public async Task<Response<UserSignInResponse>> SignIn([FromBody] UserSignInDto signIn)
        {
            var result = await userManager.SignIn(signIn);
            return result;
        }
        [HttpPost("[Action]")]
        public async Task<Response<VerifySignInResponse>> VerifySignIn([FromBody] VerifySignInDto verifyDto)
        {
            var result = await userManager.VerifyCodeAndSignIn(verifyDto);
            return result;
        }
        [HttpPost("[Action]")]
        public async Task<Response<UserSignUpResponse>> SignUp([FromBody] UserSignUpDto dto)
        {
            var result = await userManager.SignUp(dto);
            return result;
        }

        [HttpPost("[Action]")]
        public async Task<Response<VerifySendResponse>> VerifyCodeSend([FromBody] VerifySendDto verifySendDto)
        {
            var result = await userManager.VerifyCodeSend(verifySendDto);
            return result;
        }

        [HttpPost("[Action]")]
        public async Task<Response<VerifyResponse>> VerifyConfirm([FromBody] VerifyDto verifyDto)
        {
            var result = await userManager.VerifyCodeAndConfirm(verifyDto);
            return result;
        }
    }
}
