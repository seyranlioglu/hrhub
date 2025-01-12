using HrHub.Abstraction.Result;
using HrHub.Core.Base;
using HrHub.Domain.Contracts.Dtos.WhatYouWillLearns;
using HrHub.Domain.Contracts.Dtos.WhatYouWillLearnsDtos;
using HrHub.Domain.Contracts.Responses.CommonResponse;

namespace HrHub.Application.Managers.WhatYouWillLearnManagers
{
    public interface IWhatYouWillLearnManager : IBaseManager
    {
        Task<Response<ReturnIdResponse>> AddWhatYouWillLearnAsync(AddWhatYouWillLearnDto data, CancellationToken cancellationToken = default);
        Task<Response<CommonResponse>> DeleteWhatYouWillLearnAsync(long id, CancellationToken cancellationToken = default);
        Task<Response<GetWhatYouWillLearnDto>> GetWhatYouWillLearnByIdAsync(long id);
        Task<Response<IEnumerable<GetWhatYouWillLearnDto>>> GetWhatYouWillLearnListAsync();
        Task<Response<CommonResponse>> UpdateWhatYouWillLearnAsync(UpdateWhatYouWillLearnDto dto, CancellationToken cancellationToken = default);
    }
}
