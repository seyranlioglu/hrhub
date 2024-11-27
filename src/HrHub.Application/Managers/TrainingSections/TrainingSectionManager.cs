using AutoMapper;
using FluentValidation.Results;
using HrHub.Abstraction.Extensions;
using HrHub.Abstraction.Result;
using HrHub.Application.BusinessRules.TrainingSectionBusinessRules;
using HrHub.Core.Base;
using HrHub.Core.Data.Repository;
using HrHub.Core.Helpers;
using HrHub.Domain.Contracts.Dtos.TrainingSectionDtos;
using HrHub.Domain.Contracts.Responses.CommonResponse;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HrHub.Application.Managers.TrainingSections
{
    public class TrainingSectionManager : ManagerBase, ITrainingSectionManager
    {
        private readonly IHrUnitOfWork hrUnitOfWork;
        private readonly IMapper mapper;
        private readonly Repository<TrainingSection> trainingSectionRepository;

        public TrainingSectionManager(IHttpContextAccessor httpContextAccessor,
                                      IHrUnitOfWork hrUnitOfWork,
                                      IMapper mapper) : base(httpContextAccessor)
        {
            this.hrUnitOfWork = hrUnitOfWork;
            this.trainingSectionRepository = hrUnitOfWork.CreateRepository<TrainingSection>();
            this.mapper = mapper;
        }

        public async Task<Response<ReturnIdResponse>> AddTrainingSectionAsync(AddTrainingSectionDto data, CancellationToken cancellationToken = default)
        {
            if (ValidationHelper.RuleBasedValidator<AddTrainingSectionDto>(data, typeof(AddTrainingSectionBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<ReturnIdResponse>();

            var trainingSectionEntity = mapper.Map<TrainingSection>(data);
            var result = await trainingSectionRepository.AddAndReturnAsync(trainingSectionEntity);
            await hrUnitOfWork.SaveChangesAsync();

            return ProduceSuccessResponse(new ReturnIdResponse
            {
                Id = result.Id
            });
        }
        public async Task<Response<CommonResponse>> UpdateTrainingSectionAsync(UpdateTrainingSectionDto dto, CancellationToken cancellationToken = default)
        {
            if (ValidationHelper.RuleBasedValidator<UpdateTrainingSectionDto>(dto, typeof(IUpdateTrainingSectionBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<CommonResponse>();

            var trainingSectionEntity = await trainingSectionRepository.GetAsync(predicate: p => p.Id == dto.Id);
            var mapperData = mapper.Map(dto, trainingSectionEntity);
            trainingSectionRepository.Update(mapperData);
            await hrUnitOfWork.SaveChangesAsync(cancellationToken);

            return ProduceSuccessResponse(new CommonResponse
            {
                Message = "Success",
                Code = StatusCodes.Status200OK,
                Result = true
            });
        }
        public async Task<Response<CommonResponse>> DeleteTrainingSectionAsync(long id, CancellationToken cancellationToken = default)
        {
            var trainingDto = await trainingSectionRepository.GetAsync(predicate: t => t.Id == id, selector: s => mapper.Map<DeleteTrainingSectionDto>(s));
            if (ValidationHelper.RuleBasedValidator<DeleteTrainingSectionDto>(trainingDto, typeof(ExistTrainingSectionBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<CommonResponse>();

            var trainingSectionEntity = await trainingSectionRepository.GetAsync(predicate: p => p.Id == id);
            trainingSectionEntity.IsDelete = true;
            var deletedData = mapper.Map(trainingDto, trainingSectionEntity);

            trainingSectionRepository.Update(deletedData);
            await hrUnitOfWork.SaveChangesAsync(cancellationToken);
            return ProduceSuccessResponse(new CommonResponse
            {
                Message = "Success",
                Code = StatusCodes.Status200OK,
                Result = true
            });
        }
        public async Task<Response<IEnumerable<GetTrainingSectionDto>>> GetTrainingSectionListAsync()
        {
            var trainingSectionListDto = await trainingSectionRepository.GetListAsync(predicate: p => p.IsActive,
                                                                        include: i => i.Include(s => s.Training),
                                                                        selector: s => mapper.Map<GetTrainingSectionDto>(s));

            if (ValidationHelper.RuleBasedValidator<GetTrainingSectionDto>(trainingSectionListDto.FirstOrDefault(), typeof(ExistTrainingSectionBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<IEnumerable<GetTrainingSectionDto>>();

            return ProduceSuccessResponse(trainingSectionListDto);
        }
        public async Task<Response<GetTrainingSectionDto>> GetTrainingSectionByIdAsync(long id)
        {
            var trainingDto = await trainingSectionRepository.GetAsync(predicate: p => p.IsActive && p.Id == id,
                                                                       include: i => i.Include(s => s.Training),
                                                                       selector: s => mapper.Map<GetTrainingSectionDto>(s));

            if (ValidationHelper.RuleBasedValidator<GetTrainingSectionDto>(trainingDto, typeof(ExistTrainingSectionBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<GetTrainingSectionDto>();

            return ProduceSuccessResponse(trainingDto);
        }
    }
}
