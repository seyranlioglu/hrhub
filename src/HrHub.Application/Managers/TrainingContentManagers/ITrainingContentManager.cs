using HrHub.Abstraction.Result;
using HrHub.Core.Base;
using HrHub.Domain.Contracts.Dtos.TrainingContentDtos;
using HrHub.Domain.Contracts.Responses.CommonResponse;
using HrHub.Domain.Contracts.Responses.TrainingContentResponse;

namespace HrHub.Application.Managers.TrainingContentManagers
{
    public interface ITrainingContentManager : IBaseManager
    {
        Task<Response<ReturnIdResponse>> AddTrainingContentAsync(AddTrainingContentDto data, CancellationToken cancellationToken = default);
        Task<Response<CommonResponse>> DeleteTrainingContentAsync(long id, CancellationToken cancellationToken = default);
        Task<Response<GetTrainingContentDto>> GetTrainingContentAsync(long id);
        Task<Response<IEnumerable<GetListTrainingContentDto>>> GetTrainingContentListAsync();
        Task<Response<CommonResponse>> UpdateTrainingContentAsync(UpdateTrainingContentDto data, CancellationToken cancellationToken = default);
    }
}
