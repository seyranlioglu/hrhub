using AutoMapper;
using HrHub.Abstraction.Result;
using HrHub.Abstraction.StatusCodes;
using HrHub.Core.Base;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Contracts.Dtos.CampaignDtos; // Bu DTO'lar oluşturulmalı
using HrHub.Domain.Contracts.Enums;
using HrHub.Domain.Contracts.Responses.CommonResponse;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.Repositories.Abstract;
using HrHub.Infrastructre.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrHub.Application.Managers.Campaigns
{
    public class CampaignManager : ManagerBase, ICampaignManager
    {
        private readonly IHrUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly Repository<Campaign> _campaignRepository;
        private readonly Repository<CampaignParticipant> _participantRepository;
        private readonly Repository<CampaignPriceTier> _campaignTierRepository;
        private readonly IInstructorRepository _instructorRepository;

        public CampaignManager(IHttpContextAccessor httpContextAccessor,
                               IHrUnitOfWork unitOfWork,
                               IMapper mapper,
                               IInstructorRepository instructorRepository) : base(httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _instructorRepository = instructorRepository;
            _campaignRepository = unitOfWork.CreateRepository<Campaign>();
            _participantRepository = unitOfWork.CreateRepository<CampaignParticipant>();
            _campaignTierRepository = unitOfWork.CreateRepository<CampaignPriceTier>();
        }

        // ====================================================================================
        // A. KAMPANYA YÖNETİMİ (ADMIN)
        // ====================================================================================

        public async Task<Response<List<CampaignListDto>>> GetCampaignsAsync(bool activeOnly = false)
        {
            var query = _campaignRepository.GetQuery(predicate: x => x.IsDelete != true);

            if (activeOnly)
            {
                query = query.Where(x => x.IsActive == true && x.EndDate >= DateTime.UtcNow);
            }

            var list = await query.OrderByDescending(x => x.CreatedDate).ToListAsync();
            var dtos = _mapper.Map<List<CampaignListDto>>(list);

            return ProduceSuccessResponse(dtos);
        }

        public async Task<Response<ReturnIdResponse>> CreateCampaignAsync(CreateCampaignDto dto)
        {
            var entity = _mapper.Map<Campaign>(dto);
            entity.CreatedDate = DateTime.UtcNow;
            entity.CreateUserId = GetCurrentUserId();
            entity.IsActive = true;

            // Tier Atamaları (Eğer varsa)
            if (dto.TargetPriceTierIds != null && dto.TargetPriceTierIds.Any())
            {
                foreach (var tierId in dto.TargetPriceTierIds)
                {
                    entity.CampaignPriceTiers.Add(new CampaignPriceTier { PriceTierId = tierId });
                }
            }

            await _campaignRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return ProduceSuccessResponse(new ReturnIdResponse { Id = entity.Id });
        }

        public async Task<Response<CommonResponse>> ToggleCampaignStatusAsync(long id)
        {
            var entity = await _campaignRepository.GetAsync(x => x.Id == id);
            if (entity == null) return ProduceFailResponse<CommonResponse>("Kampanya bulunamadı", HrStatusCodes.Status404NotFound);

            entity.IsActive = !entity.IsActive;
            entity.UpdateDate = DateTime.UtcNow;
            entity.UpdateUserId = GetCurrentUserId();

            _campaignRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return ProduceSuccessResponse(new CommonResponse { Result = true, Message = "Durum Değiştirildi" });
        }

        // ====================================================================================
        // B. KATILIM YÖNETİMİ (EĞİTMEN)
        // ====================================================================================

        // Eğitmenin katılabileceği kampanyaları listeler (Opt-In olanlar)
        public async Task<Response<List<CampaignOpportunityDto>>> GetAvailableCampaignsForInstructorAsync()
        {
            long userId = GetCurrentUserId();
            var instructor = await _instructorRepository.GetAsync(x => x.UserId == userId);
            if (instructor == null) return ProduceFailResponse<List<CampaignOpportunityDto>>("Eğitmen profili bulunamadı", HrStatusCodes.Status403Forbidden);

            // 1. Aktif, Opt-In tipindeki ve süresi dolmamış kampanyaları çek
            var campaigns = await _campaignRepository.GetListAsync(
                predicate: x => x.IsActive == true &&
                                x.IsDelete != true &&
                                x.Scope == CampaignScope.OptIn &&
                                x.EndDate > DateTime.UtcNow
            );

            // 2. Eğitmenin daha önce katıldıklarını bul
            var myParticipations = await _participantRepository.GetListAsync(
                predicate: x => x.InstructorId == instructor.Id &&
                                (x.Status == ParticipationStatus.Approved || x.Status == ParticipationStatus.Pending)
            );
            var joinedCampaignIds = myParticipations.Select(x => x.CampaignId).ToList();

            // 3. Mapping
            var result = campaigns.Select(c => new CampaignOpportunityDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                DiscountValue = c.Value,
                IsJoined = joinedCampaignIds.Contains(c.Id) // Daha önce katılmış mı?
            }).ToList();

            return ProduceSuccessResponse(result);
        }

        // Eğitmenin kampanyaya katılması (OTOMATİK ONAYLI)
        public async Task<Response<CommonResponse>> JoinCampaignAsync(JoinCampaignDto dto)
        {
            long userId = GetCurrentUserId();
            var instructor = await _instructorRepository.GetAsync(x => x.UserId == userId);
            if (instructor == null) return ProduceFailResponse<CommonResponse>("Yetkisiz işlem", HrStatusCodes.Status403Forbidden);

            var campaign = await _campaignRepository.GetAsync(x => x.Id == dto.CampaignId);
            if (campaign == null || campaign.Scope != CampaignScope.OptIn)
                return ProduceFailResponse<CommonResponse>("Bu kampanyaya katılım yapılamaz.", HrStatusCodes.Status400BadRequest);

            // Zaten katılmış mı?
            var existing = await _participantRepository.GetAsync(x => x.CampaignId == dto.CampaignId && x.InstructorId == instructor.Id);
            if (existing != null)
            {
                // Eğer daha önce çıkmışsa (OptOut), tekrar aktif et
                if (existing.Status == ParticipationStatus.OptOut || existing.Status == ParticipationStatus.Rejected)
                {
                    existing.Status = ParticipationStatus.Approved; // Tekrar Onayla
                    existing.ActionDate = DateTime.UtcNow;
                    _participantRepository.Update(existing);
                }
                else
                {
                    return ProduceSuccessResponse(new CommonResponse { Result = true, Message = "Zaten katılımınız mevcut." });
                }
            }
            else
            {
                // Yeni Katılım (Otomatik Onaylı)
                var participant = new CampaignParticipant
                {
                    CampaignId = dto.CampaignId,
                    InstructorId = instructor.Id,
                    TrainingId = dto.TrainingId, // Opsiyonel: Null ise tüm eğitimler
                    Status = ParticipationStatus.Approved, // <--- OTOMATİK ONAY BURADA
                    ActionDate = DateTime.UtcNow,
                    CreatedDate = DateTime.UtcNow,
                    CreateUserId = userId
                };
                await _participantRepository.AddAsync(participant);
            }

            await _unitOfWork.SaveChangesAsync();
            return ProduceSuccessResponse(new CommonResponse { Result = true, Message = "Kampanyaya katılımınız onaylandı." });
        }

        public async Task<Response<CommonResponse>> ExitCampaignAsync(long campaignId)
        {
            long userId = GetCurrentUserId();
            var instructor = await _instructorRepository.GetAsync(x => x.UserId == userId);

            var participant = await _participantRepository.GetAsync(x => x.CampaignId == campaignId && x.InstructorId == instructor.Id);
            if (participant == null) return ProduceFailResponse<CommonResponse>("Katılım bulunamadı", HrStatusCodes.Status404NotFound);

            participant.Status = ParticipationStatus.OptOut; // Çıkış yapıldı
            participant.ActionDate = DateTime.UtcNow;

            _participantRepository.Update(participant);
            await _unitOfWork.SaveChangesAsync();

            return ProduceSuccessResponse(new CommonResponse { Result = true, Message = "Kampanyadan ayrıldınız." });
        }
    }
}