using HrHub.Abstraction.Result;
using HrHub.Core.Base;
using HrHub.Domain.Contracts.Dtos.TrainingCategoryDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Application.Managers.TrainingCategories
{
    public interface ITrainingCategoryManager : IBaseManager
    {
        Task<Response<CommonResponse>> AddTrainingCategoryAsync(AddTrainingCategoryDto data, CancellationToken cancellationToken = default);
        Task<Response<CommonResponse>> DeleteTrainingCategoryAsync(long id, CancellationToken cancellationToken = default);
        Task<Response<IEnumerable<GetTrainingCategoryDto>>> GetListTrainingCategoryAsync();
        Task<Response<GetTrainingCategoryDto>> GetTrainingCategoryAsync(long id);
        Task<Response<CommonResponse>> UpdateTrainingCategoryAsync(UpdateTrainingCategoryDto data, CancellationToken cancellationToken = default);
    }
}
