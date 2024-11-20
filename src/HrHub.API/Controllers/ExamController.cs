using HrHub.Abstraction.Result;
using HrHub.Application.Managers.ExamOperationManagers;
using HrHub.Core.Controllers;
using HrHub.Domain.Contracts.Dtos.ExamDtos;
using HrHub.Domain.Contracts.Responses.ExamResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HrHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ApiControllerBase
    {
        private readonly IExamManager examManager;

        public ExamController(IExamManager examManager)
        {
            this.examManager = examManager;
        }
        [HttpPost("[Action]")]
        [Authorize(Roles ="Instructior")]
        public async Task<Response<AddExamResponse>> AddExam([FromBody]AddExamDto data)
        {
            var response = await examManager.AddExamAsync(data).ConfigureAwait(false);
            return response;
        }


    }
}
