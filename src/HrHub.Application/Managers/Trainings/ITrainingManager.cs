using HrHub.Abstraction.Contracts.Dtos.TrainingDtos;
using HrHub.Abstraction.Data.Collections;
using HrHub.Abstraction.Result;
using HrHub.Core.Base;
using HrHub.Domain.Contracts.Dtos.DashboardDtos;
using HrHub.Domain.Contracts.Dtos.TrainingDtos;
using HrHub.Domain.Contracts.Responses.CommonResponse;

namespace HrHub.Application.Managers.Trainings;

public interface ITrainingManager : IBaseManager
{
    Task<Response<ReturnIdResponse>> AddTrainingAsync(AddTrainingDto data, CancellationToken cancellationToken = default);
    Task<Response<CommonResponse>> DeleteTrainingAsync(long id, CancellationToken cancellationToken = default);
    Task<Response<GetTrainingDto>> GetTrainingByIdAsync(long id);
    Task<Response<IEnumerable<GetTrainingDto>>> GetTrainingListAsync();
    Task<Response<CommonResponse>> ReorderTrainingContentAsync(ReorderTrainingContentDto dto, CancellationToken cancellationToken = default);
    Task<Response<CommonResponse>> UpdateTrainingAsync(UpdateTrainingDto dto, CancellationToken cancellationToken = default);
    Task<Response<IEnumerable<GetMyTrainingDto>>> GetMyTrainingsAsync(CancellationToken cancellationToken = default);
    Task<Response<TrainingDetailDto>> GetTrainingDetailForUserAsync(long trainingId);
    Task<Response<IEnumerable<GetTrainingDto>>> GetMyGivenTrainingsAsync();
    /// <summary>
    /// Kullanıcıya özel, ağırlıklı puanlama algoritması ile eğitim önerir.
    /// </summary>
    Task<Response<List<TrainingViewCardDto>>> GetRecommendedTrainingsAsync();

    Task<Response<List<TrainingViewCardDto>>> SearchTrainingsAsync(string searchTerm, int pageIndex = 0, int pageSize = 12);
    Task<Response<List<TrainingCardDto>>> GetNavbarRecentTrainingsAsync(int count = 5);
    Task<Response<PagedList<TrainingListItemDto>>> GetAdvancedTrainingListAsync(SearchTrainingRequestDto request);
    Task<Response<TrainingFilterOptionsDto>> GetTrainingFilterOptionsAsync();
}
