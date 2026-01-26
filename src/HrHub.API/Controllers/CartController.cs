using HrHub.Abstraction.Result;
using HrHub.Application.Managers.CartManagers;
using HrHub.Core.Controllers;
using HrHub.Domain.Contracts.Dtos.CartDtos;
using HrHub.Domain.Contracts.Responses.CommonResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HrHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Sadece giriş yapmış kullanıcılar sepet işlemi yapabilir
    public class CartController : ApiControllerBase
    {
        private readonly ICartManager _cartManager;

        public CartController(ICartManager cartManager)
        {
            _cartManager = cartManager;
        }

        // GET: api/Cart/get-active-cart
        [HttpGet("get-active-cart")]
        public async Task<Response<CartViewDto>> GetActiveCart()
        {
            return await _cartManager.GetActiveCartAsync().ConfigureAwait(false);
        }

        // POST: api/Cart/add-to-cart
        [HttpPost("add-to-cart")]
        public async Task<Response<CartViewDto>> AddToCart([FromBody] AddToCartDto request)
        {
            return await _cartManager.AddToCartAsync(request).ConfigureAwait(false);
        }

        // DELETE: api/Cart/remove-from-cart/{cartItemId}
        [HttpDelete("remove-from-cart/{cartItemId}")]
        public async Task<Response<CartViewDto>> RemoveFromCart(long cartItemId)
        {
            return await _cartManager.RemoveFromCartAsync(cartItemId).ConfigureAwait(false);
        }

        // POST: api/Cart/checkout
        [HttpPost("checkout")]
        public async Task<Response<CommonResponse>> Checkout([FromBody] CheckoutRequestDto request)
        {
            // Not: CheckoutRequestDto aşağıda tanımlı veya DTO klasöründe olmalı.
            // Şimdilik string note parametresini body'den alacak şekilde düzenleyelim.
            return await _cartManager.CheckoutCartAsync(request.CartId, request.Note).ConfigureAwait(false);
        }

        // POST: api/Cart/update-licence
        [HttpPost("update-licence")]
        public async Task<Response<CartViewDto>> UpdateLicence([FromBody] UpdateLicenceDto request)
        {
            return await _cartManager.UpdateLicenceCountAsync(request.CartItemId, request.Count).ConfigureAwait(false);
        }
    }

    // Controller içinde hızlı kullanım için DTO'lar (Eğer ayrı dosyada yoksa)
    public class CheckoutRequestDto
    {
        public long CartId { get; set; }
        public string Note { get; set; }
    }

    public class UpdateLicenceDto
    {
        public long CartItemId { get; set; }
        public int Count { get; set; }
    }
}