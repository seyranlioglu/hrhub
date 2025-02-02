using AutoMapper;
using FluentValidation.Results;
using HrHub.Abstraction.Extensions;
using HrHub.Abstraction.Result;
using HrHub.Application.BusinessRules.ContentCommentBusinessRules;
using HrHub.Core.Base;
using HrHub.Core.Data.Repository;
using HrHub.Core.Helpers;
using HrHub.Core.HrFluentValidation;
using HrHub.Domain.Contracts.Dtos.ContentCommentDtos;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;
using Microsoft.AspNetCore.Http;

namespace HrHub.Application.Managers.CommentManagers
{
    public class ContentCommentManager : ManagerBase, IContentCommentManager
    {
        private readonly IHrUnitOfWork hrUnitOfWork;
        private readonly IMapper mapper;
        private readonly Repository<ContentComment> contentCommentRepository;
        public ContentCommentManager(IHttpContextAccessor httpContextAccessor, IHrUnitOfWork hrUnitOfWork, IMapper mapper) : base(httpContextAccessor)
        {
            this.hrUnitOfWork = hrUnitOfWork;
            this.mapper = mapper;
            this.contentCommentRepository = hrUnitOfWork.CreateRepository<ContentComment>();
        }

        public async Task<Response<CommonResponse>> AddContentCommentAsync(AddContentCommentDto data, CancellationToken cancellationToken = default)
        {
            

            var validator = new FieldBasedValidator<AddContentCommentDto>();
            var validateResult = validator.Validate(data);

            if (!validateResult.IsValid)
                return validateResult.SendResponse<CommonResponse>();

            if (ValidationHelper.RuleBasedValidator<AddContentCommentDto>(data, typeof(IAddContentCommentBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<CommonResponse>();

            var entity = mapper.Map<ContentComment>(data);
            entity.CreateUserId = GetCurrentUserId();
            entity.CreatedDate = DateTime.UtcNow;
            entity.IsDelete = false;
            entity.IsActive = true;
            await contentCommentRepository.AddAsync(entity);
            await hrUnitOfWork.SaveChangesAsync();
            return ProduceSuccessResponse(new CommonResponse { Result = true, Code = StatusCodes.Status200OK, Message = "İşlem Tamamlandı." });
        }
        public async Task<Response<CommonResponse>> UpdateContentCommentAsync(UpdateContentCommentDto dto, CancellationToken cancellationToken = default)
        {
            

            var validator = new FieldBasedValidator<UpdateContentCommentDto>();
            var validateResult = validator.Validate(dto);

            if (!validateResult.IsValid)
                return validateResult.SendResponse<CommonResponse>();

            if (ValidationHelper.RuleBasedValidator<UpdateContentCommentDto>(dto, typeof(IUpdateContentCommentBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<CommonResponse>();

            var data = await contentCommentRepository.GetAsync(p => p.Id == dto.Id);
            var entity = mapper.Map(dto, data);
            entity.UpdateUserId = GetCurrentUserId();
            entity.UpdateDate = DateTime.UtcNow;
            contentCommentRepository.Update(entity);
            await hrUnitOfWork.SaveChangesAsync();
            return ProduceSuccessResponse(new CommonResponse { Result = true, Code = StatusCodes.Status200OK, Message = "İşlem Tamamlandı." });
        }
        public async Task<Response<CommonResponse>> DeleteContentCommentAsync(long id, CancellationToken cancellationToken = default)
        {
            if (ValidationHelper.RuleBasedValidator<UpdateContentCommentDto>(new UpdateContentCommentDto { Id = id }, typeof(IUpdateContentCommentBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<CommonResponse>();

            var entity = await contentCommentRepository.GetAsync(p => p.Id == id);
            entity.UpdateUserId = GetCurrentUserId();
            entity.UpdateDate = DateTime.UtcNow;
            entity.IsDelete = true;
            entity.IsActive = false;
            contentCommentRepository.Update(entity);
            await hrUnitOfWork.SaveChangesAsync();
            return ProduceSuccessResponse(new CommonResponse { Result = true, Code = StatusCodes.Status200OK, Message = "İşlem Tamamlandı." });
        }

        public async Task<Response<ContentCommentDto>> GetContentCommentByIdAsync(long id)
        {
            var contentComment = await contentCommentRepository.GetAsync(predicate: p => p.Id == id && p.IsDelete != true);

            var contentCommentDto = mapper.Map<ContentCommentDto>(contentComment);
            contentCommentDto.Editable = contentCommentDto.UserId == GetCurrentUserId();
            return ProduceSuccessResponse(contentCommentDto);
        }

        public async Task<Response<IEnumerable<ContentCommentDto>>> GetContentCommentListAsync()
        {
            var contentComment = await contentCommentRepository.GetListAsync(p => p.IsDelete != true, selector: s => mapper.Map<ContentCommentDto>(s));
            contentComment = contentComment.Select(s =>
            {
                s.Editable = s.UserId == GetCurrentUserId();
                return s;
            });
            return ProduceSuccessResponse(contentComment);
        }


    }
}
