using HrHub.Abstraction.Result;
using HrHub.Application.Managers.ExamOperationManagers;
using HrHub.Core.Controllers;
using HrHub.Domain.Contracts.Dtos.ExamDtos;
using HrHub.Domain.Contracts.Responses.CommonResponse;
using HrHub.Domain.Contracts.Responses.ExamResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        [Authorize(Roles = "Admin", Policy = "Instructior")]
        public async Task<Response<AddExamResponse>> AddExam([FromBody] AddExamDto data)
        {
            var response = await examManager.AddExamAsync(data).ConfigureAwait(false);
            return response;
        }

        [HttpPost("[Action]")]
        [Authorize(Roles = "Admin", Policy = "Instructior")]
        public async Task<Response<ReturnIdResponse>> AddExamTopic([FromBody] AddExamTopicDto data)
        {
            var response = await examManager.AddExamTopicAsync(data).ConfigureAwait(false);
            return response;
        }

        [HttpPost("[Action]")]
        [Authorize(Roles = "Admin", Policy = "Instructior")]
        public async Task<Response<ReturnIdResponse>> AddExamQuestionAsync(AddExamQuestionDto data)
        {
            var response = await examManager.AddExamQuestionAsync(data).ConfigureAwait(false);
            return response;
        }

        [HttpPost("[Action]")]
        [Authorize(Roles = "Admin", Policy = "Instructior")]
        public async Task<Response<List<GetExamListResponse>>> GetExamListForGridAsync(GetExamListDto filter)
        {
            var response = await examManager.GetExamListForGridAsync(filter).ConfigureAwait(false);
            return response;
        }

        [HttpPost("[Action]")]
        [Authorize(Roles = "Admin", Policy = "Instructior")]
        public async Task<Response<AddExamVersionReponse>> AddNewVersionAsync(AddNewVersionDto versionData)
        {
            var response = await examManager.AddNewVersionAsync(versionData).ConfigureAwait(false);
            return response;
        }

        [HttpPut("[Action]")]
        [Authorize(Roles = "Admin", Policy = "Instructior")]
        public async Task<Response<CommonResponse>> UpdateExamInfoAsync(UpdateExamDto updateData)
        {
            var response = await examManager.UpdateExamInfoAsync(updateData).ConfigureAwait(false);
            return response;
        }

        [HttpPut("[Action]")]
        [Authorize(Roles = "Admin", Policy = "Instructior")]
        public async Task<Response<CommonResponse>> UpdateTopicInfoAsync(UpdateExamTopicDto updateData)
        {
            var response = await examManager.UpdateTopicInfoAsync(updateData).ConfigureAwait(false);
            return response;
        }

        [HttpPut("[Action]")]
        [Authorize(Roles = "Admin", Policy = "Instructior")]
        public async Task<Response<CommonResponse>> UpdateSeqNoAsync(UpdateExamTopicSeqNumDto updateData)
        {
            var response = await examManager.UpdateSeqNoAsync(updateData).ConfigureAwait(false);
            return response;
        }

        [HttpPut("[Action]")]
        [Authorize(Roles = "Admin", Policy = "Instructior")]
        public async Task<Response<CommonResponse>> UpdateQuestionAsync(UpdateExamQuestionDto updateData)
        {
            var response = await examManager.UpdateQuestionAsync(updateData).ConfigureAwait(false);
            return response;
        }

        [HttpPut("[Action]")]
        [Authorize(Roles = "User")]
        public async Task<Response<GetExamResponse>> GetExamByIdWithStudentAsync(GetExamDto filter)
        {
            var response = await examManager.GetExamByIdWithStudentAsync(filter).ConfigureAwait(false);
            return response;
        }

        //[HttpPut("[Action]")]
        //[Authorize(Roles = "Admin", Policy = "Instructior")]
        //public async Task<Response<CommonResponse>> PublishVersionAsync(PublishExamVersionDto data)
        //{
        //    var response = await examManager.PublishExamVersionAsync(data).ConfigureAwait(false);
        //    return response;
        //}

        //[HttpPut("[Action]")]
        //[Authorize(Roles = "User")]
        //public async Task<Response<CalculateExamResultResponse>> CalculateExamResultAsync(CalculateExamResultDto examResult)
        //{
        //    var response = await examManager.CalculateUserExamResultAsync(examResult).ConfigureAwait(false);
        //    return response;
        //}
    }
}
