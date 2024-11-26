using HrHub.Abstraction.Result;
using HrHub.Core.Base;
using HrHub.Domain.Contracts.Dtos.ExamDtos;
using HrHub.Domain.Contracts.Responses.CommonResponse;
using HrHub.Domain.Contracts.Responses.ExamResponses;

namespace HrHub.Application.Managers.ExamOperationManagers
{
    public interface IExamManager : IBaseManager
    {
        Task<Response<AddExamResponse>> AddExamAsync(AddExamDto data, CancellationToken cancellationToken = default);
        Task<Response<ReturnIdResponse>> AddExamQuestionAsync(AddExamQuestionDto question, CancellationToken cancellationToken = default);
        Task<Response<ReturnIdResponse>> AddExamTopicAsync(AddExamTopicDto data, CancellationToken cancellationToken = default);
        Task<Response<GetExamResponse>> GetExamByContentIdAsync(GetExamDto filter, CancellationToken cancellationToken = default);
        Task<Response<List<GetExamListResponse>>> GetExamListForGridAsync(GetExamListDto filter, CancellationToken cancellationToken = default);
    }
}