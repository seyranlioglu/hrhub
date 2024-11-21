using HrHub.Abstraction.Contracts.Dtos.ContentTypes;
using HrHub.Abstraction.Enums;
using HrHub.Application.Helpers;
using HrHub.Application.Managers.TypeManagers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Reflection;

namespace HrHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;

        public TypeController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        [HttpPost("[Action]")]
        public async Task<IActionResult> Add([FromQuery] string typeEntity, [FromBody] object requestData)
        {
            var result =await  EntityHelper.ExecuteAddAsync(typeEntity, requestData, _serviceProvider);
            return Ok(result);

        }
    }
}
