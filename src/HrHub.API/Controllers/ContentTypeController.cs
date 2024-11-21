using HrHub.Abstraction.Contracts.Dtos.ContentTypes;
using HrHub.Abstraction.Result;
using HrHub.Application.Managers.ContentTypes;
using HrHub.Core.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace HrHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentTypeController : ApiControllerBase
    {
        private readonly IContentTypeManager contentTypeManager;

        public ContentTypeController(IContentTypeManager contentTypeManager)
        {
            this.contentTypeManager = contentTypeManager;
        }

        [HttpGet("[Action]")]
        public async Task<Response<IEnumerable<ContentTypeDto>>> GetListAsync()
        {
            return await contentTypeManager.GetListForContentTypeAsync();
        }

        [HttpGet("[Action]")]
        public async Task<Response<ContentTypeDto>> GetByIdAsync(long id)
        {
            return await contentTypeManager.GetByIdForContentTypeAsync(id);
        }
    }
}
