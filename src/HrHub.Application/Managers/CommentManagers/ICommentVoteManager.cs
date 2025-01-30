using HrHub.Abstraction.Result;
using HrHub.Core.Base;
using HrHub.Domain.Contracts.Dtos.CommentVoteDtos;

namespace HrHub.Application.Managers.CommentManagers
{
    public interface ICommentVoteManager : IBaseManager
    {
        Task<Response<CommonResponse>> AddCommentVoteAsync(AddCommentVoteDto data, CancellationToken cancellationToken = default);
        Task<Response<CommonResponse>> DeleteCommentVoteAsync(long id, CancellationToken cancellationToken = default);
        Task<Response<CommentVoteDto>> GetCommentVoteByIdAsync(long id);
        Task<Response<IEnumerable<CommentVoteDto>>> GetCommentVoteListAsync();
        Task<Response<CommonResponse>> UpdateCommentVoteAsync(UpdateCommentVoteDto dto, CancellationToken cancellationToken = default);
    }
}
