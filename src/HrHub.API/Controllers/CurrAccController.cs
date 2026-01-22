using HrHub.Abstraction.Result;
using HrHub.Application.Managers.CurrAccManagers;
using HrHub.Domain.Contracts.Dtos.CommonDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HrHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CurrAccController : ControllerBase
    {
        private readonly ICurrAccManager currAccManager;
        public CurrAccController(ICurrAccManager currAccManager)
        {
            this.currAccManager = currAccManager;
        }
        [HttpGet("[Action]")]
        public async Task<Response<List<CommonTypeGetDto>>> GetCurrAccRecs([FromQuery] string filterData)
        {
            var result = await currAccManager.GetCurrAccRecs(filterData);
            return result;
        }
    }
}
