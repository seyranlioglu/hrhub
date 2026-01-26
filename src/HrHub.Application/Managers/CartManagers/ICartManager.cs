using HrHub.Abstraction.Result;
using HrHub.Core.Base;
using HrHub.Domain.Contracts.Dtos.CartDtos;
using System.Threading.Tasks;

namespace HrHub.Application.Managers.CartManagers
{
    public interface ICartManager : IBaseManager
    {
        // Aktif sepeti getir (Görüntüleme)
        Task<Response<CartViewDto>> GetActiveCartAsync();

        // Sepete Ekle
        Task<Response<CartViewDto>> AddToCartAsync(AddToCartDto request);

        // Sepetten Çıkar
        Task<Response<CartViewDto>> RemoveFromCartAsync(long cartItemId);

        // Sepeti Onaya Gönder (Sipariş Ver)
        Task<Response<CommonResponse>> CheckoutCartAsync(long cartId, string note);

        // Sepet Kalem Sayısını Güncelle (Opsiyonel: Adet artırma/azaltma)
        Task<Response<CartViewDto>> UpdateLicenceCountAsync(long cartItemId, int count);
    }
}