
using HrHub.Abstraction.Result;
using HrHub.Application.Managers.ContentTypes;
using HrHub.Application.Managers.TrainingCategories;
using HrHub.Core.Controllers;
using HrHub.Domain.Contracts.Dtos.ContentTypes;
using HrHub.Domain.Contracts.Dtos.TrainingCategoryDtos;
using Microsoft.AspNetCore.Authorization;
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
        //[Authorize(Roles = "Admin", Policy = "Instructior")]
        public async Task<Response<IEnumerable<ContentTypeDto>>> GetListAsync()
        {
            return await contentTypeManager.GetListForContentTypeAsync();
        }


        [HttpGet("[Action]")]
        //[Authorize(Roles = "Admin", Policy = "Instructior")]
        public async Task<Response<ContentTypeDto>> GetByIdAsync(long id)
        {
            return await contentTypeManager.GetByIdForContentTypeAsync(id);
        }

        [HttpPost("[Action]")]
        //[Authorize(Roles = "Admin", Policy = "Instructior")]
        public async Task<Response<CommonResponse>> AddContentType([FromBody] AddContentTypeDto data)
        {
            var response = await contentTypeManager.AddContentTypeAsync(data).ConfigureAwait(false);
            return response;
        }

        [HttpPut("[Action]")]
        //[Authorize(Roles = "Admin", Policy = "Instructior")]
        public async Task<Response<CommonResponse>> UpdateContentType([FromBody] UpdateContentTypeDto data)
        {
            var response = await contentTypeManager.UpdateContentTypeAsync(data).ConfigureAwait(false);
            return response;
        }

        [HttpDelete("[Action]")]
        //[Authorize(Roles = "Admin", Policy = "Instructior")]
        public async Task<Response<CommonResponse>> DeleteContentType([FromBody] long id)
        {
            var response = await contentTypeManager.DeleteContentTypeAsync(id).ConfigureAwait(false);
            return response;
        }
    }
}
