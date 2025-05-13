using HrHub.Abstraction.Result;
using HrHub.Domain.Contracts.Dtos.TrainingAnnouncementCommentDtos;

namespace HrHub.Application.Managers.TrainingAnnouncementCommentManagers
{
    public interface ITrainingAnnouncementCommentManager
    {
        Task<Response<CommonResponse>> AddTrainingAnnouncementCommentAsync(AddTrainingAnnouncementCommentDto data, CancellationToken cancellationToken = default);
        Task<Response<CommonResponse>> DeleteTrainingAnnouncementCommentAsync(long id, CancellationToken cancellationToken = default);
        Task<Response<TrainingAnnouncementCommentDto>> GetTrainingAnnouncementCommentByIdAsync(long id);
        Task<Response<IEnumerable<TrainingAnnouncementCommentDto>>> GetTrainingAnnouncementCommentListAsync();
        Task<Response<CommonResponse>> UpdateTrainingAnnouncementCommentAsync(UpdateTrainingAnnouncementCommentDto dto, CancellationToken cancellationToken = default);
    }
}
