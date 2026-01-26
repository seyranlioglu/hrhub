using HrHub.Abstraction.Consts; // Roles ve Policies için
using HrHub.Abstraction.Result;
using HrHub.Application.Managers.DashboardManagers;
using HrHub.Core.Controllers;
using HrHub.Domain.Contracts.Dtos.DashboardDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ApiControllerBase
    {
        private readonly IDashboardManager dashboardManager;

        public DashboardController(IDashboardManager dashboardManager)
        {
            this.dashboardManager = dashboardManager;
        }

        [HttpGet("continue-learning")]
        public async Task<Response<ContinueTrainingDto>> GetContinueLearning()
        {
            // Response<T> formatına uygun dönüş (CreateActionResult yerine direkt dönüş)
            // TrainingController örneğinde böyle kullanılmış.
            return await dashboardManager.GetLastActiveTrainingAsync().ConfigureAwait(false);
        }

        [HttpGet("stats")]
        public async Task<Response<DashboardStatsDto>> GetStats()
        {
            return await dashboardManager.GetUserStatsAsync().ConfigureAwait(false);
        }

        [HttpGet("assigned-trainings")]
        public async Task<Response<List<TrainingCardDto>>> GetAssignedTrainings()
        {
            return await dashboardManager.GetAssignedTrainingsAsync().ConfigureAwait(false);
        }

        [HttpGet("recommended-trainings")]
        public async Task<Response<List<TrainingViewCardDto>>> GetRecommendedTrainings()
        {
            return await dashboardManager.GetRecommendedTrainingsAsync().ConfigureAwait(false);
        }
    }
}