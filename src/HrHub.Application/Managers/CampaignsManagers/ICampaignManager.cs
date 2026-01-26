using HrHub.Abstraction.Result;
using HrHub.Core.Base;
using HrHub.Domain.Contracts.Dtos.CampaignDtos;
using HrHub.Domain.Contracts.Responses.CommonResponse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrHub.Application.Managers.Campaigns
{
    public interface ICampaignManager : IBaseManager
    {
        // ====================================================================================
        // A. KAMPANYA YÖNETİMİ (ADMIN)
        // ====================================================================================

        /// <summary>
        /// Sistemdeki kampanyaları listeler.
        /// </summary>
        /// <param name="activeOnly">True ise sadece süresi geçmemiş aktifleri getirir.</param>
        Task<Response<List<CampaignListDto>>> GetCampaignsAsync(bool activeOnly = false);

        /// <summary>
        /// Yeni bir kampanya oluşturur.
        /// </summary>
        Task<Response<ReturnIdResponse>> CreateCampaignAsync(CreateCampaignDto dto);

        /// <summary>
        /// Kampanyayı acil durdurur veya tekrar başlatır.
        /// </summary>
        Task<Response<CommonResponse>> ToggleCampaignStatusAsync(long id);


        // ====================================================================================
        // B. KATILIM YÖNETİMİ (EĞİTMEN / INSTRUCTOR)
        // ====================================================================================

        /// <summary>
        /// Giriş yapmış eğitmenin katılabileceği 'Opt-In' kampanyaları listeler.
        /// </summary>
        Task<Response<List<CampaignOpportunityDto>>> GetAvailableCampaignsForInstructorAsync();

        /// <summary>
        /// Eğitmenin bir kampanyaya katılmasını sağlar.
        /// </summary>
        Task<Response<CommonResponse>> JoinCampaignAsync(JoinCampaignDto dto);

        /// <summary>
        /// Eğitmenin katıldığı bir kampanyadan ayrılmasını sağlar.
        /// </summary>
        Task<Response<CommonResponse>> ExitCampaignAsync(long campaignId);
    }
}