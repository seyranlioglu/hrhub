using HrHub.Abstraction.Result;
using HrHub.Application.Managers.TrainingSections;
using HrHub.Core.Controllers;
using HrHub.Domain.Contracts.Dtos.TrainingSectionDtos;
using HrHub.Domain.Contracts.Responses.CommonResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HrHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingSectionController : ApiControllerBase
    {
        private readonly ITrainingSectionManager trainingSectionManager;

        public TrainingSectionController(ITrainingSectionManager trainingSectionManager)
        {
            this.trainingSectionManager = trainingSectionManager;
        }

        [HttpPost("[Action]")]
        //[Authorize(Roles = "Admin", Policy = "Instructior")]
        public async Task<Response<ReturnIdResponse>> AddTraining([FromBody] AddTrainingSectionDto data)
        {
            var response = await trainingSectionManager.AddTrainingSectionAsync(data).ConfigureAwait(false);
            return response;
        }

        [HttpPut("[Action]")]
        //[Authorize(Roles = "Admin", Policy = "Instructior")]
        public async Task<Response<CommonResponse>> UpdateTraining([FromBody] UpdateTrainingSectionDto data)
        {
            var response = await trainingSectionManager.UpdateTrainingSectionAsync(data).ConfigureAwait(false);
            return response;
        }

        [HttpDelete("[Action]")]
        //[Authorize(Roles = "Admin", Policy = "Instructior")]
        public async Task<Response<CommonResponse>> DeleteTraining([FromBody] long id)
        {
            var response = await trainingSectionManager.DeleteTrainingSectionAsync(id).ConfigureAwait(false);
            return response;
        }

        [HttpGet("[Action]")]
        public async Task<Response<IEnumerable<GetTrainingSectionDto>>> GetListAsync()
        {
            return await trainingSectionManager.GetTrainingSectionListAsync().ConfigureAwait(false);
        }

        [HttpGet("[Action]")]
        public async Task<Response<GetTrainingSectionDto>> GetByIdAsync([FromRoute] long id)
        {
            return await trainingSectionManager.GetTrainingSectionByIdAsync(id).ConfigureAwait(false);
        }
    }
}
