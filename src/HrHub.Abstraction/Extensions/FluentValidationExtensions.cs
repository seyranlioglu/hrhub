using HrHub.Abstraction.Result;
using HrHub.Abstraction.StatusCodes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Abstraction.Extensions
{
    public static class FluentValidationExtensions
    {
        public static Response<TResponse> SendResponse<TResponse>(this FluentValidation.Results.ValidationResult validationResult) where TResponse : class
        {
            string message = JsonConvert.SerializeObject(validationResult.Errors
                .Select(s => new
            {
                    Code = s.ErrorCode,
                    Message = s.ErrorMessage
            }));
            return Response<TResponse>.Fail<TResponse>(message, HrStatusCodes.Status119ValidationError);
        }
    }
}
