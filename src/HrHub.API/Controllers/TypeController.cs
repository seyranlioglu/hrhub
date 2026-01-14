using HrHub.Application.Helpers;
using HrHub.Application.Managers.TypeManagers;
using HrHub.Domain.Contracts.Dtos.CurrAccTypeDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HrHub.API.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class TypeController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ICurrAccTypeManager _currAccTypeManager;

        public TypeController(IServiceProvider serviceProvider, ICurrAccTypeManager currAccTypeManager)
        {
            _serviceProvider = serviceProvider;
            _currAccTypeManager = currAccTypeManager;
        }

        [HttpGet("[Action]")]
        [AllowAnonymous]
        public async Task<List<GetCurrAccTypeDto>> GetCurrAccTypes()
        {
            var result = await _currAccTypeManager.GetList<GetCurrAccTypeDto>();
            if (result == null)
                return new List<GetCurrAccTypeDto>();

            return result.ToList();
        }

        [HttpPost("[Action]")]
        public async Task<IActionResult> Add([FromQuery] string typeEntity, [FromBody] object requestData)
        {
            var result = await EntityHelper.ExecuteAddAsync(typeEntity, requestData, _serviceProvider);
            return Ok(result);

        }
        [HttpPut("[Action]")]
        public async Task<IActionResult> Update([FromQuery] string typeEntity, [FromBody] object requestData)
        {
            var result = await EntityHelper.ExecuteUpdateAsync(typeEntity, requestData, _serviceProvider);
            return Ok(result);

        }
        [HttpDelete("[Action]")]
        public async Task<IActionResult> Delete([FromQuery] string typeEntity, [FromBody] object requestData)
        {
            var result = await EntityHelper.ExecuteDeleteAsync(typeEntity, requestData, _serviceProvider);
            return Ok(result);

        }
        [HttpPost("[Action]")]
        public async Task<IActionResult> Get([FromQuery] string typeEntity, [FromBody] object requestData)
        {
            var result = await EntityHelper.ExecuteGetAsync(typeEntity, requestData, _serviceProvider);
            return Ok(result);

        }

        [HttpPost("[Action]")]
        public async Task<IActionResult> GetList([FromQuery] string typeEntity, [FromBody] object requestData)
        {
            var result = await EntityHelper.ExecuteGetListAsync(typeEntity, requestData, _serviceProvider);
            return Ok(result);

        }
    }
}
