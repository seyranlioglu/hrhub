using HrHub.Abstraction.Consts;
using HrHub.Abstraction.Exceptions;
using HrHub.Abstraction.Result;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Core.Base
{
    public class ManagerBase
    {
        public readonly IHttpContextAccessor httpContextAccessor;

        public ManagerBase(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
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
            return Convert.ToInt64(user.FindFirst(ClaimTypes.NameIdentifier).Value);
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
            return GetUser().HasClaim("Policy", Policies.MainUser);
        }

        public bool IsInstructor()
        {
            return GetUser().HasClaim("Policy", Policies.Instructor);
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
