using HrHub.Abstraction.Contracts.Dtos.TrainingDtos;
using HrHub.Abstraction.Managers.TrainingProcessRequestManagers;
using HrHub.Abstraction.Result;
using HrHub.Core.Controllers;
using HrHub.Domain.Contracts.Dtos.TrainingProcessRequestDtos;
using HrHub.Domain.Contracts.Responses.CommonResponse;
using Microsoft.AspNetCore.Mvc;

namespace HrHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingProcessRequestController : ApiControllerBase
    {
        private readonly ITrainingProcessRequestManager _requestManager;

        public TrainingProcessRequestController(ITrainingProcessRequestManager requestManager)
        {
            _requestManager = requestManager;
        }

        /// <summary>
        /// Yeni bir işlem talebi oluşturur (Yayın onayı, Süre uzatma vb.)
        /// </summary>
        [HttpPost("[Action]")]
        public async Task<Response<ReturnIdResponse>> CreateRequest([FromBody] CreateProcessRequestDto data)
        {
            var response = await _requestManager.CreateRequestAsync(data).ConfigureAwait(false);
            return response;
        }

        /// <summary>
        /// Gelen bir talebi onaylar veya reddeder.
        /// </summary>
        /// <param name="requestId">Talebin benzersiz ID'si</param>
        /// <param name="isApproved">Onay için true, ret için false</param>
        /// <param name="adminNote">Açıklama veya ret sebebi</param>
        [HttpPost("respond")]
        public async Task<Response<CommonResponse>> RespondToRequest(long requestId, bool isApproved, string? adminNote)
        {
            var response = await _requestManager.RespondToRequestAsync(requestId, isApproved, adminNote).ConfigureAwait(false);
            return response;
        }

        /// <summary>
        /// Bekleyen tüm talepleri listeler (Admin/Yönetici ekranı için).
        /// </summary>
        [HttpGet("[Action]")]
        public async Task<Response<IEnumerable<ProcessRequestListDto>>> GetPendingRequests()
        {
            var response = await _requestManager.GetPendingRequestsAsync().ConfigureAwait(false);
            return response;
        }
    }
}