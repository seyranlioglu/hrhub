using HrHub.Identity.Abstraction;
using HrHub.Identity.Model;
using HrHub.Identity.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Identity.Security.Token
{
    public class TokenHandler : ITokenHandler
    {
        
        public AccessToken CreateAccessToken(TokenModel model)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(model.ExpirationTime);

            var securityKey = SignHandler.GetSecurityKey(model.SecurityKey);

            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: model.Issuer,
                audience: model.Audience,
                expires: accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: GetClaim(model),
                signingCredentials: signingCredentials
                
                );

            var handler = new JwtSecurityTokenHandler();

            var token = handler.WriteToken(jwtSecurityToken);

            AccessToken accessToken = new AccessToken();

            accessToken.Token = token;
            accessToken.RefreshToken = CreateRefreshToken();
            accessToken.Expiration = accessTokenExpiration;

            return accessToken;
        }

        private string CreateRefreshToken()
        {
            var numberByte = new Byte[32];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(numberByte);

                return Convert.ToBase64String(numberByte);
            }
        }
        private IEnumerable<Claim> GetClaim(TokenModel tokenModel)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,tokenModel.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email,tokenModel.Email),
                new Claim(ClaimTypes.Name,$"{tokenModel.UserName}"),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Acr,tokenModel.Channel),
                new Claim("IsMainUser", tokenModel.IsMainUser.ToString())
            };
            claims.AddRange(tokenModel.Roles.Select(s => new Claim(ClaimTypes.Role, s)));
            return claims;
        }
    }
}
