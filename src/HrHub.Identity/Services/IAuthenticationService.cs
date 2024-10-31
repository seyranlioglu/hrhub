using HrHub.Abstraction.Result;
using HrHub.Identity.Entities;
using HrHub.Identity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Identity.Services
{
    public interface IAuthenticationService
    {
        Task<Tuple<AppUser, bool>> CheckUserAsync(string userName, string password);
        Task<Response<AccessToken>> CreateAccessTokenByRefreshToken(RefreshTokenViewModelResource refreshTokenViewModel);
        Task<bool> RevokeRefreshToken(RefreshTokenViewModelResource refreshTokenViewModel);
        Task<AccessToken> SignIn(SignInViewModelResource signInViewModel);
        Task<string> GeneratePasswordResetTokenAsync(AppUser user);
        Task<bool> CheckPasswordAsync(AppUser user, string password);
        Task<bool> ResetPasswordAsync(AppUser user, string token, string newPassword);
        Task<AccessToken> SignInWithAuthKey(string authKey);
    }
}
