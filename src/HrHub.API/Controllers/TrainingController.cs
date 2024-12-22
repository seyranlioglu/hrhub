using HrHub.Abstraction.Consts;
using HrHub.Abstraction.Contracts.Dtos.TrainingDtos;
using HrHub.Abstraction.Result;
using HrHub.Application.Managers.ContentTypes;
using HrHub.Application.Managers.ExamOperationManagers;
using HrHub.Application.Managers.Trainings;
using HrHub.Core.Controllers;
using HrHub.Domain.Contracts.Dtos.ContentTypes;
using HrHub.Domain.Contracts.Dtos.ExamDtos;
using HrHub.Domain.Contracts.Dtos.TrainingDtos;
using HrHub.Domain.Contracts.Responses.CommonResponse;
using HrHub.Domain.Contracts.Responses.ExamResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HrHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingController : ApiControllerBase
    {
        private readonly ITrainingManager trainingManager;

        public TrainingController(ITrainingManager trainingManager)
        {
            this.trainingManager = trainingManager;
        }

        [HttpPost("[Action]")]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.User}", Policy = Policies.Instructor)]
        public async Task<Response<ReturnIdResponse>> AddTraining([FromBody] AddTrainingDto data)
        {
            var response = await trainingManager.AddTrainingAsync(data).ConfigureAwait(false);
            return response;
        }

        [HttpPut("[Action]")]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.User}", Policy = Policies.Instructor)]
        public async Task<Response<CommonResponse>> UpdateTraining([FromBody] UpdateTrainingDto data)
        {
            var response = await trainingManager.UpdateTrainingAsync(data).ConfigureAwait(false);
            return response;
        }

        [HttpDelete("[Action]")]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.User}", Policy = Policies.Instructor)]
        public async Task<Response<CommonResponse>> DeleteTraining([FromBody] long id)
        {
            var response = await trainingManager.DeleteTrainingAsync(id).ConfigureAwait(false);
            return response;
        }
        
        [HttpGet("[Action]")]
        public async Task<Response<IEnumerable<GetTrainingDto>>> GetListAsync()
        {
            return await trainingManager.GetTrainingListAsync().ConfigureAwait(false);
        }

        [HttpGet("[Action]")]
        public async Task<Response<GetTrainingDto>> GetAsync([FromRoute] long id)
        {
            return await trainingManager.GetTrainingByIdAsync(id).ConfigureAwait(false);
        }
    }
}
