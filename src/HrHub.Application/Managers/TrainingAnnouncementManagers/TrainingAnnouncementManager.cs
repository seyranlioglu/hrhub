using AutoMapper;
using FluentValidation.Results;
using HrHub.Abstraction.Extensions;
using HrHub.Abstraction.Result;
using HrHub.Application.BusinessRules.TrainingAnnouncementBusinessRules;
using HrHub.Application.Managers.TrainingAnnouncementManagers;
using HrHub.Core.Base;
using HrHub.Core.Data.Repository;
using HrHub.Core.Helpers;
using HrHub.Core.HrFluentValidation;
using HrHub.Domain.Contracts.Dtos.TrainingAnnouncementDtos;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;
using Microsoft.AspNetCore.Http;

namespace HrHub.Application.Managers.CommentManagers
{
    internal class TrainingAnnouncementManager : ManagerBase, ITrainingAnnouncementManager
    {
        private readonly IHrUnitOfWork hrUnitOfWork;
        private readonly IMapper mapper;
        private readonly Repository<TrainingAnnouncement> trainingAnnouncementRepository;
        public TrainingAnnouncementManager(IHttpContextAccessor httpContextAccessor, IHrUnitOfWork hrUnitOfWork, IMapper mapper) : base(httpContextAccessor)
        {
            this.hrUnitOfWork = hrUnitOfWork;
            this.mapper = mapper;
            this.trainingAnnouncementRepository = hrUnitOfWork.CreateRepository<TrainingAnnouncement>();
        }

        public async Task<Response<CommonResponse>> AddTrainingAnnouncementAsync(AddTrainingAnnouncementDto data, CancellationToken cancellationToken = default)
        {
            var validator = new FieldBasedValidator<AddTrainingAnnouncementDto>();
            var validateResult = validator.Validate(data);

            if (!validateResult.IsValid)
                return validateResult.SendResponse<CommonResponse>();

            if (ValidationHelper.RuleBasedValidator<AddTrainingAnnouncementDto>(data, typeof(IAddTrainingAnnouncementBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<CommonResponse>();

            var entity = mapper.Map<TrainingAnnouncement>(data);
            entity.CreateUserId = GetCurrentUserId();
            entity.CreatedDate = DateTime.UtcNow;
            entity.IsDelete = false;
            entity.IsActive = true;
            await trainingAnnouncementRepository.AddAsync(entity);
            await hrUnitOfWork.SaveChangesAsync();
            return ProduceSuccessResponse(new CommonResponse { Result = true, Code = StatusCodes.Status200OK, Message = "İşlem Tamamlandı." });
        }
        public async Task<Response<CommonResponse>> UpdateTrainingAnnouncementAsync(UpdateTrainingAnnouncementDto dto, CancellationToken cancellationToken = default)
        {
            var validator = new FieldBasedValidator<UpdateTrainingAnnouncementDto>();
            var validateResult = validator.Validate(dto);

            if (!validateResult.IsValid)
                return validateResult.SendResponse<CommonResponse>();

            if (ValidationHelper.RuleBasedValidator<UpdateTrainingAnnouncementDto>(dto, typeof(IUpdateTrainingAnnouncementBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<CommonResponse>();

            var data = await trainingAnnouncementRepository.GetAsync(p => p.Id == dto.Id);
            var entity = mapper.Map(dto, data);
            entity.UpdateUserId = GetCurrentUserId();
            entity.UpdateDate = DateTime.UtcNow;
            trainingAnnouncementRepository.Update(entity);
            await hrUnitOfWork.SaveChangesAsync();
            return ProduceSuccessResponse(new CommonResponse { Result = true, Code = StatusCodes.Status200OK, Message = "İşlem Tamamlandı." });
        }
        public async Task<Response<CommonResponse>> DeleteTrainingAnnouncementAsync(long id, CancellationToken cancellationToken = default)
        {
            if (ValidationHelper.RuleBasedValidator<UpdateTrainingAnnouncementDto>(new UpdateTrainingAnnouncementDto { Id = id }, typeof(IUpdateTrainingAnnouncementBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<CommonResponse>();

            var entity = await trainingAnnouncementRepository.GetAsync(p => p.Id == id);
            entity.UpdateUserId = GetCurrentUserId();
            entity.UpdateDate = DateTime.UtcNow;
            entity.IsDelete = true;
            entity.IsActive = false;
            trainingAnnouncementRepository.Update(entity);
            await hrUnitOfWork.SaveChangesAsync();
            return ProduceSuccessResponse(new CommonResponse { Result = true, Code = StatusCodes.Status200OK, Message = "İşlem Tamamlandı." });
        }

        public async Task<Response<TrainingAnnouncementDto>> GetTrainingAnnouncementByIdAsync(long id)
        {
            var TrainingAnnouncement = await trainingAnnouncementRepository.GetAsync(predicate: p => p.Id == id && p.IsDelete != true);

            var TrainingAnnouncementDto = mapper.Map<TrainingAnnouncementDto>(TrainingAnnouncement);
            TrainingAnnouncementDto.Editable = TrainingAnnouncementDto.UserId == GetCurrentUserId();
            return ProduceSuccessResponse(TrainingAnnouncementDto);
        }

        public async Task<Response<IEnumerable<TrainingAnnouncementDto>>> GetTrainingAnnouncementListAsync()
        {
            var TrainingAnnouncement = await trainingAnnouncementRepository.GetListAsync(p => p.IsDelete != true, selector: s => mapper.Map<TrainingAnnouncementDto>(s));
            TrainingAnnouncement = TrainingAnnouncement.Select(s =>
            {
                s.Editable = s.UserId == GetCurrentUserId();
                return s;
            });
            return ProduceSuccessResponse(TrainingAnnouncement);
        }


    }
}
