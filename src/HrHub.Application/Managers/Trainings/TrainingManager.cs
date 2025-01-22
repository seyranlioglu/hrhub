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
        if (ValidationHelper.RuleBasedValidator<AddTrainingDto>(data, typeof(IAddTrainingBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
            return cBasedValidResult.SendResponse<ReturnIdResponse>();


        var trainingEntity = mapper.Map<Training>(data);
        trainingEntity.IsActive = true;
        trainingEntity.ForWhomId = data.ForWhomId == 0 ? (long?)null : data.ForWhomId;
        trainingEntity.InstructorId = data.InstructorId == 0 ? (long?)null : data.InstructorId;
        trainingEntity.CompletionTimeUnitId = data.CompletionTimeUnitId == 0 ? (long?)null : data.CompletionTimeUnitId;
        trainingEntity.TrainingLevelId = data.TrainingLevelId == 0 ? (long?)null : data.TrainingLevelId;
        trainingEntity.PreconditionId = data.PreconditionId == 0 ? (long?)null : data.PreconditionId;
        trainingEntity.ForWhomId = data.ForWhomId == 0 ? (long?)null : data.ForWhomId;
        trainingEntity.EducationLevelId = data.EducationLevelId == 0 ? (long?)null : data.EducationLevelId;
        trainingEntity.PriceTierId = data.PriceTierId == 0 ? (long?)null : data.PriceTierId;
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

        if (ValidationHelper.RuleBasedValidator<UpdateTrainingDto>(dto, typeof(IUpdateTrainingBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
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
        var trainingDto = await trainingRepository.GetAsync(predicate: t => t.Id == id, selector: s => mapper.Map<DeleteTrainingDto>(s));
        if (ValidationHelper.RuleBasedValidator<DeleteTrainingDto>(trainingDto, typeof(IExistTrainingBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
            return cBasedValidResult.SendResponse<CommonResponse>();

        var trainingEntity = await trainingRepository.GetAsync(predicate: p => p.Id == id);
        trainingEntity.IsDelete = true;
        trainingEntity.DeleteDate = DateTime.UtcNow;
        //trainingEntity.DeleteUserId = this.GetCurrentUserId();

        trainingRepository.Update(trainingEntity);
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
        var trainingListDto = await trainingRepository.GetListAsync(predicate: p => p.IsDelete != true,
                                                                    include: i => i.Include(s => s.TrainingCategory)
                                                                    .Include(s => s.Instructor)
                                                                    .Include(s => s.TimeUnit)
                                                                    .Include(s => s.TrainingLevel)
                                                                    .Include(s => s.TrainingStatus)
                                                                    .Include(s => s.EducationLevel)
                                                                    .Include(s => s.ForWhom)
                                                                    .Include(s => s.Precondition)
                                                                    .Include(s => s.PriceTier)
                                                                    .Include(s => s.TrainingSections).ThenInclude(d => d.TrainingContents).ThenInclude(e => e.ContentType)
                                                                    .Include(s => s.TrainingType),
                                                                    selector: s => mapper.Map<GetTrainingDto>(s));
        return ProduceSuccessResponse(trainingListDto);

    }


    public async Task<Response<GetTrainingDto>> GetTrainingByIdAsync(long id)
    {
        var trainingDto = await trainingRepository.GetAsync(predicate: p => p.Id == id && p.IsDelete != true,
                                                            include: i => i.Include(s => s.TrainingCategory)
                                                                    .Include(s => s.Instructor)
                                                                    .Include(s => s.TimeUnit)
                                                                    .Include(s => s.TrainingLevel)
                                                                    .Include(s => s.TrainingStatus)
                                                                    .Include(s => s.EducationLevel)
                                                                    .Include(s => s.ForWhom)
                                                                    .Include(s => s.Precondition)
                                                                    .Include(s => s.PriceTier)
                                                                    .Include(s => s.TrainingSections).ThenInclude(d => d.TrainingContents).ThenInclude(e=>e.ContentType)
                                                                    .Include(s => s.TrainingType),
                                                                    selector: s => mapper.Map<GetTrainingDto>(s));
        return ProduceSuccessResponse(trainingDto);
    }
}
