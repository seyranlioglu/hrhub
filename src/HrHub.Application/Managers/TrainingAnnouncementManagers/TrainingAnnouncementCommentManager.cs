using AutoMapper;
using FluentValidation.Results;
using HrHub.Abstraction.Extensions;
using HrHub.Abstraction.Result;
using HrHub.Application.BusinessRules.TrainingAnnouncementCommentBusinessRules;
using HrHub.Application.Managers.TrainingAnnouncementCommentManagers;
using HrHub.Core.Base;
using HrHub.Core.Data.Repository;
using HrHub.Core.Helpers;
using HrHub.Core.HrFluentValidation;
using HrHub.Domain.Contracts.Dtos.TrainingAnnouncementCommentDtos;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;
using Microsoft.AspNetCore.Http;

namespace HrHub.Application.Managers.CommentManagers
{
    internal class TrainingAnnouncementCommentManager : ManagerBase, ITrainingAnnouncementCommentManager
    {
        private readonly IHrUnitOfWork hrUnitOfWork;
        private readonly IMapper mapper;
        private readonly Repository<TrainingAnnouncementsComment> trainingAnnouncementCommentRepository;
        public TrainingAnnouncementCommentManager(IHttpContextAccessor httpContextAccessor, IHrUnitOfWork hrUnitOfWork, IMapper mapper) : base(httpContextAccessor)
        {
            this.hrUnitOfWork = hrUnitOfWork;
            this.mapper = mapper;
            this.trainingAnnouncementCommentRepository = hrUnitOfWork.CreateRepository<TrainingAnnouncementsComment>();
        }

        public async Task<Response<CommonResponse>> AddTrainingAnnouncementCommentAsync(AddTrainingAnnouncementCommentDto data, CancellationToken cancellationToken = default)
        {
            var validator = new FieldBasedValidator<AddTrainingAnnouncementCommentDto>();
            var validateResult = validator.Validate(data);

            if (!validateResult.IsValid)
                return validateResult.SendResponse<CommonResponse>();

            if (ValidationHelper.RuleBasedValidator<AddTrainingAnnouncementCommentDto>(data, typeof(IAddTrainingAnnouncementCommentBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<CommonResponse>();

            var entity = mapper.Map<TrainingAnnouncementsComment>(data);
            entity.CreateUserId = GetCurrentUserId();
            entity.CreatedDate = DateTime.UtcNow;
            entity.IsDelete = false;
            entity.IsActive = true;
            await trainingAnnouncementCommentRepository.AddAsync(entity);
            await hrUnitOfWork.SaveChangesAsync();
            return ProduceSuccessResponse(new CommonResponse { Result = true, Code = StatusCodes.Status200OK, Message = "İşlem Tamamlandı." });
        }
        public async Task<Response<CommonResponse>> UpdateTrainingAnnouncementCommentAsync(UpdateTrainingAnnouncementCommentDto dto, CancellationToken cancellationToken = default)
        {
            var validator = new FieldBasedValidator<UpdateTrainingAnnouncementCommentDto>();
            var validateResult = validator.Validate(dto);

            if (!validateResult.IsValid)
                return validateResult.SendResponse<CommonResponse>();

            if (ValidationHelper.RuleBasedValidator<UpdateTrainingAnnouncementCommentDto>(dto, typeof(IUpdateTrainingAnnouncementCommentBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<CommonResponse>();

            var data = await trainingAnnouncementCommentRepository.GetAsync(p => p.Id == dto.Id);
            var entity = mapper.Map(dto, data);
            entity.UpdateUserId = GetCurrentUserId();
            entity.UpdateDate = DateTime.UtcNow;
            trainingAnnouncementCommentRepository.Update(entity);
            await hrUnitOfWork.SaveChangesAsync();
            return ProduceSuccessResponse(new CommonResponse { Result = true, Code = StatusCodes.Status200OK, Message = "İşlem Tamamlandı." });
        }
        public async Task<Response<CommonResponse>> DeleteTrainingAnnouncementCommentAsync(long id, CancellationToken cancellationToken = default)
        {
            if (ValidationHelper.RuleBasedValidator<UpdateTrainingAnnouncementCommentDto>(new UpdateTrainingAnnouncementCommentDto { Id = id }, typeof(IUpdateTrainingAnnouncementCommentBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<CommonResponse>();

            var entity = await trainingAnnouncementCommentRepository.GetAsync(p => p.Id == id);
            entity.UpdateUserId = GetCurrentUserId();
            entity.UpdateDate = DateTime.UtcNow;
            entity.IsDelete = true;
            entity.IsActive = false;
            trainingAnnouncementCommentRepository.Update(entity);
            await hrUnitOfWork.SaveChangesAsync();
            return ProduceSuccessResponse(new CommonResponse { Result = true, Code = StatusCodes.Status200OK, Message = "İşlem Tamamlandı." });
        }

        public async Task<Response<TrainingAnnouncementCommentDto>> GetTrainingAnnouncementCommentByIdAsync(long id)
        {
            var TrainingAnnouncementComment = await trainingAnnouncementCommentRepository.GetAsync(predicate: p => p.Id == id && p.IsDelete != true);

            var TrainingAnnouncementCommentDto = mapper.Map<TrainingAnnouncementCommentDto>(TrainingAnnouncementComment);
            TrainingAnnouncementCommentDto.Editable = TrainingAnnouncementCommentDto.UserId == GetCurrentUserId();
            return ProduceSuccessResponse(TrainingAnnouncementCommentDto);
        }

        public async Task<Response<IEnumerable<TrainingAnnouncementCommentDto>>> GetTrainingAnnouncementCommentListAsync()
        {
            var TrainingAnnouncementComment = await trainingAnnouncementCommentRepository.GetListAsync(p => p.IsDelete != true, selector: s => mapper.Map<TrainingAnnouncementCommentDto>(s));
            TrainingAnnouncementComment = TrainingAnnouncementComment.Select(s =>
            {
                s.Editable = s.UserId == GetCurrentUserId();
                return s;
            });
            return ProduceSuccessResponse(TrainingAnnouncementComment);
        }


    }
}
