using HrHub.Abstraction.Enums;
using HrHub.Abstraction.Result;
using HrHub.Core.Base;
using HrHub.Domain.Contracts.Dtos.UserDtos;
using HrHub.Domain.Contracts.Responses.UserResponses;

namespace HrHub.Application.Managers.UserManagers
{
    public interface IUserManager : IBaseManager
    {
        Task<Response<UserSignUpResponse>> SignUp(UserSignUpDto data, CancellationToken cancellationToken = default);
        Task<Response<UserSignInResponse>> SignIn(UserSignInDto data, CancellationToken cancellationToken = default);
        Task<Response<VerifySendResponse>> VerifyCodeSend(VerifySendDto verifySendDto, CancellationToken cancellationToken = default);
        Task<Response<VerifyResponse>> VerifyCodeAndConfirm(VerifyDto verify, CancellationToken cancellationToken = default);
        Task<Response<VerifySignInResponse>> VerifyCodeAndSignIn(VerifySignInDto verify, CancellationToken cancellationToken = default);

    }
}
