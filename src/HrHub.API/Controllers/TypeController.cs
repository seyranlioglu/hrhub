using HrHub.Application.Helpers;
using Microsoft.AspNetCore.Mvc;

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
            var result = await EntityHelper.ExecuteAddAsync(typeEntity, requestData, _serviceProvider);
            return Ok(result);

        }
    }
}
