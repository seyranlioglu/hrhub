using HrHub.Abstraction.Consts;
using HrHub.Abstraction.Result;
using HrHub.Application.Managers.TrainingAnnouncementCommentManagers;
using HrHub.Core.Controllers;
using HrHub.Domain.Contracts.Dtos.TrainingAnnouncementCommentDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HrHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingAnnouncementCommentController : ApiControllerBase
    {
        private readonly ITrainingAnnouncementCommentManager trainingAnnouncementCommentManager;

        public TrainingAnnouncementCommentController(ITrainingAnnouncementCommentManager trainingAnnouncementCommentManager)
        {
            this.trainingAnnouncementCommentManager = trainingAnnouncementCommentManager;
        }

        [HttpPost("[Action]")]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.User}")]
        public async Task<Response<CommonResponse>> AddTrainingAnnouncementComment([FromBody] AddTrainingAnnouncementCommentDto data)
        {
            var response = await trainingAnnouncementCommentManager.AddTrainingAnnouncementCommentAsync(data).ConfigureAwait(false);
            return response;
        }

        [HttpPut("[Action]")]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.User}")]
        public async Task<Response<CommonResponse>> UpdateTrainingAnnouncementComment([FromBody] UpdateTrainingAnnouncementCommentDto data)
        {
            var response = await trainingAnnouncementCommentManager.UpdateTrainingAnnouncementCommentAsync(data).ConfigureAwait(false);
            return response;
        }

        [HttpDelete("[Action]")]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.User}")]
        public async Task<Response<CommonResponse>> DeleteTrainingAnnouncementComment([FromBody] long id)
        {
            var response = await trainingAnnouncementCommentManager.DeleteTrainingAnnouncementCommentAsync(id).ConfigureAwait(false);
            return response;
        }

        [HttpGet("[Action]")]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.User}")]
        public async Task<Response<IEnumerable<TrainingAnnouncementCommentDto>>> GetList()
        {
            return await trainingAnnouncementCommentManager.GetTrainingAnnouncementCommentListAsync().ConfigureAwait(false);
        }

        [HttpGet("[Action]")]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.User}")]
        public async Task<Response<TrainingAnnouncementCommentDto>> GetById(long id)
        {
            return await trainingAnnouncementCommentManager.GetTrainingAnnouncementCommentByIdAsync(id).ConfigureAwait(false);
        }
    }
}
