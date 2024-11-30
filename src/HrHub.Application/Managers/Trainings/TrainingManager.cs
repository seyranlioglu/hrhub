using AutoMapper;
using FluentValidation.Results;
using HrHub.Abstraction.Consts;
using HrHub.Abstraction.Contracts.Dtos.TrainingDtos;
using HrHub.Abstraction.Extensions;
using HrHub.Abstraction.Result;
using HrHub.Application.BusinessRules.TrainingBusinessRules;
using HrHub.Core.Base;
using HrHub.Core.Data.Repository;
using HrHub.Core.Helpers;
using HrHub.Domain.Contracts.Dtos.TrainingDtos;
using HrHub.Domain.Contracts.Responses.CommonResponse;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HrHub.Application.Managers.Trainings;

public class TrainingManager : ManagerBase, ITrainingManager
{
    private readonly IHrUnitOfWork hrUnitOfWork;
    private readonly IMapper mapper;
    private readonly Repository<Training> trainingRepository;
    private readonly Repository<TrainingStatus> trainingStatuRepository;

    public TrainingManager(IHttpContextAccessor httpContextAccessor,
                           IHrUnitOfWork hrUnitOfWork,
                           IMapper mapper) : base(httpContextAccessor)
    {
        this.hrUnitOfWork = hrUnitOfWork;
        trainingRepository = hrUnitOfWork.CreateRepository<Training>();
        trainingStatuRepository = hrUnitOfWork.CreateRepository<TrainingStatus>();
        this.mapper = mapper;
    }

    public async Task<Response<ReturnIdResponse>> AddTrainingAsync(AddTrainingDto data, CancellationToken cancellationToken = default)
    {
        if (ValidationHelper.RuleBasedValidator<AddTrainingDto>(data, typeof(AddTrainingBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
            return cBasedValidResult.SendResponse<ReturnIdResponse>();


        var trainingEntity = mapper.Map<Training>(data);
        trainingEntity.CurrentAmount = data.Amount - (data.Amount * data.DiscountRate / 100); // Bunu konuşuruz!!! 
        //CompletionTime hesaplanacak, elle girilmeyecek. Konuşacağız
        trainingEntity.TrainingStatusId = await trainingStatuRepository.GetAsync(predicate: p => p.Code == TrainingStatuConst.Preparing,
                                                                                 selector: s => s.Id);
        var result = await trainingRepository.AddAndReturnAsync(trainingEntity);
        await hrUnitOfWork.SaveChangesAsync();

        return ProduceSuccessResponse(new ReturnIdResponse
        {
            Id = result.Id
        });
    }

    public async Task<Response<CommonResponse>> UpdateTrainingAsync(UpdateTrainingDto dto, CancellationToken cancellationToken = default)
    {
        var training = await trainingRepository.GetAsync(predicate: t => t.Id == dto.Id);

        if (ValidationHelper.RuleBasedValidator<UpdateTrainingDto>(dto, typeof(UpdateTrainingBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
            return cBasedValidResult.SendResponse<CommonResponse>();

       var mapperData = mapper.Map(dto, training);
        training.CurrentAmount = dto.Amount - (dto.Amount * dto.DiscountRate / 100);

        trainingRepository.Update(mapperData);
        await hrUnitOfWork.SaveChangesAsync(cancellationToken);

        return ProduceSuccessResponse(new CommonResponse
        {
            Message = "Success",
            Code = StatusCodes.Status200OK,
            Result = true
        });
    }
    public async Task<Response<CommonResponse>> DeleteTrainingAsync(long id, CancellationToken cancellationToken = default)
    {
        var trainingDto = await trainingRepository.GetAsync(predicate: t => t.Id == id, selector : s => mapper.Map<DeleteTrainingDto>(s));
        if (ValidationHelper.RuleBasedValidator<DeleteTrainingDto>(trainingDto, typeof(ExistTrainingBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
            return cBasedValidResult.SendResponse<CommonResponse>();

        var trainingEntity = await trainingRepository.GetAsync(predicate: p => p.Id == id);
        trainingEntity.IsDelete = true;

        var deletedData = mapper.Map(trainingDto, trainingEntity);

        trainingRepository.Update(deletedData);
        await hrUnitOfWork.SaveChangesAsync(cancellationToken);
        return ProduceSuccessResponse(new CommonResponse
        {
            Message = "Success",
            Code = StatusCodes.Status200OK,
            Result = true
        });
    }

    public async Task<Response<IEnumerable<GetTrainingDto>>> GetTrainingListAsync()
    {
        var trainingListDto = await trainingRepository.GetListAsync(predicate: p => p.IsActive,
                                                                    include: i => i.Include(s => s.TrainingCategories)
                                                                    .Include(s => s.Instructors)
                                                                    .Include(s => s.TimeUnits)
                                                                    .Include(s => s.TrainingLevels),
                                                                    selector: s => mapper.Map<GetTrainingDto>(s));

        if (ValidationHelper.RuleBasedValidator<GetTrainingDto>(trainingListDto.FirstOrDefault(), typeof(ExistTrainingBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
            return cBasedValidResult.SendResponse<IEnumerable<GetTrainingDto>>();

        return ProduceSuccessResponse(trainingListDto);

    }


    public async Task<Response<GetTrainingDto>> GetTrainingByIdAsync(long id)
    {
        var trainingDto = await trainingRepository.GetAsync(predicate: p => p.IsActive && p.Id == id,
                                                                include: i => i.Include(s => s.TrainingCategories)
                                                                    .Include(s => s.Instructors)
                                                                    .Include(s => s.TimeUnits)
                                                                    .Include(s => s.TrainingLevels),
                                                                    selector: s => mapper.Map<GetTrainingDto>(s));

        if (ValidationHelper.RuleBasedValidator<GetTrainingDto>(trainingDto, typeof(ExistTrainingBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
            return cBasedValidResult.SendResponse<GetTrainingDto>();

        return ProduceSuccessResponse(trainingDto);
    }
}
