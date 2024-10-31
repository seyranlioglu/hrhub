using HrHub.Abstraction.Exceptions;
using HrHub.Identity.Abstraction;
using HrHub.Identity.Entities;
using HrHub.Identity.Helpers;
using HrHub.Identity.Model;
using HrHub.Identity.Options;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HrHub.Abstraction.Result;

namespace HrHub.Identity.Services
{
    public class AuthenticationService : BaseService, IAuthenticationService
    {
        private readonly ITokenHandler tokenHandler;
        private readonly AsisTokenOptions tokenOptions;
        private readonly IAppUserService userService;
        private readonly IMapper mapper;
        public AuthenticationService(IAppUserService userService,
                                     ITokenHandler tokenHandler,
                                     IOptions<AsisTokenOptions> tokenOptions,
                                     UserManager<AppUser> userManager,
                                     SignInManager<AppUser> signInManager,
                                     RoleManager<AppRole> roleManager,
                                     IMapper mapper) : base(userManager, signInManager, roleManager)
        {
            this.tokenHandler = tokenHandler;
            this.userService = userService;
            this.tokenOptions = tokenOptions.Value;
            this.mapper = mapper;
        }


        public async Task<Tuple<AppUser, bool>> CheckUserAsync(string userName, string password)
        {
            AppUser user = await userManager.FindByEmailAsync(userName);

            if (user != null)
            {
                bool isUser = await userManager.CheckPasswordAsync(user, password);
                return Tuple.Create(user, isUser);
            }

            throw new BusinessException("Kulanıcı Bulunamadı!");
        }
        public async Task<bool> CheckPasswordAsync(AppUser user,string password)
        {
            return await userManager.CheckPasswordAsync(user, password);
        }
        public async Task<bool> ResetPasswordAsync(AppUser user,string token,string newPassword)
        {
            var result = await userManager.ResetPasswordAsync(user,token,newPassword);
            if (result.Succeeded)
                return true;
            throw new BusinessException("Şifre Değiştirilemedi.");

        }
        public async Task<AccessToken> SignIn(SignInViewModelResource signInViewModel)
        {
            AppUser user = await userManager.FindByNameAsync(signInViewModel.Email);

            if (user != null)
            {
                bool isUser = await userManager.CheckPasswordAsync(user, signInViewModel.Password);
                if (isUser)
                {
                    AccessToken accessToken = tokenHandler.CreateAccessToken(new TokenModel 
                    { 
                        Channel = "",
                        Email = user.Email,
                        UserId = user.Id,
                        UserName = user.UserName,
                        Audience = TokenHelper.TokenOptions.Audience,
                        ExpirationTime = TokenHelper.TokenOptions.AccessTokenExpiration,
                        Issuer = TokenHelper.TokenOptions.Issuer,
                        SecurityKey = TokenHelper.TokenOptions.SecurityKey
                    });

                    Claim refreshTokenClaim = new Claim("refreshToken", accessToken.RefreshToken);
                    Claim refreshTokenEndDateClaim = new Claim("refreshTokenEndDate", DateTime.Now.AddMinutes(TokenHelper.TokenOptions.RefreshTokenExpiration).ToString());

                    List<Claim> refreshClaimList = userManager.GetClaimsAsync(user).Result.Where(c => c.Type.Contains("refreshToken")).ToList();

                    if (refreshClaimList.Any())
                    {
                        await userManager.ReplaceClaimAsync(user, refreshClaimList[0], refreshTokenClaim);
                        await userManager.ReplaceClaimAsync(user, refreshClaimList[1], refreshTokenEndDateClaim);
                    }
                    else
                    {
                        await userManager.AddClaimsAsync(user, new[] { refreshTokenClaim, refreshTokenEndDateClaim });
                    }

                    return accessToken;
                }
            }
            return null;
        }
        public async Task<AccessToken> SignInWithAuthKey(string authKey)
        {
            AppUser user =  userManager.Users.FirstOrDefault(s=>s.AuthCode== authKey);

            if (user != null)
            {
                
                    AccessToken accessToken = tokenHandler.CreateAccessToken(new TokenModel
                    {
                        Channel = "",
                        Email = user.Email,
                        UserId = user.Id,
                        UserName = user.UserName,
                        Audience = TokenHelper.TokenOptions.Audience,
                        ExpirationTime = TokenHelper.TokenOptions.AccessTokenExpiration,
                        Issuer = TokenHelper.TokenOptions.Issuer,
                        SecurityKey = TokenHelper.TokenOptions.SecurityKey
                    });

                    Claim refreshTokenClaim = new Claim("refreshToken", accessToken.RefreshToken);
                    Claim refreshTokenEndDateClaim = new Claim("refreshTokenEndDate", DateTime.Now.AddMinutes(TokenHelper.TokenOptions.RefreshTokenExpiration).ToString());

                    List<Claim> refreshClaimList = userManager.GetClaimsAsync(user).Result.Where(c => c.Type.Contains("refreshToken")).ToList();

                    if (refreshClaimList.Any())
                    {
                        await userManager.ReplaceClaimAsync(user, refreshClaimList[0], refreshTokenClaim);
                        await userManager.ReplaceClaimAsync(user, refreshClaimList[1], refreshTokenEndDateClaim);
                    }
                    else
                    {
                        await userManager.AddClaimsAsync(user, new[] { refreshTokenClaim, refreshTokenEndDateClaim });
                    }

                    return accessToken;
                
            }
            return null;
        }

        public async Task<string> GeneratePasswordResetTokenAsync(AppUser user)
        {
            return await userManager.GeneratePasswordResetTokenAsync(user);
        }
        public async Task<Response<AccessToken>> CreateAccessTokenByRefreshToken(RefreshTokenViewModelResource refreshTokenViewModel)
        {
            var userClaim = await userService.GetUserByRefreshToken(refreshTokenViewModel.RefreshToken);
            if (userClaim.Item1 != null)
            {
                AccessToken accessToken = tokenHandler.CreateAccessToken(new TokenModel
                {

                    Channel = "",
                    Email = userClaim.Item1.Email,
                    UserId = userClaim.Item1.Id,
                    UserName = userClaim.Item1.UserName,
                    Audience=TokenHelper.TokenOptions.Audience,
                    ExpirationTime= TokenHelper.TokenOptions.AccessTokenExpiration,
                    Issuer= TokenHelper.TokenOptions.Issuer,
                    SecurityKey= TokenHelper.TokenOptions.SecurityKey
                });

                Claim refreshTokenClaim = new Claim("refreshToken", accessToken.RefreshToken);
                Claim refreshTokenEndDateClaim = new Claim("refreshTokenEndDate", DateTime.Now.AddHours(TokenHelper.TokenOptions.RefreshTokenExpiration).ToString());

                await userManager.ReplaceClaimAsync(userClaim.Item1, userClaim.Item2[0], refreshTokenClaim);
                await userManager.ReplaceClaimAsync(userClaim.Item1, userClaim.Item2[1], refreshTokenEndDateClaim);

                return Response<AccessToken>.Success(accessToken);
            }
            else
            {
                return Response<AccessToken>.Fail<AccessToken>("Invalid access token or refresh token", (int)HttpStatusCode.BadRequest);
            }
        }
        public async Task<bool> RevokeRefrefreshToken(string refreshToken)
        {

            var result = await userService.GetUserByRefreshToken(refreshToken);

            if (result.Item1 == null) return false;

            IdentityResult response = await userManager.RemoveClaimsAsync(result.Item1, result.Item2);

            if (response.Succeeded)
            {
                return true;
            }

            return false;
        }
        public Task ForbidAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> RevokeRefreshToken(RefreshTokenViewModelResource refreshTokenViewModel)
        {
            bool result = await RevokeRefrefreshToken(refreshTokenViewModel.RefreshToken);

            if (result)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Task SignInAsync(HttpContext context, string? scheme, ClaimsPrincipal principal, AuthenticationProperties? properties)
        {
            throw new NotImplementedException();
        }
        public async Task SignOutAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
        {
            await signInManager.SignOutAsync();
            throw new NotImplementedException();
        }

        //public JwtAuthResult Refresh(string refreshToken, string accessToken, DateTime now)
        //{
        //    var (principal, jwtToken) = DecodeJwtToken(accessToken);
        //    if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature))
        //    {
        //        throw new SecurityTokenException("Invalid token");
        //    }

        //    var userName = principal.Identity.Name;
        //    if (!_usersRefreshTokens.TryGetValue(refreshToken, out var existingRefreshToken))
        //    {
        //        throw new SecurityTokenException("Invalid token");
        //    }
        //    if (existingRefreshToken.UserName != userName || existingRefreshToken.ExpireAt < now)
        //    {
        //        throw new SecurityTokenException("Invalid token");
        //    }

        //    return GenerateTokens(userName, principal.Claims.ToArray(), now); // need to recover the original claims
        //}

        //public (ClaimsPrincipal, JwtSecurityToken) DecodeJwtToken(string token)
        //{
        //    if (string.IsNullOrWhiteSpace(token))
        //    {
        //        throw new SecurityTokenException("Invalid token");
        //    }
        //    var principal = new JwtSecurityTokenHandler()
        //        .ValidateToken(token,
        //            new TokenValidationParameters
        //            {
        //                ValidateIssuer = true,
        //                ValidIssuer = _jwtTokenConfig.Issuer,
        //                ValidateIssuerSigningKey = true,
        //                IssuerSigningKey = new SymmetricSecurityKey(_secret),
        //                ValidAudience = _jwtTokenConfig.Audience,
        //                ValidateAudience = true,
        //                ValidateLifetime = true,
        //                ClockSkew = TimeSpan.FromMinutes(1)
        //            },
        //            out var validatedToken);
        //    return (principal, validatedToken as JwtSecurityToken);
        //}

    }
}
