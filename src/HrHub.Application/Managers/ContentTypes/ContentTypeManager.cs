using AutoMapper;
using HrHub.Abstraction.Contracts.Dtos.ContentTypes;
using HrHub.Abstraction.Result;
using HrHub.Core.Base;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;
using Microsoft.AspNetCore.Http;

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
        IEnumerable<ContentTypeDto> data = await contentTypeRepository.GetListAsync(predicate: p => p.IsDelete == false,
                                                              selector: s => mapper.Map<ContentTypeDto>(s));

        if (!data.Any())
            return ProduceFailResponse<IEnumerable<ContentTypeDto>>("ContentTypes not found!", StatusCodes.Status404NotFound);
        else
            return ProduceSuccessResponse(data);
    }

    public async Task<Response<ContentTypeDto>> GetByIdForContentTypeAsync(long id)
    {
        var data = await contentTypeRepository.GetAsync(predicate: p => p.Id == id,
                                                        selector: s => mapper.Map<ContentTypeDto>(s));

        if (data is null)
            return ProduceFailResponse<ContentTypeDto>("ContentTypes not found!", StatusCodes.Status404NotFound);
        else
            return ProduceSuccessResponse(data);
    }
}
