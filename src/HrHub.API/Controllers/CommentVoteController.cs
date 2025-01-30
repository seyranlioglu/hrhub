using HrHub.Abstraction.Consts;
using HrHub.Abstraction.Result;
using HrHub.Application.Managers.CommentManagers;
using HrHub.Core.Controllers;
using HrHub.Domain.Contracts.Dtos.CommentVoteDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HrHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentVoteController : ApiControllerBase
    {
        private readonly ICommentVoteManager commentVoteManager;

        public CommentVoteController(ICommentVoteManager commentVoteManager)
        {
            this.commentVoteManager = commentVoteManager;
        }

        [HttpPost("[Action]")]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.User}")]
        public async Task<Response<CommonResponse>> AddCommentVote([FromBody] AddCommentVoteDto data)
        {
            var response = await commentVoteManager.AddCommentVoteAsync(data).ConfigureAwait(false);
            return response;
        }

        [HttpPut("[Action]")]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.User}")]
        public async Task<Response<CommonResponse>> UpdateCommentVote([FromBody] UpdateCommentVoteDto data)
        {
            var response = await commentVoteManager.UpdateCommentVoteAsync(data).ConfigureAwait(false);
            return response;
        }

        [HttpDelete("[Action]")]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.User}")]
        public async Task<Response<CommonResponse>> DeleteCommentVote([FromBody] long id)
        {
            var response = await commentVoteManager.DeleteCommentVoteAsync(id).ConfigureAwait(false);
            return response;
        }

        [HttpGet("[Action]")]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.User}")]
        public async Task<Response<IEnumerable<CommentVoteDto>>> GetList()
        {
            return await commentVoteManager.GetCommentVoteListAsync().ConfigureAwait(false);
        }

        [HttpGet("[Action]")]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.User}")]
        public async Task<Response<CommentVoteDto>> GetById(long id)
        {
            return await commentVoteManager.GetCommentVoteByIdAsync(id).ConfigureAwait(false);
        }
    }
}
