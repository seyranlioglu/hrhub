using HrHub.Abstraction.Result;
using HrHub.Core.Base;
using HrHub.Domain.Contracts.Dtos.ContentCommentDtos;

namespace HrHub.Application.Managers.CommentManagers
{
    public interface IContentCommentManager : IBaseManager
    {
        Task<Response<CommonResponse>> AddContentCommentAsync(AddContentCommentDto data, CancellationToken cancellationToken = default);
        Task<Response<CommonResponse>> DeleteContentCommentAsync(long id, CancellationToken cancellationToken = default);
        Task<Response<ContentCommentDto>> GetContentCommentByIdAsync(long id);
        Task<Response<IEnumerable<ContentCommentDto>>> GetContentCommentListAsync();
        Task<Response<CommonResponse>> UpdateContentCommentAsync(UpdateContentCommentDto dto, CancellationToken cancellationToken = default);
    }
}
