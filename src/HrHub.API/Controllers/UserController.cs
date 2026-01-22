using HrHub.Abstraction.Consts;
using HrHub.Abstraction.Result;
using HrHub.Application.Managers.UserManagers;
using HrHub.Domain.Contracts.Dtos.UserDtos;
using HrHub.Domain.Contracts.Responses.UserResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

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
        [AllowAnonymous]
        [HttpPost("[Action]")]
        public async Task<Response<UserSignInResponse>> SignIn([FromBody] UserSignInDto signIn)
        {
            var result = await userManager.SignIn(signIn);
            return result;
        }
        [AllowAnonymous]
        [HttpPost("[Action]")]
        public async Task<Response<VerifySignInResponse>> VerifySignIn([FromBody] VerifySignInDto verifyDto)
        {
            var result = await userManager.VerifyCodeAndSignIn(verifyDto);
            return result;
        }
        [AllowAnonymous]
        [HttpPost("[Action]")]
        public async Task<Response<UserSignUpResponse>> SignUp([FromBody] UserSignUpDto dto)
        {
            var result = await userManager.SignUp(dto);
            return result;
        }

        [AllowAnonymous]
        [HttpPost("[Action]")]
        public async Task<Response<VerifySendResponse>> VerifyCodeSend([FromBody] VerifySendDto verifySendDto)
        {
            var result = await userManager.VerifyCodeSend(verifySendDto);
            return result;
        }
        [AllowAnonymous]
        [HttpPost("[Action]")]
        public async Task<Response<VerifyResponse>> VerifyConfirm([FromBody] VerifyDto verifyDto)
        {
            var result = await userManager.VerifyCodeAndConfirm(verifyDto);
            return result;
        }
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Admin},{Roles.User}", Policy = Policies.MainUser)]
        [HttpPost("[Action]")]
        public async Task<Response<CommonResponse>> AddUser([FromBody] AddUserDto addUser)
        {
            var result = await userManager.AddUser(addUser);
            return result;
        }
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.User},{Roles.Admin}")]
        [HttpPost("[Action]")]
        public async Task<Response<CommonResponse>> ChangePassword([FromBody] ChangePasswordDto changePassword)
        {
            var result = await userManager.ChangePassword(changePassword);
            return result;
        }
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.User},{Roles.Admin}")]
        [HttpPost("[Action]")]
        public async Task<Response<CommonResponse>> VerifyChangePassword([FromBody] VerifyChangePasswordDto verifyChangePassword)
        {
            var result = await userManager.VerifyCodeAndChangePassword(verifyChangePassword);
            return result;
        }

        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.User},{Roles.Admin}")]
        [HttpPost("[Action]")]
        public async Task<Response<CommonResponse>> ChangePasswordReset([FromBody] PasswordResetDto passwordReset)
        {
            var result = await userManager.PasswordReset(passwordReset, "Password Change");
            return result;
        }

        [AllowAnonymous]
        [HttpPost("[Action]")]
        public async Task<Response<CommonResponse>> ForgotPassword([FromBody] ForgotPasswordDto forgotPassword)
        {
            var result = await userManager.ForgotPassword(forgotPassword);
            return result;
        }
        [AllowAnonymous]
        [HttpPost("[Action]")]
        public async Task<Response<CommonResponse>> VerifyForgotPassword([FromBody] VerifyForgotPasswordDto verifyChangePassword)
        {
            var result = await userManager.VerifyCodeAndForgotPassword(verifyChangePassword);
            return result;
        }

        [AllowAnonymous]
        [HttpPost("[Action]")]
        public async Task<Response<CommonResponse>> ForgotPasswordReset([FromBody] PasswordResetDto passwordReset)
        {
            var result = await userManager.PasswordReset(passwordReset, "Forgot Password");
            return result;
        }
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.User}", Policy = Policies.MainUser)]
        [HttpGet("[Action]")]
        public async Task<Response<List<GetUserResponse>>> GetList()
        {
            var result = await userManager.GetUserList();
            return result;
        }

        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.User},{Roles.Admin}")]
        [HttpPost("[Action]")]
        public async Task<Response<GetUserResponse>> GetById( [FromBody]GetUserByIdDto getUserById)
        {
            var result = await userManager.GetUserById(getUserById);
            return result;
        }


        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.User}", Policy = Policies.MainUser)]
        [HttpPost("[Action]")]
        public async Task<Response<CommonResponse>> SetStatus(SetUserStatusDto setUserStatusDto)
        {
            var result = await userManager.SetUserStatus(setUserStatusDto);
            return result;
        }
        [Authorize(Roles = Roles.SuperAdmin)]
        [HttpPost("[Action]")]
        public async Task<Response<CommonResponse>> ChangeUserPassword(PasswordResetDto passwordResetDto)
        {
            var result = await userManager.PasswordReset(passwordResetDto, "Password Change by SuperAdmin", isSendMail: true);
            return result;
        }

        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Admin},{Roles.User}", Policy = Policies.MainUser)]
        [HttpPut("[Action]")]
        public async Task<Response<CommonResponse>> Update(UserUpdateDto updateUserDto)
        {
            var result = await userManager.UpdateUser(updateUserDto);
            return result;
        }

        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Admin},{Roles.User}", Policy = Policies.MainUser)]
        [HttpDelete("[action]/{userId:long}")]
        public async Task<Response<CommonResponse>> Delete(long userId)
        {
            var result = await userManager.DeleteUser(userId);
            return result;
        }

        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.User}", Policy = Policies.MainUser)]
        [HttpPost("[action]")]
        public async Task<Response<CommonResponse>> SetUserInstructor(UserInstructorDto dto)
        {
            var result = await userManager.SetUserInstructor(dto);
            return result;
        }
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Admin},{Roles.User}", Policy = Policies.MainUser)]
        [HttpGet("[Action]")]
        public async Task<Response<List<CurrAccTypeDto>>> CurrAccTypeList()
        {
            var result = await userManager.GetCurrAccTypeList();
            return result;
        }

        [HttpGet("managed-users")]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Admin},{Roles.User}", Policy = Policies.MainUser)]
        public async Task<Response<List<ManagedUserDto>>> GetManagedUsers()
        {
            var result = await userManager.GetManagedUsersAsync();
            return result;
        }

        /// <summary>
        /// Sisteme yeni personel davet eder veya mevcutsa transfer eder.
        /// </summary>
        [HttpPost("invite-user")]
        [Authorize(Policy = Policies.MainUser)] // Sadece Kurum Yöneticisi
        public async Task<Response<CommonResponse>> InviteUser([FromBody] InviteUserDto dto)
        {
            // Manager 'Response<string>' döner.
            var result = await userManager.InviteUserAsync(
                dto.Email,
                dto.FirstName,
                dto.LastName,
                dto.ForceTransfer
            );

            // Response wrapper ile döner (Frontend data.isSuccessful kontrolü yapabilir)
            return result;
        }
    }
}
