using HrHub.Abstraction.Result;
using HrHub.Application.Managers.Pricing;
using HrHub.Domain.Contracts.Dtos.PricingDtos;
using HrHub.Domain.Contracts.Responses.CommonResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PricingController : ControllerBase
    {
        private readonly IPricingManager _pricingManager;

        public PricingController(IPricingManager pricingManager)
        {
            _pricingManager = pricingManager;
        }

        // ====================================================================================
        // PRICE TIERS (FİYAT SEVİYELERİ)
        // ====================================================================================

        [HttpGet("tiers")]
        [AllowAnonymous]
        public async Task<Response<List<PriceTierDto>>> GetPriceTiers([FromQuery] bool activeOnly = true)
        {
            return await _pricingManager.GetAllPriceTiersAsync(activeOnly);
        }

        [HttpPost("tiers")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<Response<ReturnIdResponse>> CreatePriceTier([FromBody] CreatePriceTierDto dto)
        {
            return await _pricingManager.CreatePriceTierAsync(dto);
        }

        [HttpPut("tiers")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<Response<CommonResponse>> UpdatePriceTier([FromBody] UpdatePriceTierDto dto)
        {
            return await _pricingManager.UpdatePriceTierAsync(dto);
        }

        [HttpDelete("tiers/{id}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<Response<CommonResponse>> DeletePriceTier(long id)
        {
            return await _pricingManager.DeletePriceTierAsync(id);
        }

        // ====================================================================================
        // PRICE TIER DETAILS (MATRİS KURALLARI)
        // ====================================================================================

        [HttpPost("tiers/details")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<Response<CommonResponse>> AddOrUpdateDetail([FromBody] TierDetailDto dto)
        {
            return await _pricingManager.AddOrUpdateTierDetailAsync(dto);
        }

        [HttpDelete("tiers/details/{detailId}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<Response<CommonResponse>> DeleteDetail(long detailId)
        {
            return await _pricingManager.DeleteTierDetailAsync(detailId);
        }

        // ====================================================================================
        // SUBSCRIPTION PLANS (ABONELİK PAKETLERİ)
        // ====================================================================================

        [HttpGet("plans")]
        [AllowAnonymous]
        public async Task<Response<List<SubscriptionPlanDto>>> GetSubscriptionPlans()
        {
            return await _pricingManager.GetSubscriptionPlansAsync();
        }

        [HttpPost("plans")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<Response<ReturnIdResponse>> CreateSubscriptionPlan([FromBody] CreateSubscriptionPlanDto dto)
        {
            return await _pricingManager.CreateSubscriptionPlanAsync(dto);
        }

        [HttpPatch("plans/{id}/toggle")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<Response<CommonResponse>> TogglePlanStatus(long id)
        {
            return await _pricingManager.ToggleSubscriptionPlanStatusAsync(id);
        }

        [HttpGet("tier/{tierId}")]
        [AllowAnonymous]
        public async Task<Response<List<PriceTierDetailDto>>> GetTierDetails(long tierId)
        {
            return await _pricingManager.GetPricingDetailsByTierIdAsync(tierId);
        }
    }
}