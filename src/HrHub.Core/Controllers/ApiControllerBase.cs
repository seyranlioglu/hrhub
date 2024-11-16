using HrHub.Abstraction.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Core.Controllers
{
    [ApiController]
  //  [Authorize]
    [Route("api/[controller]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class ApiControllerBase : ControllerBase
    {
        protected Response<TBody> ProduceResponse<TBody>(TBody body) where TBody : class
        {
            // TODO: Header values
            return Response<TBody>.Success(body);
        }
    }
}
