using HrHub.Identity.Entities;
using HrHub.Identity.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Identity.Services
{
    public interface IAppUserService
    {
        Task<bool> UpdateUser(UpdateUserDto userViewModel);
        Task<AppUser> GetUserByUserName(string userName);
        Task<Tuple<AppUser, IList<Claim>>> GetUserByRefreshToken(string refreshToken);
        Task<Tuple<string, bool>> SignUpAsync(SignUpDto userViewModel);
        Task<string> GenerateEmailConfirmationTokenAsync(AppUser user);
        Task<AppUser> GetUserByIdAsync(long userId);
        Task<bool> UnlockUserAsync(AppUser user);
        Task<bool> DeleteUser(long userId);
        Task<bool> IsLockoutAsync(string userName);
        Task<bool> ConfirmEmailAsync(AppUser user, string token);
        Task<bool> ResetPasswordAsync(AppUser user, string token, string newPassword);
        Task<AppUser> GetUserByEmailAsync(string email);
        Task<bool> UpdateUser(AppUser userViewModel);
    }
}
