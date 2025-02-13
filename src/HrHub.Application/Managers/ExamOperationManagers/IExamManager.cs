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
        Task<Response<AddExamVersionReponse>> AddNewVersionAsync(AddNewVersionDto versionData, CancellationToken cancellationToken = default);
        //Task<Response<CalculateExamResultResponse>> CalculateUserExamResultAsync(long userExamId, CancellationToken cancellationToken = default);
        Task<Response<GetExamResponse>> GetExamByIdWithStudentAsync(GetExamDto filter, CancellationToken cancellationToken = default);
        Task<Response<List<GetExamListResponse>>> GetExamDetail(GetExamDetailDto filter, CancellationToken cancellationToken = default);
        Task<Response<GetExamListForLookupResponse>> GetExamListForLookup(GetExamListDto filter, CancellationToken cancellationToken = default);
        Task<Response<GetNextQuestionResponse>> GetNextQuestionAsync(GetNextQuestionDto filter, CancellationToken cancellationToken = default);
        Task<Response<GetExamInstructionResponse>> PrePrepareExamForStudentAsync(GetExamDto filter, CancellationToken cancellationToken = default);
        Task<Response<PublishExamVersionResponse>> PublishExamVersionAsync(PublishExamVersionDto request, CancellationToken cancellationToken = default);
        Task<Response<CommonResponse>> UpdateExamInfoAsync(UpdateExamDto updateData, CancellationToken cancellationToken = default);
        Task<Response<CommonResponse>> UpdateQuestionAsync(UpdateExamQuestionDto updateData, CancellationToken cancellationToken = default);
        Task<Response<CommonResponse>> UpdateSeqNoAsync(UpdateExamTopicSeqNumDto updateData, CancellationToken cancellationToken = default);
        Task<Response<CommonResponse>> UpdateTopicInfoAsync(UpdateExamTopicDto updateData, CancellationToken cancellationToken = default);
    }
}