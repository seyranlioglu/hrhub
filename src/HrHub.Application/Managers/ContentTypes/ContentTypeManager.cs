using AutoMapper;
using FluentValidation.Results;
using HrHub.Abstraction.Extensions;
using HrHub.Abstraction.Result;
using HrHub.Application.BusinessRules.ContentTypeBusinessRules;
using HrHub.Application.BusinessRules.TrainingCategoryBusinessRule;
using HrHub.Application.BusinessRules.WhatYouWillLearnBusinessRule;
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
    private readonly IMapper mapper;
    private readonly Repository<ContentType> contentTypeRepository;

    public ContentTypeManager(IHttpContextAccessor httpContextAccessor,
                              IHrUnitOfWork unitOfWork,
                              IMapper mapper) : base(httpContextAccessor)
    {
        unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        contentTypeRepository = unitOfWork.CreateRepository<ContentType>();
    }


    public async Task<Response<IEnumerable<ContentTypeDto>>> GetListForContentTypeAsync()
    {
        return ProduceSuccessResponse(await contentTypeRepository.GetListAsync(predicate: p => p.IsDelete == false,
                                                              selector: s => mapper.Map<ContentTypeDto>(s)));
    }

    public async Task<Response<ContentTypeDto>> GetByIdForContentTypeAsync(long id)
    {
        return ProduceSuccessResponse(await contentTypeRepository.GetAsync(predicate: p => p.Id == id,
                                                    selector: s => mapper.Map<ContentTypeDto>(s)));
    }
    public async Task<Response<CommonResponse>> AddContentTypeAsync(AddContentTypeDto data, CancellationToken cancellationToken = default)
    {
        if (ValidationHelper.RuleBasedValidator<AddContentTypeDto>(data, typeof(AddContentTypeBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
            return cBasedValidResult.SendResponse<CommonResponse>();

        var trainingCategory = mapper.Map<ContentType>(data);
        await contentTypeRepository.AddAsync(trainingCategory, cancellationToken);
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
        if (ValidationHelper.RuleBasedValidator<UpdateContentTypeDto>(data, typeof(UpdateContentTypeBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
            return cBasedValidResult.SendResponse<CommonResponse>();

        var updData = await contentTypeRepository.GetAsync(predicate: p => p.Id == data.Id);
        mapper.Map(updData, data);

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

        if (ValidationHelper.RuleBasedValidator<DeleteContentTypeDto>(entityDto, typeof(ExistContentTypeBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
            return cBasedValidResult.SendResponse<CommonResponse>();

        var entity = await contentTypeRepository.GetAsync(predicate: p => p.Id == id);
        entity.IsDelete = true;

        var deletedData = mapper.Map(entityDto, entity);

        contentTypeRepository.Update(deletedData);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ProduceSuccessResponse(new CommonResponse
        {
            Message = "Success",
            Code = StatusCodes.Status200OK,
            Result = true
        });
    }
}
