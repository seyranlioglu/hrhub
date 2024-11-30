using HrHub.Abstraction.Result;
using HrHub.Domain.Contracts.Dtos.TrainingSectionDtos;
using HrHub.Domain.Contracts.Responses.CommonResponse;

namespace HrHub.Application.Managers.TrainingSections
{
    public interface ITrainingSectionManager
    {
        Task<Response<ReturnIdResponse>> AddTrainingSectionAsync(AddTrainingSectionDto data, CancellationToken cancellationToken = default);
        Task<Response<CommonResponse>> DeleteTrainingSectionAsync(long id, CancellationToken cancellationToken = default);
        Task<Response<GetTrainingSectionDto>> GetTrainingSectionByIdAsync(long id);
        Task<Response<IEnumerable<GetTrainingSectionDto>>> GetTrainingSectionListAsync();
        Task<Response<CommonResponse>> UpdateTrainingSectionAsync(UpdateTrainingSectionDto dto, CancellationToken cancellationToken = default);
    }
}
