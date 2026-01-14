using AutoMapper;
using HrHub.Abstraction.Contracts.Dtos.TrainingDtos;
using HrHub.Abstraction.Managers.TrainingProcessRequestManagers;
using HrHub.Abstraction.Result;
using HrHub.Abstraction.StatusCodes;
using HrHub.Core.Base;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Contracts.Dtos.TrainingProcessRequestDtos;
using HrHub.Domain.Contracts.Enums;
using HrHub.Domain.Contracts.Responses.CommonResponse;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HrHub.Application.Managers.TrainingProcessRequestManagers;

public class TrainingProcessRequestManager : ManagerBase, ITrainingProcessRequestManager
{
    private readonly IHrUnitOfWork _unitOfWork;
    private readonly Repository<TrainingProcessRequest> _requestRepository;
    private readonly Repository<TrainingStatus> _statusRepository;
    private readonly Repository<Training> _trainingRepository;
    private readonly Repository<CurrAccTrainingUser> _currAccTrainingUserRepository;

    public TrainingProcessRequestManager(
        IHttpContextAccessor httpContextAccessor,
        IHrUnitOfWork unitOfWork) : base(httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _requestRepository = unitOfWork.CreateRepository<TrainingProcessRequest>();
        _statusRepository = unitOfWork.CreateRepository<TrainingStatus>();
        _trainingRepository = unitOfWork.CreateRepository<Training>();
        _currAccTrainingUserRepository = unitOfWork.CreateRepository<CurrAccTrainingUser>();
    }

    /// <summary>
    /// Yeni bir işlem veya erişim talebi oluşturur.
    /// </summary>
    public async Task<Response<ReturnIdResponse>> CreateRequestAsync(CreateProcessRequestDto dto, CancellationToken cancellationToken = default)
    {
        // 1. Standart "Pending" statüsünü bul (Tüm talepler beklemede başlar)
        var pendingStatusId = await _statusRepository.GetAsync(
            predicate: x => x.Code == "Pending",
            selector: s => s.Id
        );

        if (pendingStatusId == 0)
            return ProduceFailResponse<ReturnIdResponse>("Sistem statü tanımı (Pending) bulunamadı.", StatusCodes.Status500InternalServerError);

        // 2. Talep mükerrerlik kontrolü (Eğer zaten bekleyen bir talep varsa yenisini açtırma)
        var existingRequest = await _requestRepository.ExistsAsync(x =>
            x.TrainingId == dto.TrainingId &&
            x.RequesterUserId == GetCurrentUserId() &&
            x.RequestType == dto.RequestType &&
            x.RequestStatusId == pendingStatusId);

        if (existingRequest)
            return ProduceFailResponse<ReturnIdResponse>("Zaten bekleyen bir talebiniz bulunmaktadır.", StatusCodes.Status400BadRequest);

        // 3. Entity oluşturma
        var newRequest = new TrainingProcessRequest
        {
            TrainingId = dto.TrainingId,
            RequestType = dto.RequestType,
            RequestStatusId = pendingStatusId,
            RequesterUserId = GetCurrentUserId(),
            CurrAccTrainingUserId = dto.CurrAccTrainingUserId,
            Note = dto.Note,
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            CreateUserId = GetCurrentUserId()
        };

        // Eğer bu bir eğitim yayınlama onayıysa, hedef statüyü "Published" olarak işaretle
        if (dto.RequestType == TrainingRequestType.PublishApproval)
        {
            newRequest.TargetStatusId = await _statusRepository.GetAsync(x => x.Code == "Published", selector : s => s.Id);
        }

        var result = await _requestRepository.AddAndReturnAsync(newRequest);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // TODO: NotificationManager.SendNotification(newRequest) -> Bildirim tetiği buraya gelecek

        return ProduceSuccessResponse(new ReturnIdResponse { Id = result.Id });
    }

    /// <summary>
    /// Gelen bir talebi onaylar veya reddeder.
    /// </summary>
    public async Task<Response<CommonResponse>> RespondToRequestAsync(long requestId, bool isApproved, string? adminNote, CancellationToken cancellationToken = default)
    {
        var request = await _requestRepository.GetAsync(
            predicate: x => x.Id == requestId,
            include: i => i.Include(r => r.Training).Include(r => r.CurrAccTrainingUser)
        );

        if (request == null)
            return ProduceFailResponse<CommonResponse>("Talep kaydı bulunamadı.", StatusCodes.Status404NotFound);

        var statusId = await _statusRepository.GetAsync(
            predicate: x => x.Code == (isApproved ? "Approved" : "Rejected"),
            selector: s => s.Id
        );

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            // 1. Talebi Güncelle
            request.RequestStatusId = statusId;
            request.ResponderUserId = GetCurrentUserId();
            request.ResponseDate = DateTime.UtcNow;
            request.Note = adminNote;
            request.UpdateDate = DateTime.UtcNow;
            request.UpdateUserId = GetCurrentUserId();

            _requestRepository.Update(request);

            // 2. Talebin Türüne Göre Yan Etkileri (Side Effects) Yönet
            if (isApproved)
            {
                switch (request.RequestType)
                {
                    case TrainingRequestType.PublishApproval:
                        // Eğitimin genel statüsünü "Published" yap
                        if (request.TargetStatusId.HasValue)
                        {
                            var training = await _trainingRepository.GetAsync(x => x.Id == request.TrainingId);
                            training.TrainingStatusId = request.TargetStatusId.Value;
                            _trainingRepository.Update(training);
                        }
                        break;

                    case TrainingRequestType.AccessExtension:
                        // Kullanıcının eğitim süresini uzat (Örn: +15 gün)
                        if (request.CurrAccTrainingUserId.HasValue)
                        {
                            var assignment = await _currAccTrainingUserRepository.GetAsync(x => x.Id == request.CurrAccTrainingUserId.Value);
                            // Mevcut tarihe veya bugüne ekleme mantığı
                            assignment.DueDate = DateTime.UtcNow.AddDays(15);
                            _currAccTrainingUserRepository.Update(assignment);
                        }
                        break;
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync();

            return ProduceSuccessResponse(new CommonResponse
            {
                Result = true,
                Message = isApproved ? "Talep onaylandı." : "Talep reddedildi.",
                Code = StatusCodes.Status200OK
            });
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollBackTransactionAsync();
            return ProduceFailResponse<CommonResponse>($"İşlem başarısız: {ex.Message}", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Bekleyen talepleri listeler (Admin paneli için).
    /// </summary>
    public async Task<Response<IEnumerable<ProcessRequestListDto>>> GetPendingRequestsAsync()
    {
        var pendingStatusId = await _statusRepository.GetAsync(predicate : x => x.Code == "Pending", selector: s => s.Id);

        var requests = await _requestRepository.GetListAsync(
            predicate: x => x.RequestStatusId == pendingStatusId && x.IsActive == true,
            include: i => i.Include(r => r.Training).Include(r => r.RequesterUser),
            selector: r => new ProcessRequestListDto
            {
                Id = r.Id,
                TrainingTitle = r.Training.Title ?? "",
                RequesterFullName = $"{r.RequesterUser.Name} {r.RequesterUser.SurName}",
                RequestTypeName = r.RequestType.ToString(), // Enum'dan string'e
                CreatedDate = r.CreatedDate,
                Note = r.Note
            }
        );

        return ProduceSuccessResponse(requests);
    }
}