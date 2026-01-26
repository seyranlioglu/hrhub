using HrHub.Abstraction.Result;
using HrHub.Application.Managers.Campaigns;
using HrHub.Domain.Contracts.Dtos.CampaignDtos;
using HrHub.Domain.Contracts.Responses.CommonResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        private readonly ICampaignManager _campaignManager;

        public CampaignController(ICampaignManager campaignManager)
        {
            _campaignManager = campaignManager;
        }

        // ====================================================================================
        // ADMIN İŞLEMLERİ
        // ====================================================================================

        [HttpGet]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<Response<List<CampaignListDto>>> GetCampaigns([FromQuery] bool activeOnly = false)
        {
            return await _campaignManager.GetCampaignsAsync(activeOnly);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<Response<ReturnIdResponse>> CreateCampaign([FromBody] CreateCampaignDto dto)
        {
            return await _campaignManager.CreateCampaignAsync(dto);
        }

        [HttpPatch("{id}/toggle")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<Response<CommonResponse>> ToggleCampaignStatus(long id)
        {
            return await _campaignManager.ToggleCampaignStatusAsync(id);
        }

        // ====================================================================================
        // EĞİTMEN İŞLEMLERİ (INSTRUCTOR)
        // ====================================================================================

        [HttpGet("opportunities")]
        [Authorize(Roles = "Instructor,Admin,SuperAdmin")]
        public async Task<Response<List<CampaignOpportunityDto>>> GetInstructorOpportunities()
        {
            return await _campaignManager.GetAvailableCampaignsForInstructorAsync();
        }

        [HttpPost("join")]
        [Authorize(Roles = "Instructor")]
        public async Task<Response<CommonResponse>> JoinCampaign([FromBody] JoinCampaignDto dto)
        {
            return await _campaignManager.JoinCampaignAsync(dto);
        }

        [HttpPost("exit/{campaignId}")]
        [Authorize(Roles = "Instructor")]
        public async Task<Response<CommonResponse>> ExitCampaign(long campaignId)
        {
            return await _campaignManager.ExitCampaignAsync(campaignId);
        }
    }
}