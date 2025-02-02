using HrHub.Abstraction.Consts;
using HrHub.Abstraction.Result;
using HrHub.Application.Managers.CommentManagers;
using HrHub.Core.Controllers;
using HrHub.Domain.Contracts.Dtos.ContentCommentDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HrHub.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class ContentCommentController : ApiControllerBase
    {
        private readonly IContentCommentManager contentCommentManager;

        public ContentCommentController(IContentCommentManager contentCommentManager)
        {
            this.contentCommentManager = contentCommentManager;
        }

        [HttpPost("[Action]")]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.User}")]
        public async Task<Response<CommonResponse>> AddContentComment([FromBody] AddContentCommentDto data)
        {
            var response = await contentCommentManager.AddContentCommentAsync(data).ConfigureAwait(false);
            return response;
        }

        [HttpPut("[Action]")]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.User}")]
        public async Task<Response<CommonResponse>> UpdateContentComment([FromBody] UpdateContentCommentDto data)
        {
            var response = await contentCommentManager.UpdateContentCommentAsync(data).ConfigureAwait(false);
            return response;
        }

        [HttpDelete("[Action]")]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.User}")]
        public async Task<Response<CommonResponse>> DeleteContentComment([FromBody] long id)
        {
            var response = await contentCommentManager.DeleteContentCommentAsync(id).ConfigureAwait(false);
            return response;
        }

        [HttpGet("[Action]")]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.User}")]
        public async Task<Response<IEnumerable<ContentCommentDto>>> GetList()
        {
            return await contentCommentManager.GetContentCommentListAsync().ConfigureAwait(false);
        }

        [HttpGet("[Action]")]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.User}")]
        public async Task<Response<ContentCommentDto>> GetById(long id)
        {
            return await contentCommentManager.GetContentCommentByIdAsync(id).ConfigureAwait(false);
        }
    }
}
