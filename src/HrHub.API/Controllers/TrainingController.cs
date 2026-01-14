using HrHub.Abstraction.Contracts.Dtos.TrainingDtos;
using HrHub.Abstraction.Result;
using HrHub.Application.Managers.Trainings;
using HrHub.Core.Controllers;
using HrHub.Domain.Contracts.Dtos.DashboardDtos;
using HrHub.Domain.Contracts.Dtos.TrainingDtos;
using HrHub.Domain.Contracts.Responses.CommonResponse;
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
        //[Authorize(Roles = $"{Roles.SuperAdmin},{Roles.User}", Policy = Policies.Instructor)]
        public async Task<Response<ReturnIdResponse>> AddTraining([FromBody] AddTrainingDto data)
        {
            var response = await trainingManager.AddTrainingAsync(data).ConfigureAwait(false);
            return response;
        }

        [HttpPut("[Action]")]
        //[Authorize(Roles = $"{Roles.SuperAdmin},{Roles.User}", Policy = Policies.Instructor)]
        public async Task<Response<CommonResponse>> UpdateTraining([FromBody] UpdateTrainingDto data)
        {
            var response = await trainingManager.UpdateTrainingAsync(data).ConfigureAwait(false);
            return response;
        }

        [HttpDelete("[Action]")]
        //[Authorize(Roles = $"{Roles.SuperAdmin},{Roles.User}", Policy = Policies.Instructor)]
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
        public async Task<Response<GetTrainingDto>> GetByIdAsync(long id)
        {
            return await trainingManager.GetTrainingByIdAsync(id).ConfigureAwait(false);
        }

        [HttpPut("[Action]")]
        // [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.User}", Policy = Policies.Instructor)] // Gerekirse aç
        public async Task<Response<CommonResponse>> ReorderContent([FromBody] ReorderTrainingContentDto data)
        {
            // Frontend'e not: Endpoint URL'i -> api/Training/ReorderContent
            return await trainingManager.ReorderTrainingContentAsync(data).ConfigureAwait(false);
        }

        [HttpGet("[Action]")]
        // [Authorize(Roles = "User")] // Sadece user yetkisi yeterli
        public async Task<Response<IEnumerable<GetMyTrainingDto>>> GetMyTrainings()
        {
            return await trainingManager.GetMyTrainingsAsync().ConfigureAwait(false);
        }

        [HttpGet("[Action]/{id}")]
        // [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.User}", Policy = Policies.Instructor)] // Gerekirse aç
        public async Task<Response<TrainingDetailDto>> GetDetailForUser(long id)
        {
            return await trainingManager.GetTrainingDetailForUserAsync(id).ConfigureAwait(false);
        }

        [HttpGet("[Action]")]
        //[Authorize] // Gerekirse aç
        public async Task<Response<List<TrainingViewCardDto>>> GetRecommendedTrainings()
        {
            var result = await trainingManager.GetRecommendedTrainingsAsync();
            return result;
        }
    }
}
