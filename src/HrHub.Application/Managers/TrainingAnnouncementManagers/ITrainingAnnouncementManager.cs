using HrHub.Abstraction.Result;
using HrHub.Domain.Contracts.Dtos.TrainingAnnouncementDtos;

namespace HrHub.Application.Managers.TrainingAnnouncementManagers
{
    public interface ITrainingAnnouncementManager
    {
        Task<Response<CommonResponse>> AddTrainingAnnouncementAsync(AddTrainingAnnouncementDto data, CancellationToken cancellationToken = default);
        Task<Response<CommonResponse>> DeleteTrainingAnnouncementAsync(long id, CancellationToken cancellationToken = default);
        Task<Response<TrainingAnnouncementDto>> GetTrainingAnnouncementByIdAsync(long id);
        Task<Response<IEnumerable<TrainingAnnouncementDto>>> GetTrainingAnnouncementListAsync();
        Task<Response<CommonResponse>> UpdateTrainingAnnouncementAsync(UpdateTrainingAnnouncementDto dto, CancellationToken cancellationToken = default);
    }
}
