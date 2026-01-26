using HrHub.Abstraction.Consts;
using HrHub.Abstraction.Data;
using HrHub.Abstraction.Data.EfCore.Repository;
using HrHub.Abstraction.Result;
using HrHub.Abstraction.StatusCodes;
using HrHub.Application.Managers.CartManagers;
using HrHub.Application.Repositories.CartStatusRepositories;
using HrHub.Core.Base;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Contracts.Dtos.CartDtos;
using HrHub.Domain.Contracts.Enums;
using HrHub.Domain.Contracts.Responses.CommonResponse;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrHub.Application.Managers.CartManagers
{
    public class CartManager : ManagerBase, ICartManager
    {
        private readonly Repository<Cart> _cartRepository;
        private readonly Repository<CartItem> _cartItemRepository;
        private readonly Repository<Training> _trainingRepository; // Fiyat için gerekli
        private readonly IHrUnitOfWork unitOfWork;
        private readonly ICartStatusRepository _cartStatusRepository;

        public CartManager(
            IHttpContextAccessor httpContextAccessor,
            ICartStatusRepository cartStatusRepository,
            IHrUnitOfWork unitOfWork) : base(httpContextAccessor)
        {
            _cartRepository = unitOfWork.CreateRepository<Cart>();
            _cartItemRepository = unitOfWork.CreateRepository<CartItem>();
            _trainingRepository = unitOfWork.CreateRepository<Training>();
            _cartStatusRepository = cartStatusRepository;
            this.unitOfWork = unitOfWork;
        }

        // 1. AKTİF SEPETİ GETİR
        public async Task<Response<CartViewDto>> GetActiveCartAsync()
        {
            try
            {
                long userId = GetCurrentUserId();
                long activeStatusId = await _cartStatusRepository.GetIdByCodeAsync(CartStatusCodes.Active);

                if (activeStatusId == 0)
                    return ProduceSuccessResponse(new CartViewDto { Items = new List<CartItemViewDto>() });

                var cart = await _cartRepository.GetAsync(
                    predicate: x => x.CreateUserId == userId &&
                                    x.StatusId == activeStatusId &&
                                    x.IsActive == true &&
                                    x.IsDelete != true,
                    include: i => i.Include(c => c.CartItems)
                                   .ThenInclude(ci => ci.Training)
                                   .ThenInclude(t => t.TrainingCategory),
                    orderBy: q => q.OrderByDescending(x => x.CreatedDate)
                );

                if (cart == null)
                {
                    return ProduceSuccessResponse(new CartViewDto { Items = new List<CartItemViewDto>() });
                }

                var viewDto = MapToCartViewDto(cart);
                return ProduceSuccessResponse(viewDto);
            }
            catch (Exception ex)
            {
                return ProduceFailResponse<CartViewDto>("Sepet getirilirken bir hata oluştu.", HrStatusCodes.Status503ServiceUnavailableError);
            }
        }

        // 2. SEPETE EKLE
        public async Task<Response<CartViewDto>> AddToCartAsync(AddToCartDto request)
        {
            try
            {
                long userId = GetCurrentUserId();
                long currAccId = GetCurrAccId();

                long activeStatusId = await _cartStatusRepository.GetIdByCodeAsync(CartStatusCodes.Active);
                if (activeStatusId == 0) return ProduceFailResponse<CartViewDto>("Sistem Hatası: Sepet durumu bulunamadı.", HrStatusCodes.Status111DataNotFound);

                // 1. EĞİTİM VE FİYAT DETAYLARINI ÇEK
                var training = await _trainingRepository.GetAsync(
                    predicate: x => x.Id == request.TrainingId && x.IsActive == true && x.IsDelete != true,
                    include: i => i.Include(t => t.PriceTier).ThenInclude(pt => pt.Details)
                                   .Include(t => t.PriceTier).ThenInclude(pt => pt.CampaignPriceTiers).ThenInclude(cpt => cpt.Campaign)
                );

                if (training == null) return ProduceFailResponse<CartViewDto>("Eğitim bulunamadı.", HrStatusCodes.Status111DataNotFound);

                // 2. SEPETİ BUL VEYA OLUŞTUR
                // Include ile CartItems'ı da çekiyoruz ki üzerinde işlem yapabilelim.
                var cart = await _cartRepository.GetAsync(
                    predicate: x => x.CreateUserId == userId &&
                                    x.StatusId == activeStatusId &&
                                    x.IsActive == true &&
                                    x.IsDelete != true,
                    include: i => i.Include(c => c.CartItems)
                );

                if (cart == null)
                {
                    cart = new Cart
                    {
                        CurrAccId = currAccId,
                        AddCartUserId = userId,
                        StatusId = activeStatusId,
                        CreateUserId = userId,
                        CreatedDate = DateTime.UtcNow,
                        IsActive = true,
                        TotalAmount = 0,
                        PromotionCode = "",
                        CartItems = new List<CartItem>()
                    };
                    // Yeni cart'ı context'e ekliyoruz (State: Added)
                    await _cartRepository.AddAsync(cart);
                }

                // 3. ÜRÜN KONTROLÜ VE GÜNCELLEME
                // CartItems listesi hafızada (memory) olduğu için buradan kontrol ediyoruz.
                var existingItem = cart.CartItems.FirstOrDefault(x => x.TrainingId == request.TrainingId && x.IsDelete != true && x.IsActive == true);

                if (existingItem != null)
                {
                    // --- GÜNCELLEME ---
                    int newQuantity = existingItem.LicenceCount + (request.LicenceCount > 0 ? request.LicenceCount : 1);
                    var priceInfo = CalculatePricing(training, newQuantity);

                    // Mevcut nesneyi güncelliyoruz (State: Modified olacak)
                    existingItem.LicenceCount = newQuantity;
                    existingItem.Amount = priceInfo.UnitSellingPrice;
                    existingItem.CurrentAmount = priceInfo.TotalPrice;
                    existingItem.DiscountRate = priceInfo.DiscountRate;
                    existingItem.UpdateDate = DateTime.UtcNow;
                    existingItem.UpdateUserId = userId;

                    // Repository Update çağırmaya gerek yok, nesne zaten Context üzerinde track ediliyor.
                    // Ama garanti olsun diye çağırabiliriz, zararı yok.
                    _cartItemRepository.Update(existingItem);
                }
                else
                {
                    // --- YENİ EKLEME ---
                    int quantity = request.LicenceCount > 0 ? request.LicenceCount : 1;
                    var priceInfo = CalculatePricing(training, quantity);

                    var newItem = new CartItem
                    {
                        // CartId atamasına gerek yok, cart.CartItems.Add yapınca EF Core ilişkiyi kendi kurar.
                        TrainingId = request.TrainingId,
                        LicenceCount = quantity,
                        Amount = priceInfo.UnitSellingPrice,
                        CurrentAmount = priceInfo.TotalPrice,
                        DiscountRate = priceInfo.DiscountRate,
                        TaxRate = 0,
                        CreateUserId = userId,
                        CreatedDate = DateTime.UtcNow,
                        IsActive = true
                    };

                    // Hafızadaki listeye ekle. 
                    // EF Core SaveChanges dendiğinde bunu fark edip Insert sorgusu atacak.
                    cart.CartItems.Add(newItem);
                }

                // 4. TOPLAM TUTARI GÜNCELLE (RecalculateCartTotal Metodunu Çağırmadan!)
                // Elimizdeki listenin güncel halini toplayıp Cart'a yazıyoruz.
                // Böylece ikinci bir DB sorgusuna ve Tracking hatasına gerek kalmıyor.
                cart.TotalAmount = cart.CartItems
                    .Where(x => x.IsActive && x.IsDelete != true)
                    .Sum(x => x.CurrentAmount);

                // Cart'ın güncellendiğini bildir
                _cartRepository.Update(cart);

                // 5. TEK SEFERDE KAYDET
                // Hem yeni item, hem update edilen item, hem de cart toplamı tek transaction'da gider.
                await unitOfWork.SaveChangesAsync();

                return await GetActiveCartAsync();
            }
            catch (Exception ex)
            {
                return ProduceFailResponse<CartViewDto>("Sepete ekleme sırasında hata oluştu: " + ex.Message, HrStatusCodes.Status110DatabaseError);
            }
        }

        // 3. SEPETTEN ÇIKAR
        public async Task<Response<CartViewDto>> RemoveFromCartAsync(long cartItemId)
        {
            try
            {
                var item = await _cartItemRepository.GetAsync(w => w.Id == cartItemId);
                if (item == null) return ProduceFailResponse<CartViewDto>("Ürün bulunamadı.", HrStatusCodes.Status111DataNotFound);

                item.IsDelete = true;
                item.IsActive = false;
                item.DeleteDate = DateTime.UtcNow;
                item.DeleteUserId = GetCurrentUserId();

                _cartItemRepository.Update(item);
                await unitOfWork.SaveChangesAsync();

                await RecalculateCartTotal(item.CartId);

                return await GetActiveCartAsync();
            }
            catch (Exception ex)
            {
                return ProduceFailResponse<CartViewDto>("Sepetten çıkarma işleminde hata oluştu.", HrStatusCodes.Status110DatabaseError);
            }
        }

        // 4. SİPARİŞ VER (CHECKOUT)
        public async Task<Response<CommonResponse>> CheckoutCartAsync(long cartId, string note)
        {
            try
            {
                var cart = await _cartRepository.GetAsync(w => w.Id == cartId);
                if (cart == null) return ProduceFailResponse<CommonResponse>("Sepet bulunamadı.", HrStatusCodes.Status111DataNotFound);

                long pendingStatusId = await _cartStatusRepository.GetIdByCodeAsync(CartStatusCodes.PendingApproval);
                if (pendingStatusId == 0) return ProduceFailResponse<CommonResponse>("Sistem Hatası: Onay durumu bulunamadı.", HrStatusCodes.Status111DataNotFound);

                cart.StatusId = pendingStatusId;
                cart.ConfirmNotes = note;
                cart.UpdateDate = DateTime.UtcNow;
                cart.UpdateUserId = GetCurrentUserId();

                _cartRepository.Update(cart);
                await unitOfWork.SaveChangesAsync();

                return ProduceSuccessResponse(new CommonResponse
                {
                    Result = true,
                    Message = "Siparişiniz başarıyla oluşturuldu ve onaya gönderildi.",
                    Code = HrStatusCodes.Status200OK
                });
            }
            catch (Exception ex)
            {
                return ProduceFailResponse<CommonResponse>("Sipariş oluşturulurken hata oluştu.", HrStatusCodes.Status503ServiceUnavailableError);
            }
        }

        // 5. LİSANS SAYISI GÜNCELLE
        public async Task<Response<CartViewDto>> UpdateLicenceCountAsync(long cartItemId, int count)
        {
            try
            {
                // Item ve bağlı eğitimi (fiyat için) çek
                var item = await _cartItemRepository.GetAsync(
                    predicate: w => w.Id == cartItemId,
                    include: i => i.Include(c => c.Training).ThenInclude(t => t.PriceTier).ThenInclude(pt => pt.Details)
                                   .Include(c => c.Training).ThenInclude(t => t.PriceTier).ThenInclude(pt => pt.CampaignPriceTiers).ThenInclude(cpt => cpt.Campaign)
                );

                if (item == null) return ProduceFailResponse<CartViewDto>("Kayıt bulunamadı", HrStatusCodes.Status111DataNotFound);
                if (count < 1) count = 1;

                // Fiyatı Yeniden Hesapla (Adet değişince Tier değişebilir!)
                if (item.Training != null)
                {
                    var priceInfo = CalculatePricing(item.Training, count);

                    item.LicenceCount = count;
                    item.Amount = priceInfo.UnitSellingPrice; // Yeni Birim Fiyat
                    item.DiscountRate = priceInfo.DiscountRate;
                    item.CurrentAmount = priceInfo.TotalPrice; // Yeni Toplam
                }
                else
                {
                    // Eğitim silinmişse eski fiyatla devam et (Fail safe)
                    item.LicenceCount = count;
                    item.CurrentAmount = item.Amount * count;
                }

                item.UpdateDate = DateTime.UtcNow;
                item.UpdateUserId = GetCurrentUserId();

                _cartItemRepository.Update(item);
                await unitOfWork.SaveChangesAsync();

                await RecalculateCartTotal(item.CartId);

                return await GetActiveCartAsync();
            }
            catch (Exception ex)
            {
                return ProduceFailResponse<CartViewDto>("Güncelleme sırasında hata oluştu.", HrStatusCodes.Status110DatabaseError);
            }
        }

        // =================================================================================================
        // PRIVATE HELPERS
        // =================================================================================================

        private async Task RecalculateCartTotal(long cartId)
        {
            // Cart'ı tekrar çekmeye gerek yok aslında ama güvenli olması için çekiyoruz.
            // AsNoTracking kullanabiliriz performans için ama Update yapacağız.
            var cart = await _cartRepository.GetAsync(
                predicate: x => x.Id == cartId,
                include: i => i.Include(c => c.CartItems)
            );

            if (cart != null)
            {
                cart.TotalAmount = cart.CartItems
                    .Where(x => x.IsDelete != true && x.IsActive == true)
                    .Sum(x => x.CurrentAmount);

                _cartRepository.Update(cart);
                await unitOfWork.SaveChangesAsync();
            }
        }

        private CartViewDto MapToCartViewDto(Cart cart)
        {
            var activeItems = cart.CartItems
                .Where(x => x.IsDelete != true && x.IsActive == true)
                .OrderBy(x => x.CreatedDate)
                .ToList();

            return new CartViewDto
            {
                CartId = cart.Id,
                TotalAmount = cart.TotalAmount,
                PromotionCode = cart.PromotionCode,
                TotalItemCount = activeItems.Count,
                Items = activeItems.Select(x => new CartItemViewDto
                {
                    Id = x.Id,
                    TrainingId = x.TrainingId,
                    TrainingTitle = x.Training?.Title ?? "Bilinmeyen Eğitim",
                    TrainingImage = (!string.IsNullOrEmpty(x.Training?.HeaderImage) && x.Training.HeaderImage != "none")
                                    ? x.Training.HeaderImage
                                    : "none",
                    CategoryName = x.Training?.TrainingCategory?.Title ?? "Genel",
                    Amount = x.Amount,
                    DiscountRate = x.DiscountRate,
                    CurrentAmount = x.CurrentAmount,
                    TaxRate = x.TaxRate,
                    LicenceCount = x.LicenceCount,
                    RowTotal = x.CurrentAmount
                }).ToList()
            };
        }

        /// <summary>
        /// Sepete atılan adede göre birim ve toplam fiyatı hesaplar.
        /// </summary>
        private (decimal UnitSellingPrice, decimal TotalPrice, decimal DiscountRate) CalculatePricing(Training training, int quantity)
        {
            if (training.PriceTier == null || training.PriceTier.Details == null || !training.PriceTier.Details.Any())
                return (0, 0, 0);

            // 1. ADIM: Adede uygun kuralı bul (Hacim İskontosu)
            // Örn: 1-10 arası 100 TL, 11-50 arası 90 TL
            var applicableRule = training.PriceTier.Details
                .Where(d => d.IsActive && quantity >= d.MinLicenceCount && quantity <= d.MaxLicenceCount)
                .FirstOrDefault();

            // Eğer tam aralık bulunamazsa (örn: 1000 tane aldı ama max kural 500), en yüksek miktarlı kuralı uygula
            if (applicableRule == null)
            {
                applicableRule = training.PriceTier.Details.OrderByDescending(d => d.MinLicenceCount).FirstOrDefault();
            }

            // Baz Fiyatı Belirle (1 Kişilik fiyatı referans alalım - İndirim oranı hesabı için)
            var baseRule = training.PriceTier.Details.OrderBy(d => d.MinLicenceCount).FirstOrDefault();
            decimal baseListPrice = baseRule?.Amount ?? 0;

            decimal unitPrice = 0;

            if (applicableRule != null)
            {
                if (applicableRule.Amount > 0)
                {
                    unitPrice = applicableRule.Amount;
                }
                else if (applicableRule.DiscountRate > 0)
                {
                    // Eğer sadece oran girildiyse, Base Fiyat üzerinden düş
                    unitPrice = baseListPrice - (baseListPrice * applicableRule.DiscountRate / 100);
                }
            }

            // 2. ADIM: KAMPANYA KONTROLÜ (Ekstra İndirim)
            decimal campaignDiscountRate = 0;
            if (training.PriceTier.CampaignPriceTiers != null && training.PriceTier.CampaignPriceTiers.Any())
            {
                var activeCampaign = training.PriceTier.CampaignPriceTiers
                    .Select(cpt => cpt.Campaign)
                    .FirstOrDefault(c => c.IsActive &&
                                         c.StartDate <= DateTime.UtcNow &&
                                         c.EndDate >= DateTime.UtcNow);

                if (activeCampaign != null)
                {
                    if (activeCampaign.Type == CampaignType.PercentageDiscount)
                    {
                        campaignDiscountRate = activeCampaign.Value;
                        unitPrice = unitPrice - (unitPrice * campaignDiscountRate / 100);
                    }
                    else if (activeCampaign.Type == CampaignType.FixedAmountDiscount)
                    {
                        unitPrice = unitPrice - activeCampaign.Value;
                    }
                }
            }

            if (unitPrice < 0) unitPrice = 0;

            // 3. ADIM: Sonuçları Hesapla
            decimal totalPrice = unitPrice * quantity;
            decimal totalDiscountRate = 0;

            // İndirim oranı: (Liste Fiyatı - Satış Fiyatı) / Liste Fiyatı
            // Not: Buradaki oran hem Hacim hem Kampanya indiriminin birleşimidir.
            if (baseListPrice > 0)
            {
                totalDiscountRate = ((baseListPrice - unitPrice) / baseListPrice) * 100;
            }

            return (unitPrice, totalPrice, totalDiscountRate);
        }
    }
}