using HrHub.Abstraction.Enums;
using HrHub.Abstraction.Result;
using HrHub.Application.Helpers;
using HrHub.Core.Base;
using HrHub.Core.Data.UnitOfWork;
using HrHub.Core.HrFluentValidation;
using HrHub.Core.Utilties.Encryption;
using HrHub.Domain.Contracts.Dtos.UserDtos;
using HrHub.Domain.Contracts.Responses.UserResponses;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Identity.Services;
using HrHub.Infrastructre.Repositories.Concrete;
using Microsoft.AspNetCore.Http;

namespace HrHub.Application.Managers.UserManagers
{
    public interface IUserManager : IBaseManager
    {
        bool IsMainUser();
        Task<Response<UserSignUpResponse>> SignUp(UserSignUpDto data, CancellationToken cancellationToken = default);
        Task<Response<UserSignInResponse>> SignIn(UserSignInDto data, CancellationToken cancellationToken = default);
        Task<Response<VerifySendResponse>> VerifyCodeSend(VerifySendDto verifySendDto, CancellationToken cancellationToken = default);
        Task<Response<VerifyResponse>> VerifyCodeAndConfirm(VerifyDto verify, CancellationToken cancellationToken = default);
        Task<Response<VerifySignInResponse>> VerifyCodeAndSignIn(VerifySignInDto verify, CancellationToken cancellationToken = default);
        Task<Response<CommonResponse>> AddUser(AddUserDto verify, CancellationToken cancellationToken = default);
        Task<Response<CommonResponse>> ChangePassword(ChangePasswordDto changePassword, CancellationToken cancellationToken = default);
        Task<Response<CommonResponse>> VerifyCodeAndChangePassword(VerifyChangePasswordDto verify, CancellationToken cancellationToken = default);
        Task<Response<CommonResponse>> ForgotPassword(ForgotPasswordDto forgotPassword, CancellationToken cancellationToken = default);
        Task<Response<CommonResponse>> VerifyCodeAndForgotPassword(VerifyForgotPasswordDto verify, CancellationToken cancellationToken = default);
        Task<Response<CommonResponse>> PasswordReset(PasswordResetDto passwordReset, string reason, bool isSendMail = false, CancellationToken cancellationToken = default);
        Task<Response<GetUserResponse>> GetUserById(GetUserByIdDto getUserById, CancellationToken cancellationToken = default);
        Task<Response<List<GetUserResponse>>> GetUserList(CancellationToken cancellationToken = default);
        Task<Response<CommonResponse>> SetUserStatus(SetUserStatusDto setUserStatusDto, CancellationToken cancellationToken = default);
        Task<Response<CommonResponse>> UpdateUser(UserUpdateDto updateUserDto, CancellationToken cancellationToken = default);
        Task<Response<CommonResponse>> DeleteUser(long userId, CancellationToken cancellationToken = default);
    }
}
