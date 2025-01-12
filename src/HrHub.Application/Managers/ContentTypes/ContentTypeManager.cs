using AutoMapper;
using FluentValidation.Results;
using HrHub.Abstraction.Extensions;
using HrHub.Abstraction.Result;
using HrHub.Application.BusinessRules.ContentTypeBusinessRules;
using HrHub.Application.BusinessRules.TrainingCategoryBusinessRule;
using HrHub.Application.BusinessRules.WhatYouWillLearnBusinessRule;
using HrHub.Application.Factories;
using HrHub.Application.Managers.TypeManagers;
using HrHub.Core.Base;
using HrHub.Core.Data.Repository;
using HrHub.Core.Helpers;
using HrHub.Domain.Contracts.Dtos.ContentTypes;
using HrHub.Domain.Contracts.Dtos.TrainingCategoryDtos;
using HrHub.Domain.Contracts.Dtos.WhatYouWillLearns;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.Repositories.Concrete;
using HrHub.Infrastructre.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HrHub.Application.Managers.ContentTypes;
public class ContentTypeManager : ManagerBase, IContentTypeManager
{

    private readonly IHrUnitOfWork unitOfWork;
    private readonly Repository<ContentType> contentTypeRepository;
    private readonly IMapper mapper;
    public ContentTypeManager(IHttpContextAccessor httpContextAccessor,
                       IHrUnitOfWork unitOfWork,
                       IMapper mapper) : base(httpContextAccessor)
    {
        this.unitOfWork = unitOfWork;
        this.contentTypeRepository = unitOfWork.CreateRepository<ContentType>();
        this.mapper = mapper;

    }



    public async Task<Response<IEnumerable<ContentTypeDto>>> GetListForContentTypeAsync()
    {
        return ProduceSuccessResponse(await contentTypeRepository.GetListAsync(predicate: p => (p.IsDelete == false || p.IsDelete == null)
                                                                                               && p.IsActive,
                                                              selector: s => mapper.Map<ContentTypeDto>(s)));
    }

    public async Task<Response<ContentTypeDto>> GetByIdForContentTypeAsync(long id)
    {
        return ProduceSuccessResponse(await contentTypeRepository.GetAsync(predicate: p => p.Id == id
                                                                                           && (p.IsDelete == false || p.IsDelete == null)
                                                                                           && p.IsActive, 
                                                    selector: s => mapper.Map<ContentTypeDto>(s)));
    }
    public async Task<Response<CommonResponse>> AddContentTypeAsync(AddContentTypeDto data, CancellationToken cancellationToken = default)
    {
        if (ValidationHelper.RuleBasedValidator<AddContentTypeDto>(data, typeof(IAddContentTypeBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
            return cBasedValidResult.SendResponse<CommonResponse>();

        var entity = mapper.Map<ContentType>(data);
        await contentTypeRepository.AddAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ProduceSuccessResponse(new CommonResponse
        {
            Message = "Success",
            Code = StatusCodes.Status200OK,
            Result = true
        });
    }
    public async Task<Response<CommonResponse>> UpdateContentTypeAsync(UpdateContentTypeDto data, CancellationToken cancellationToken = default)
    {
        if (ValidationHelper.RuleBasedValidator<UpdateContentTypeDto>(data, typeof(IUpdateContentTypeBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
            return cBasedValidResult.SendResponse<CommonResponse>();

        var updData = await contentTypeRepository.GetAsync(predicate: p => p.Id == data.Id);
        mapper.Map(data, updData);

        contentTypeRepository.Update(updData);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ProduceSuccessResponse(new CommonResponse
        {
            Message = "Success",
            Code = StatusCodes.Status200OK,
            Result = true
        });
    }

    public async Task<Response<CommonResponse>> DeleteContentTypeAsync(long id, CancellationToken cancellationToken = default)
    {
        var entityDto = await contentTypeRepository.GetAsync(predicate: t => t.Id == id, selector: s => mapper.Map<DeleteContentTypeDto>(s));

        if (ValidationHelper.RuleBasedValidator<DeleteContentTypeDto>(entityDto, typeof(IExistContentTypeBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
            return cBasedValidResult.SendResponse<CommonResponse>();

        var entity = await contentTypeRepository.GetAsync(predicate: p => p.Id == id);
        entity.IsDelete = true;
        entity.DeleteDate = DateTime.UtcNow;
        //entity.DeleteUserId = this.GetCurrentUserId();

        contentTypeRepository.Update(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ProduceSuccessResponse(new CommonResponse
        {
            Message = "Success",
            Code = StatusCodes.Status200OK,
            Result = true
        });
    }
}
