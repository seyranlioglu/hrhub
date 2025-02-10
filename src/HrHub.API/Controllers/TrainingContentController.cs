using HrHub.Abstraction.Contracts.Dtos.TrainingDtos;
using HrHub.Abstraction.Result;
using HrHub.Application.Managers.TrainingContentManagers;
using HrHub.Application.Managers.Trainings;
using HrHub.Core.Controllers;
using HrHub.Domain.Contracts.Dtos.TrainingContentDtos;
using HrHub.Domain.Contracts.Dtos.TrainingDtos;
using HrHub.Domain.Contracts.Responses.CommonResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HrHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [RequestSizeLimit(5L * 1024 * 1024 * 1024)] // Video boyutu için 5gb olacak şekilde ayarlandı*** 

    public class TrainingContentController : ControllerBase
    {
        private readonly ITrainingContentManager trainingContentManager;

        public TrainingContentController(ITrainingContentManager trainingContentManager)
        {
            this.trainingContentManager = trainingContentManager;
        }
        [HttpPost("[Action]")]
        public async Task<Response<ReturnIdResponse>> AddTrainingContent([FromForm] AddTrainingContentDto data)
        {
            var response = await trainingContentManager.AddTrainingContentAsync(data).ConfigureAwait(false);
            return response;
        }


        [HttpPut("[Action]")]
        // [Authorize(Roles = "Admin", Policy = "Instructior")]
        public async Task<Response<CommonResponse>> UpdateTrainingContent([FromForm] UpdateTrainingContentDto data)
        {
            var response = await trainingContentManager.UpdateTrainingContentAsync(data).ConfigureAwait(false);
            return response;
        }

        [HttpDelete("[Action]")]
        //[Authorize(Roles = "Admin", Policy = "Instructior")]
        public async Task<Response<CommonResponse>> DeleteTrainingContent([FromBody] long id)
        {
            var response = await trainingContentManager.DeleteTrainingContentAsync(id).ConfigureAwait(false);
            return response;
        }

        [HttpGet("[Action]")]
        public async Task<Response<IEnumerable<GetListTrainingContentDto>>> GetListAsync()
        {
            return await trainingContentManager.GetTrainingContentListAsync().ConfigureAwait(false);
        }

        [HttpGet("[Action]")]
        public async Task<Response<GetTrainingContentDto>> GetByIdAsync([FromQuery] long id)
        {
            return await trainingContentManager.GetTrainingContentAsync(id).ConfigureAwait(false);
        }
    }
}
