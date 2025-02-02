using HrHub.Abstraction.Result;
using HrHub.Application.Managers.WhatYouWillLearnManagers;
using HrHub.Core.Controllers;
using HrHub.Domain.Contracts.Dtos.WhatYouWillLearns;
using HrHub.Domain.Contracts.Dtos.WhatYouWillLearnsDtos;
using HrHub.Domain.Contracts.Responses.CommonResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HrHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhatYouWillLearnController : ApiControllerBase
    {
        private readonly IWhatYouWillLearnManager whatYouWillLearnManager;

        public WhatYouWillLearnController(IWhatYouWillLearnManager whatYouWillLearnManager)
        {
            this.whatYouWillLearnManager = whatYouWillLearnManager;
        }
        [HttpPost("[Action]")]
        //[Authorize(Roles = "Admin", Policy = "Instructior")]
        public async Task<Response<ReturnIdResponse>> AddWhatYouWillLearnAsync([FromBody] AddWhatYouWillLearnDto data)
        {
            var response = await whatYouWillLearnManager.AddWhatYouWillLearnAsync(data).ConfigureAwait(false);
            return response;
        }

        [HttpPut("[Action]")]
        //[Authorize(Roles = "Admin", Policy = "Instructior")]
        public async Task<Response<CommonResponse>> UpdateWhatYouWillLearnAsync([FromBody] UpdateWhatYouWillLearnDto data)
        {
            var response = await whatYouWillLearnManager.UpdateWhatYouWillLearnAsync(data).ConfigureAwait(false);
            return response;
        }

        [HttpDelete("[Action]")]
        [Authorize(Roles = "Admin", Policy = "Instructior")]
        public async Task<Response<CommonResponse>> DeleteWhatYouWillLearnAsync([FromBody] long id)
        {
            var response = await whatYouWillLearnManager.DeleteWhatYouWillLearnAsync(id).ConfigureAwait(false);
            return response;
        }

        [HttpGet("[Action]")]
        public async Task<Response<IEnumerable<GetWhatYouWillLearnDto>>> GetListAsync()
        {
            return await whatYouWillLearnManager.GetWhatYouWillLearnListAsync().ConfigureAwait(false);
        }

        [HttpGet("[Action]")]
        public async Task<Response<GetWhatYouWillLearnDto>> GetByIdAsync([FromRoute] long id)
        {
            return await whatYouWillLearnManager.GetWhatYouWillLearnByIdAsync(id).ConfigureAwait(false);
        }
    }
}
