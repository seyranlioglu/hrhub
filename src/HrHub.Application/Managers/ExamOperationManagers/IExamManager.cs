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
        Task<Response<ReturnIdResponse>> AddExamTopic(AddExamTopicDto data);
    }
}