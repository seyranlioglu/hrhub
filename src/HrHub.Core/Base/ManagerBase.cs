using HrHub.Abstraction.Consts;
using HrHub.Abstraction.Exceptions;
using HrHub.Abstraction.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace HrHub.Core.Base
{
    public class ManagerBase
    {
        public readonly IHttpContextAccessor httpContextAccessor;
        private readonly IAuthorizationService authorizationService;
        public ManagerBase(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            var serviceProvider = httpContextAccessor.HttpContext?.RequestServices;
            authorizationService = serviceProvider.GetService<IAuthorizationService>();
        }

        protected Response<TBody> ProduceSuccessResponse<TBody>(TBody body, ResponseHeader header = null) where TBody : class
        {
            // TODO: Header values
            if (header == null)
            {
                return Response<TBody>.Success(body);
            }
            return Response<TBody>.Success(body, header);
        }

        protected Response<TBody> ProduceFailResponse<TBody>(string message, int responseCode) where TBody : class
        {
            // TODO: Header values
            return Response<TBody>.Fail<TBody>(message, responseCode);
        }

        public bool IsAuthenticate()
        {
            return httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public long GetCurrentUserId()
        {

            var user = GetUser();
            return long.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        public ClaimsPrincipal GetUser()
        {
            if (IsAuthenticate())
            {
                return httpContextAccessor.HttpContext.User;
            }
            throw new BusinessException(StatusCodes.Status401Unauthorized, "Unauthorized User");
        }

        public long GetCurrAccId()
        {
            var user = GetUser();
            return Convert.ToInt64(user.FindFirst("CurrAccId").Value);
        }
        public bool IsMainUser()
        {
            return GetUser().HasClaim(Policies.MainUser, "true");
        }

        public bool IsInstructor()
        {
            var user = GetUser();
            var authorizationResult = authorizationService.AuthorizeAsync(user, null, Policies.Instructor).Result;
            return authorizationResult.Succeeded;
        }

        public bool IsSuperAdmin()
        {
            return GetUser().IsInRole(Roles.SuperAdmin);
        }


        public string GetCurrentIpAddress()
        {
            if (IsAuthenticate())
            {
                var remoteIpAddress = httpContextAccessor.HttpContext.Connection.RemoteIpAddress;
                return remoteIpAddress != null ? remoteIpAddress.ToString() : null;
            }


            throw new BusinessException(StatusCodes.Status401Unauthorized, "IpAddress not found!");
        }


    }
}
