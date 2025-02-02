using HrHub.Abstraction.Result;
using HrHub.Core.Base;
using HrHub.Domain.Contracts.Dtos.CurrAccTrainingDtos;
using HrHub.Domain.Contracts.Responses.CommonResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Application.Managers.CurrAccTrainingManagers
{
    public interface ICurrAccTrainingManager : IBaseManager
    {
        Task<Response<ReturnIdResponse>> AddCurrAccTrainingAsync(AddCurrAccTrainingDto data, CancellationToken cancellationToken = default);
        Task<Response<CommonResponse>> DeleteCurrAccTrainingAsync(DeleteCurrAccTrainingDto deleteCurrAccTrainingDto, CancellationToken cancellationToken = default);
        Task<Response<IEnumerable<GetCurrAccTrainingDto>>> GetAllCurrAccTrainingsAsync();
        Task<Response<GetCurrAccTrainingDto>> GetCurrAccTrainingByIdAsync(long id);
        Task<Response<CommonResponse>> UpdateCurrAccTrainingAsync(UpdateCurrAccTrainingDto data, CancellationToken cancellationToken = default);
    }
}
