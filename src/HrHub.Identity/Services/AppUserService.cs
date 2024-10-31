using HrHub.Abstraction.Exceptions;
using HrHub.Identity.Entities;
using HrHub.Identity.Model;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Identity.Services
{
    public class AppUserService : BaseService, IAppUserService
    {
        private readonly IMapper mapper;
        public AppUserService(UserManager<AppUser> userManager,
                              SignInManager<AppUser> signInManager,
                              RoleManager<AppRole> roleManager,
                              IMapper mapper) : base(userManager, signInManager, roleManager)
        {
            this.mapper = mapper;
        }

        public async Task<Tuple<AppUser, IList<Claim>>> GetUserByRefreshToken(string refreshToken)
        {
            Claim claimRefreshToken = new Claim("refreshToken", refreshToken);

            var users = await userManager.GetUsersForClaimAsync(claimRefreshToken);

            if (users.Any())
            {
                var user = users.First();

                IList<Claim> userClaims = await userManager.GetClaimsAsync(user);

                string refreshTokenEndDate = userClaims.First(c => c.Type == "refreshTokenEndDate").Value;

                if (DateTime.Parse(refreshTokenEndDate) > DateTime.Now)
                {

                    return new Tuple<AppUser, IList<Claim>>(user, userClaims);
                }
                else
                {
                    return new Tuple<AppUser, IList<Claim>>(null, null);
                }
            }

            return new Tuple<AppUser, IList<Claim>>(null, null);
        }

        public async Task<AppUser> GetUserByIdAsync(long userId) => await userManager.FindByIdAsync(userId.ToString());

        public async Task<AppUser> GetUserByUserName(string userName) => await userManager.FindByNameAsync(userName);

        public async Task<AppUser> GetUserByEmailAsync(string email) => await userManager.FindByEmailAsync(email);

        public async Task<Tuple<string, bool>> SignUpAsync(SignUpDto userViewModel)
        {
            AppUser user = new AppUser
            {
                UserName = userViewModel.Email,
                Email = userViewModel.Email,
                PhoneNumber = userViewModel.PhoneNumber,
                Name = userViewModel.Name,
                SurName = userViewModel.SurName,
                UserShortName = userViewModel.Name.Substring(0, 1).ToUpper() + userViewModel.SurName.Substring(0, 1).ToUpper(),
                DepartmentId = userViewModel.DepartmentId,
                JobTitleId = userViewModel.JobTitleId,
                RoleId = userViewModel.RoleId,
                EmailConfirmed = false,
                PhoneNumberConfirmed = false,
                LockoutEnabled = true,
                UserTypeId = userViewModel.UserTypeId,
                AdminFlag = userViewModel.AdminFlag,
                IncorrectPinCount = userViewModel.IncorrectPinCount,
                UserStatusId = userViewModel.UserStatusId,
                IsDelete = userViewModel.IsDelete,
                AuthCode = userViewModel.AuthCode,
                CreatedDate = DateTime.UtcNow
            };
            IdentityResult result = await this.userManager.CreateAsync(user, userViewModel.Password);

            if (result.Succeeded)
            {
                return Tuple.Create( "", true );
            }
            else
            {
                return Tuple.Create(result.Errors.FirstOrDefault().Description, false);

                //throw new BusinessException(result.Errors.FirstOrDefault().Description);
            }
        }
        
        public async Task<bool> ResetPasswordAsync(AppUser user,string token,string newPassword)
        {
            var result = await userManager.ResetPasswordAsync(user,token,newPassword);
            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                throw new BusinessException(result.Errors.FirstOrDefault().Description);
            }
        }
        
        public async Task<string> GenerateEmailConfirmationTokenAsync(AppUser user)
        {
            return await this.userManager.GenerateEmailConfirmationTokenAsync(user);
        }
        
        public async Task<bool> ConfirmEmailAsync(AppUser user, string token)
        {
            IdentityResult result = await this.userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                throw new BusinessException(result.Errors.FirstOrDefault().Description);
            }
        }
       
        public async Task<bool> IsLockoutAsync(string userName)
        {
            var user = await GetUserByUserName(userName);
            return user.LockoutEnabled;
        }

        public async Task<bool> UnlockUserAsync(AppUser user)
        {
            var lockoutEnabledResult = await userManager.SetLockoutEnabledAsync(user, false);

            var endDateResult = await userManager.SetLockoutEndDateAsync(user, DateTime.Now - TimeSpan.FromMinutes(1));
            return lockoutEnabledResult.Succeeded && endDateResult.Succeeded;
        }

        public async Task<bool> UpdateUser(UpdateUserDto userViewModel)
        {
            AppUser user = await userManager.FindByNameAsync(userViewModel.Email);

            if ((userManager.Users.Count(u => u.Id != user.Id && u.PhoneNumber == userViewModel.PhoneNumber) > 1))
            {
                throw new BusinessException("Bu telefon numarası başka bir üyeye ait");
            }

            user.PhoneNumber = userViewModel.PhoneNumber;
            user.Name = userViewModel.Name;
            user.SurName = userViewModel.SurName;
            user.Email = userViewModel.Email;
            user.UserName = userViewModel.Email;
            user.RoleId = userViewModel.RoleId;
            user.DepartmentId = userViewModel.DepartmentId;
            user.JobTitleId = userViewModel.JobTitleId;
            user.UserStatusId = userViewModel.UserStatusId;
            user.AdminFlag = userViewModel.AdminFlag;
            user.IncorrectPinCount = userViewModel.IncorrectPinCount;
            user.UserShortName = userViewModel.Name.Substring(0, 1).ToUpper() + userViewModel.SurName.Substring(0, 1).ToUpper();

            IdentityResult result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return result.Succeeded;
            }
            else
            {
                throw new BusinessException(result.Errors.First().Description);

            }
        }
        public async Task<bool> UpdateUser(AppUser userViewModel)
        {

            if ((userManager.Users.Count(u => u.Id != userViewModel.Id && u.PhoneNumber == userViewModel.PhoneNumber) > 1))
            {
                throw new BusinessException("Bu telefon numarası başka bir üyeye ait");
            }

            IdentityResult result = await userManager.UpdateAsync(userViewModel);

            if (result.Succeeded)
            {
                return result.Succeeded;
            }
            else
            {
                throw new BusinessException(result.Errors.First().Description);

            }
        }

        public async Task<bool> DeleteUser(long userId)
        {
            var user = await GetUserByIdAsync(userId);

            var result = await userManager.DeleteAsync(user);

            return result.Succeeded;
        }
    }
}
