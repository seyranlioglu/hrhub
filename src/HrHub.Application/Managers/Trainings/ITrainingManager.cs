using HrHub.Abstraction.Contracts.Dtos.TrainingDtos;
using HrHub.Abstraction.Result;
using HrHub.Core.Base;
using HrHub.Domain.Contracts.Dtos.TrainingDtos;
using HrHub.Domain.Contracts.Responses.CommonResponse;

namespace HrHub.Application.Managers.Trainings;

public interface ITrainingManager : IBaseManager
{
    Task<Response<ReturnIdResponse>> AddTrainingAsync(AddTrainingDto data, CancellationToken cancellationToken = default);
    Task<Response<CommonResponse>> DeleteTrainingAsync(long id, CancellationToken cancellationToken = default);
    Task<Response<GetTrainingDto>> GetTrainingByIdAsync(long id);
    Task<Response<IEnumerable<GetTrainingDto>>> GetTrainingListAsync();
    Task<Response<CommonResponse>> UpdateTrainingAsync(UpdateTrainingDto dto, CancellationToken cancellationToken = default);
}
