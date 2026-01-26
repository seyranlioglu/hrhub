using HrHub.Abstraction.Result;
using HrHub.Core.Base;
using HrHub.Domain.Contracts.Dtos.DashboardDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Application.Managers.DashboardManagers
{
    public interface IDashboardManager : IBaseManager
    {
        Task<Response<ContinueTrainingDto>> GetLastActiveTrainingAsync();
        Task<Response<DashboardStatsDto>> GetUserStatsAsync();
        Task<Response<List<TrainingCardDto>>> GetAssignedTrainingsAsync();
        Task<Response<List<TrainingViewCardDto>>> GetRecommendedTrainingsAsync();
    }
}
