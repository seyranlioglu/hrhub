using AutoMapper;
using FluentValidation.Results;
using HrHub.Abstraction.Extensions;
using HrHub.Abstraction.Result;
using HrHub.Application.BusinessRules.CommentVoteBusinessRules;
using HrHub.Core.Base;
using HrHub.Core.Data.Repository;
using HrHub.Core.Helpers;
using HrHub.Core.HrFluentValidation;
using HrHub.Domain.Contracts.Dtos.CommentVoteDtos;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;
using Microsoft.AspNetCore.Http;

namespace HrHub.Application.Managers.CommentManagers
{
    internal class CommentVoteManager : ManagerBase, ICommentVoteManager
    {
        private readonly IHrUnitOfWork hrUnitOfWork;
        private readonly IMapper mapper;
        private readonly Repository<CommentVote> commentVoteRepository;
        public CommentVoteManager(IHttpContextAccessor httpContextAccessor, IHrUnitOfWork hrUnitOfWork, IMapper mapper) : base(httpContextAccessor)
        {
            this.hrUnitOfWork = hrUnitOfWork;
            this.mapper = mapper;
            this.commentVoteRepository = hrUnitOfWork.CreateRepository<CommentVote>();
        }

        public async Task<Response<CommonResponse>> AddCommentVoteAsync(AddCommentVoteDto data, CancellationToken cancellationToken = default)
        {
            if (ValidationHelper.RuleBasedValidator<AddCommentVoteDto>(data, typeof(IAddCommentVoteBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<CommonResponse>();

            var validator = new FieldBasedValidator<AddCommentVoteDto>();
            var validateResult = validator.Validate(data);

            if (!validateResult.IsValid)
                return validateResult.SendResponse<CommonResponse>();

            var entity = mapper.Map<CommentVote>(data);
            entity.CreateUserId = GetCurrentUserId();
            entity.CreatedDate = DateTime.UtcNow;
            entity.IsDelete = false;
            entity.IsActive = true;
            await commentVoteRepository.AddAsync(entity);
            await hrUnitOfWork.SaveChangesAsync();
            return ProduceSuccessResponse(new CommonResponse { Result = true, Code = StatusCodes.Status200OK, Message = "İşlem Tamamlandı." });
        }
        public async Task<Response<CommonResponse>> UpdateCommentVoteAsync(UpdateCommentVoteDto dto, CancellationToken cancellationToken = default)
        {
            if (ValidationHelper.RuleBasedValidator<UpdateCommentVoteDto>(dto, typeof(IUpdateCommentVoteBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<CommonResponse>();

            var validator = new FieldBasedValidator<UpdateCommentVoteDto>();
            var validateResult = validator.Validate(dto);

            if (!validateResult.IsValid)
                return validateResult.SendResponse<CommonResponse>();


            var data = await commentVoteRepository.GetAsync(p => p.Id == dto.Id);
            var entity = mapper.Map(dto, data);
            entity.UpdateUserId = GetCurrentUserId();
            entity.UpdateDate = DateTime.UtcNow;
            commentVoteRepository.Update(entity);
            await hrUnitOfWork.SaveChangesAsync();
            return ProduceSuccessResponse(new CommonResponse { Result = true, Code = StatusCodes.Status200OK, Message = "İşlem Tamamlandı." });
        }
        public async Task<Response<CommonResponse>> DeleteCommentVoteAsync(long id, CancellationToken cancellationToken = default)
        {
            if (ValidationHelper.RuleBasedValidator<UpdateCommentVoteDto>(new UpdateCommentVoteDto { Id = id }, typeof(IUpdateCommentVoteBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<CommonResponse>();

            var entity = await commentVoteRepository.GetAsync(p => p.Id == id);
            entity.UpdateUserId = GetCurrentUserId();
            entity.UpdateDate = DateTime.UtcNow;
            entity.IsDelete = true;
            entity.IsActive = false;
            commentVoteRepository.Update(entity);
            await hrUnitOfWork.SaveChangesAsync();
            return ProduceSuccessResponse(new CommonResponse { Result = true, Code = StatusCodes.Status200OK, Message = "İşlem Tamamlandı." });
        }

        public async Task<Response<CommentVoteDto>> GetCommentVoteByIdAsync(long id)
        {
            var CommentVote = await commentVoteRepository.GetAsync(predicate: p => p.Id == id && p.IsDelete != true);

            var CommentVoteDto = mapper.Map<CommentVoteDto>(CommentVote);
            CommentVoteDto.Editable = CommentVoteDto.UserId == GetCurrentUserId();
            return ProduceSuccessResponse(CommentVoteDto);
        }

        public async Task<Response<IEnumerable<CommentVoteDto>>> GetCommentVoteListAsync()
        {
            var CommentVote = await commentVoteRepository.GetListAsync(p => p.IsDelete != true, selector: s => mapper.Map<CommentVoteDto>(s));
            CommentVote = CommentVote.Select(s =>
            {
                s.Editable = s.UserId == GetCurrentUserId();
                return s;
            });
            return ProduceSuccessResponse(CommentVote);
        }


    }
}
