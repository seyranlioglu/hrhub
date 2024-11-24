using AutoMapper;
using HrHub.Abstraction.Contracts.Dtos.TrainingDtos;
using HrHub.Abstraction.Contracts.Responses.Trainings;
using HrHub.Abstraction.Result;
using HrHub.Application.Managers.Users;
using HrHub.Core.Base;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;
using Microsoft.AspNetCore.Http;

namespace HrHub.Application.Managers.Trainings;

public class TrainingManager : ManagerBase, ITrainingManager
{
    private readonly IHrUnitOfWork hrUnitOfWork;
    private readonly IMapper mapper;
    private readonly Repository<Training> trainingRepository;
    private readonly IUserManager userManager;

    public TrainingManager(IHttpContextAccessor httpContextAccessor,
                           IHrUnitOfWork hrUnitOfWork,
                           IMapper mapper,
                           IUserManager userManager) : base(httpContextAccessor)
    {
        this.hrUnitOfWork = hrUnitOfWork;
        trainingRepository = hrUnitOfWork.CreateRepository<Training>();

        this.mapper = mapper;
        this.userManager = userManager;
    }

    public async Task<Response<IEnumerable<GetTrainingDto>>> GetListForTrainingAsync()
    {
        var data = await trainingRepository.GetListAsync(predicate: p => p.IsActive && p.IsDelete == false,
                                                         selector: s => mapper.Map<GetTrainingDto>(s));

        if (!data.Any())
            return ProduceFailResponse<IEnumerable<GetTrainingDto>>("ContentTypes not found!", StatusCodes.Status404NotFound);
        else
            return ProduceSuccessResponse(data);
    }

    public async Task<Response<GetTrainingDto>> GetForTrainingAsync()
    {
        var data = await trainingRepository.GetAsync(predicate: p => p.IsActive && p.IsDelete == false,
                                                         selector: s => mapper.Map<GetTrainingDto>(s));

        if (data is null)
            return ProduceFailResponse<GetTrainingDto>("ContentTypes not found!", StatusCodes.Status404NotFound);
        else
            return ProduceSuccessResponse(data);
    }

    //public async Task<Response<AddTrainingResponse>> AddForTrainingAsync(AddTrainingDto addTrainingDto)
    //{
    //    bool isMainUser = await userManager.IsMainUser();
    //}
}
