using HrHub.Abstraction.Result;
using HrHub.Application.Managers.CurrAccTrainingManagers;
using HrHub.Core.Controllers;
using HrHub.Domain.Contracts.Dtos.CurrAccTrainingDtos;
using HrHub.Domain.Contracts.Responses.CommonResponse;
using Microsoft.AspNetCore.Mvc;

namespace HrHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrAccTrainingController : ApiControllerBase
    {
        private readonly ICurrAccTrainingManager currAccTrainingManager;
        public CurrAccTrainingController(ICurrAccTrainingManager currAccTrainingManager)
        {
            this.currAccTrainingManager = currAccTrainingManager;
        }
        [HttpPost("[Action]")]
        public async Task<Response<ReturnIdResponse>> AddCurrAccTraining([FromBody] AddCurrAccTrainingDto data)
        {
            var response = await currAccTrainingManager.AddCurrAccTrainingAsync(data).ConfigureAwait(false);
            return response;
        }
        [HttpPut("[Action]")]
        public async Task<Response<CommonResponse>> UpdateCurrAccTraining([FromBody] UpdateCurrAccTrainingDto data)
        {
            var response = await currAccTrainingManager.UpdateCurrAccTrainingAsync(data).ConfigureAwait(false);
            return response;
        }
        [HttpDelete("[Action]")]
        public async Task<Response<CommonResponse>> DeleteCurrAccTraining([FromBody] DeleteCurrAccTrainingDto deleteCurrAccTrainingDto)
        {
            var response = await currAccTrainingManager.DeleteCurrAccTrainingAsync(deleteCurrAccTrainingDto).ConfigureAwait(false);
            return response;
        }
        [HttpGet("[Action]")]
        public async Task<Response<IEnumerable<GetCurrAccTrainingDto>>> GetList()
        {
            return await currAccTrainingManager.GetAllCurrAccTrainingsAsync().ConfigureAwait(false);
        }
        [HttpGet("[Action]")]
        public async Task<Response<GetCurrAccTrainingDto>> GetById(long id)
        {
            return await currAccTrainingManager.GetCurrAccTrainingByIdAsync(id).ConfigureAwait(false);
        }
    }
}
