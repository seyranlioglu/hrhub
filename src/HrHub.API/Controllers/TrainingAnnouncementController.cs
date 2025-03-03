using HrHub.Abstraction.Consts;
using HrHub.Abstraction.Result;
using HrHub.Application.Managers.TrainingAnnouncementManagers;
using HrHub.Core.Controllers;
using HrHub.Domain.Contracts.Dtos.TrainingAnnouncementDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HrHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingAnnouncementController : ApiControllerBase
    {
        private readonly ITrainingAnnouncementManager trainingAnnouncementManager;

        public TrainingAnnouncementController(ITrainingAnnouncementManager trainingAnnouncementManager)
        {
            this.trainingAnnouncementManager = trainingAnnouncementManager;
        }

        [HttpPost("[Action]")]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.User}")]
        public async Task<Response<CommonResponse>> AddTrainingAnnouncement([FromBody] AddTrainingAnnouncementDto data)
        {
            var response = await trainingAnnouncementManager.AddTrainingAnnouncementAsync(data).ConfigureAwait(false);
            return response;
        }

        [HttpPut("[Action]")]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.User}")]
        public async Task<Response<CommonResponse>> UpdateTrainingAnnouncement([FromBody] UpdateTrainingAnnouncementDto data)
        {
            var response = await trainingAnnouncementManager.UpdateTrainingAnnouncementAsync(data).ConfigureAwait(false);
            return response;
        }

        [HttpDelete("[Action]")]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.User}")]
        public async Task<Response<CommonResponse>> DeleteTrainingAnnouncement([FromBody] long id)
        {
            var response = await trainingAnnouncementManager.DeleteTrainingAnnouncementAsync(id).ConfigureAwait(false);
            return response;
        }

        [HttpGet("[Action]")]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.User}")]
        public async Task<Response<IEnumerable<TrainingAnnouncementDto>>> GetList()
        {
            return await trainingAnnouncementManager.GetTrainingAnnouncementListAsync().ConfigureAwait(false);
        }

        [HttpGet("[Action]")]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.User}")]
        public async Task<Response<TrainingAnnouncementDto>> GetById(long id)
        {
            return await trainingAnnouncementManager.GetTrainingAnnouncementByIdAsync(id).ConfigureAwait(false);
        }
    }
}
