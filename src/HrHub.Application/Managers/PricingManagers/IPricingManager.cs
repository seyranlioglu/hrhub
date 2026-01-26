using HrHub.Abstraction.Result;
using HrHub.Core.Base;
using HrHub.Domain.Contracts.Dtos.PricingDtos;
using HrHub.Domain.Contracts.Responses.CommonResponse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrHub.Application.Managers.Pricing
{
    public interface IPricingManager : IBaseManager
    {
        // ====================================================================================
        // A. PRICE TIER (Fiyat Seviyeleri)
        // ====================================================================================

        /// <summary>
        /// Sistemdeki tüm fiyat seviyelerini (Tier) ve detaylarını getirir.
        /// </summary>
        Task<Response<List<PriceTierDto>>> GetAllPriceTiersAsync(bool activeOnly = true);

        /// <summary>
        /// Yeni bir fiyat seviyesi (başlık olarak) oluşturur.
        /// </summary>
        Task<Response<ReturnIdResponse>> CreatePriceTierAsync(CreatePriceTierDto dto);

        /// <summary>
        /// Mevcut fiyat seviyesini günceller.
        /// </summary>
        Task<Response<CommonResponse>> UpdatePriceTierAsync(UpdatePriceTierDto dto);

        /// <summary>
        /// Fiyat seviyesini siler (Soft Delete).
        /// </summary>
        Task<Response<CommonResponse>> DeletePriceTierAsync(long id);


        // ====================================================================================
        // B. PRICE TIER DETAILS (Fiyat Matrisi - Kurallar)
        // ====================================================================================

        /// <summary>
        /// Bir Tier için fiyat kuralı ekler veya günceller (Örn: 1-10 kişi arası X TL).
        /// </summary>
        Task<Response<CommonResponse>> AddOrUpdateTierDetailAsync(TierDetailDto dto);

        /// <summary>
        /// Fiyat kuralını siler.
        /// </summary>
        Task<Response<CommonResponse>> DeleteTierDetailAsync(long detailId);


        // ====================================================================================
        // C. SUBSCRIPTION PLANS (Abonelik Paketleri)
        // ====================================================================================

        /// <summary>
        /// Aktif abonelik paketlerini listeler (Fiyatlar sayfası için).
        /// </summary>
        Task<Response<List<SubscriptionPlanDto>>> GetSubscriptionPlansAsync();

        /// <summary>
        /// Yeni bir abonelik paketi oluşturur.
        /// </summary>
        Task<Response<ReturnIdResponse>> CreateSubscriptionPlanAsync(CreateSubscriptionPlanDto dto);

        /// <summary>
        /// Paketi satışa açar veya kapatır.
        /// </summary>
        Task<Response<CommonResponse>> ToggleSubscriptionPlanStatusAsync(long id);

        Task<Response<List<PriceTierDetailDto>>> GetPricingDetailsByTierIdAsync(long tierId);
    }
}