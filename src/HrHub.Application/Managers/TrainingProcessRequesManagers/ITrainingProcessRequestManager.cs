using HrHub.Abstraction.Contracts.Dtos.TrainingDtos;
using HrHub.Abstraction.Result;
using HrHub.Core.Base;
using HrHub.Domain.Contracts.Dtos.TrainingProcessRequestDtos;
using HrHub.Domain.Contracts.Responses.CommonResponse;

namespace HrHub.Abstraction.Managers.TrainingProcessRequestManagers;

/// <summary>
/// Eğitim süreçlerindeki her türlü talep (onay, süre uzatma, atama vb.) trafiğini yöneten merkezi servis.
/// </summary>
public interface ITrainingProcessRequestManager : IBaseManager
{
    /// <summary>
    /// Kullanıcı veya eğitmen tarafından yeni bir süreç talebi oluşturur.
    /// </summary>
    /// <param name="dto">Talep detaylarını içeren veri transfer objesi.</param>
    /// <returns>Oluşturulan talebin ID'sini döner.</returns>
    Task<Response<ReturnIdResponse>> CreateRequestAsync(CreateProcessRequestDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Bir yönetici veya admin tarafından gelen talebi yanıtlar (Onay/Ret).
    /// </summary>
    /// <param name="requestId">Yanıtlanacak talebin ID'si.</param>
    /// <param name="isApproved">Onay durumu (true ise onay, false ise ret).</param>
    /// <param name="adminNote">İşlemi yapanın eklediği açıklama veya ret gerekçesi.</param>
    /// <returns>İşlem sonucunu döner.</returns>
    Task<Response<CommonResponse>> RespondToRequestAsync(long requestId, bool isApproved, string? adminNote, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sistemde bekleyen (Pending) tüm talepleri listeler.
    /// </summary>
    /// <returns>Bekleyen talep listesini döner.</returns>
    Task<Response<IEnumerable<ProcessRequestListDto>>> GetPendingRequestsAsync();
}