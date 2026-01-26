using AutoMapper;
using HrHub.Abstraction.Result;
using HrHub.Abstraction.StatusCodes;
using HrHub.Core.Base;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Contracts.Dtos.PricingDtos; // Bu DTO'ları oluşturacağız
using HrHub.Domain.Contracts.Responses.CommonResponse;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrHub.Application.Managers.Pricing
{
    public class PricingManager : ManagerBase, IPricingManager
    {
        private readonly IHrUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly Repository<PriceTier> _priceTierRepository;
        private readonly Repository<PriceTierDetail> _priceTierDetailRepository;
        private readonly Repository<SubscriptionPlan> _subscriptionPlanRepository;

        public PricingManager(IHttpContextAccessor httpContextAccessor,
                              IHrUnitOfWork unitOfWork,
                              IMapper mapper) : base(httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _priceTierRepository = unitOfWork.CreateRepository<PriceTier>();
            _priceTierDetailRepository = unitOfWork.CreateRepository<PriceTierDetail>();
            _subscriptionPlanRepository = unitOfWork.CreateRepository<SubscriptionPlan>();
        }

        // ====================================================================================
        // A. PRICE TIER YÖNETİMİ
        // ====================================================================================

        public async Task<Response<List<PriceTierDto>>> GetAllPriceTiersAsync(bool activeOnly = true)
        {
            var query = _priceTierRepository.GetQuery(
                predicate: x => x.IsDelete != true,
                include: i => i.Include(pt => pt.Details)
            );

            if (activeOnly) query = query.Where(x => x.IsActive == true);

            var tiers = await query.OrderBy(x => x.Id).ToListAsync();

            // Mapping
            var dtoList = tiers.Select(x => new PriceTierDto
            {
                Id = x.Id,
                Title = x.Title,
                Code = x.Code,
                Description = x.Description,
                Currency = x.Currency,
                IsActive = x.IsActive,
                // Detayları da içine gömüyoruz
                Details = x.Details.Where(d => d.IsDelete != true && d.IsActive == true)
                                   .OrderBy(d => d.MinLicenceCount)
                                   .Select(d => new PriceTierDetailDto
                                   {
                                       Id = d.Id,
                                       MinLicenceCount = d.MinLicenceCount,
                                       MaxLicenceCount = d.MaxLicenceCount,
                                       Amount = d.Amount,
                                       DiscountRate = d.DiscountRate
                                   }).ToList()
            }).ToList();

            return ProduceSuccessResponse(dtoList);
        }

        public async Task<Response<ReturnIdResponse>> CreatePriceTierAsync(CreatePriceTierDto dto)
        {
            var entity = _mapper.Map<PriceTier>(dto);
            entity.CreatedDate = DateTime.UtcNow;
            entity.CreateUserId = GetCurrentUserId();
            entity.IsActive = true;

            await _priceTierRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return ProduceSuccessResponse(new ReturnIdResponse { Id = entity.Id });
        }

        public async Task<Response<CommonResponse>> UpdatePriceTierAsync(UpdatePriceTierDto dto)
        {
            var entity = await _priceTierRepository.GetAsync(x => x.Id == dto.Id);
            if (entity == null) return ProduceFailResponse<CommonResponse>("Kayıt bulunamadı", HrStatusCodes.Status404NotFound);

            _mapper.Map(dto, entity);
            entity.UpdateDate = DateTime.UtcNow;
            entity.UpdateUserId = GetCurrentUserId();

            _priceTierRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return ProduceSuccessResponse(new CommonResponse { Result = true, Message = "Güncellendi" });
        }

        public async Task<Response<CommonResponse>> DeletePriceTierAsync(long id)
        {
            // Kullanımda mı kontrolü (Training tablosunda var mı?)
            // Şimdilik soft delete yapıyoruz.
            var entity = await _priceTierRepository.GetAsync(x => x.Id == id);
            if (entity == null) return ProduceFailResponse<CommonResponse>("Kayıt bulunamadı", HrStatusCodes.Status404NotFound);

            entity.IsDelete = true;
            entity.DeleteDate = DateTime.UtcNow;
            entity.DeleteUserId = GetCurrentUserId();

            _priceTierRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return ProduceSuccessResponse(new CommonResponse { Result = true, Message = "Silindi" });
        }

        // ====================================================================================
        // B. PRICE TIER DETAIL (MATRİS) YÖNETİMİ
        // ====================================================================================

        public async Task<Response<CommonResponse>> AddOrUpdateTierDetailAsync(TierDetailDto dto)
        {
            if (dto.Id > 0)
            {
                // Güncelleme
                var entity = await _priceTierDetailRepository.GetAsync(x => x.Id == dto.Id);
                if (entity != null)
                {
                    entity.MinLicenceCount = dto.MinLicenceCount;
                    entity.MaxLicenceCount = dto.MaxLicenceCount;
                    entity.Amount = dto.Amount;
                    entity.DiscountRate = dto.DiscountRate;
                    entity.UpdateDate = DateTime.UtcNow;
                    entity.UpdateUserId = GetCurrentUserId();
                    _priceTierDetailRepository.Update(entity);
                }
            }
            else
            {
                // Ekleme
                var entity = new PriceTierDetail
                {
                    PriceTierId = dto.PriceTierId,
                    MinLicenceCount = dto.MinLicenceCount,
                    MaxLicenceCount = dto.MaxLicenceCount,
                    Amount = dto.Amount,
                    DiscountRate = dto.DiscountRate,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    CreateUserId = GetCurrentUserId()
                };
                await _priceTierDetailRepository.AddAsync(entity);
            }

            await _unitOfWork.SaveChangesAsync();
            return ProduceSuccessResponse(new CommonResponse { Result = true, Message = "İşlem Başarılı" });
        }

        public async Task<Response<CommonResponse>> DeleteTierDetailAsync(long detailId)
        {
            var entity = await _priceTierDetailRepository.GetAsync(x => x.Id == detailId);
            if (entity == null) return ProduceFailResponse<CommonResponse>("Detay bulunamadı", HrStatusCodes.Status404NotFound);

            _priceTierDetailRepository.Delete(entity); // Detaylar hard delete olabilir veya soft delete
            await _unitOfWork.SaveChangesAsync();

            return ProduceSuccessResponse(new CommonResponse { Result = true, Message = "Silindi" });
        }

        // ====================================================================================
        // C. SUBSCRIPTION PLAN (ABONELİK) YÖNETİMİ
        // ====================================================================================

        public async Task<Response<List<SubscriptionPlanDto>>> GetSubscriptionPlansAsync()
        {
            var plans = await _subscriptionPlanRepository.GetListAsync(
                predicate: x => x.IsActive == true && x.IsDelete != true,
                orderBy: o => o.OrderBy(x => x.Price)
            );

            var result = plans.Select(x => new SubscriptionPlanDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Price = x.Price,
                Currency = x.Currency,
                UserCountRange = $"{x.MinUserCount} - {x.MaxUserCount} Kullanıcı",
                MonthlyCredit = x.TotalMonthlyCredit,
                DurationText = $"{x.DurationMonths} Ay",
                // JSON String'i Listeye Çevir (Frontend için kolaylık)
                FeaturesList = !string.IsNullOrEmpty(x.Features)
                               ? JsonConvert.DeserializeObject<List<string>>(x.Features)
                               : new List<string>()
            }).ToList();

            return ProduceSuccessResponse(result);
        }

        public async Task<Response<ReturnIdResponse>> CreateSubscriptionPlanAsync(CreateSubscriptionPlanDto dto)
        {
            var entity = _mapper.Map<SubscriptionPlan>(dto);

            // Features listesini JSON string'e çevir
            if (dto.FeaturesList != null && dto.FeaturesList.Any())
            {
                entity.Features = JsonConvert.SerializeObject(dto.FeaturesList);
            }

            entity.CreatedDate = DateTime.UtcNow;
            entity.CreateUserId = GetCurrentUserId();
            entity.IsActive = true;

            await _subscriptionPlanRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return ProduceSuccessResponse(new ReturnIdResponse { Id = entity.Id });
        }

        public async Task<Response<CommonResponse>> ToggleSubscriptionPlanStatusAsync(long id)
        {
            var entity = await _subscriptionPlanRepository.GetAsync(x => x.Id == id);
            if (entity == null) return ProduceFailResponse<CommonResponse>("Plan bulunamadı", HrStatusCodes.Status404NotFound);

            entity.IsActive = !entity.IsActive;
            entity.UpdateDate = DateTime.UtcNow;
            entity.UpdateUserId = GetCurrentUserId();

            _subscriptionPlanRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return ProduceSuccessResponse(new CommonResponse { Result = true, Message = "Durum Değiştirildi" });
        }

        public async Task<Response<List<PriceTierDetailDto>>> GetPricingDetailsByTierIdAsync(long tierId)
        {
            // Doğrudan PriceTierDetails tablosuna gidiyoruz. Training tablosuna JOIN atmaya gerek yok.
            var details = await _priceTierDetailRepository.GetListAsync(
                predicate: d => d.PriceTierId == tierId && d.IsActive == true && d.IsDelete != true,
                orderBy: o => o.OrderBy(d => d.MinLicenceCount)
            );

            if (!details.Any())
                return ProduceFailResponse<List<PriceTierDetailDto>>("Fiyat tarifesi bulunamadı.", HrStatusCodes.Status404NotFound);

            var dtoList = details.Select(d => new PriceTierDetailDto
            {
                Id = d.Id,
                MinLicenceCount = d.MinLicenceCount,
                MaxLicenceCount = d.MaxLicenceCount,
                Amount = d.Amount,
                DiscountRate = d.DiscountRate
            }).ToList();

            return ProduceSuccessResponse(dtoList);
        }
    }
}