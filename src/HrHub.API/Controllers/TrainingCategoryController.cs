using HrHub.Abstraction.Result;
using HrHub.Application.Managers.TrainingCategories;
using HrHub.Application.Managers.TrainingContentManagers;
using HrHub.Domain.Contracts.Dtos.TrainingCategoryDtos;
using HrHub.Domain.Contracts.Dtos.TrainingContentDtos;
using HrHub.Domain.Contracts.Responses.CommonResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HrHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingCategoryController : ControllerBase
    {
        private readonly ITrainingCategoryManager trainingCategoryManager;

        public TrainingCategoryController(ITrainingCategoryManager trainingCategoryManager)
        {
            this.trainingCategoryManager = trainingCategoryManager;
        }

        [HttpPost("[Action]")]
        //[Authorize(Roles = "Admin", Policy = "Instructior")]
        public async Task<Response<CommonResponse>> AddTrainingCategory([FromBody] AddTrainingCategoryDto data)
        {
            var response = await trainingCategoryManager.AddTrainingCategoryAsync(data).ConfigureAwait(false);
            return response;
        }

        [HttpPut("[Action]")]
        //[Authorize(Roles = "Admin", Policy = "Instructior")]
        public async Task<Response<CommonResponse>> UpdateTrainingCategory([FromBody] UpdateTrainingCategoryDto data)
        {
            var response = await trainingCategoryManager.UpdateTrainingCategoryAsync(data).ConfigureAwait(false);
            return response;
        }

        [HttpDelete("[Action]")]
        //[Authorize(Roles = "Admin", Policy = "Instructior")]
        public async Task<Response<CommonResponse>> DeleteTrainingCategory([FromBody] long id)
        {
            var response = await trainingCategoryManager.DeleteTrainingCategoryAsync(id).ConfigureAwait(false);
            return response;
        }

        [HttpGet("[Action]")]
        public async Task<Response<IEnumerable<GetTrainingCategoryDto>>> GetListAsync()
        {
            return await trainingCategoryManager.GetListTrainingCategoryAsync().ConfigureAwait(false);
        }

        [HttpGet("[Action]")]
        public async Task<Response<GetTrainingCategoryDto>> GetByIdAsync(long id)
        {
            return await trainingCategoryManager.GetTrainingCategoryAsync(id).ConfigureAwait(false);
        }
    }
}