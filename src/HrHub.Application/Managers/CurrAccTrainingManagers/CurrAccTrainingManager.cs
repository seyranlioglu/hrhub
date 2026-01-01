using AutoMapper;
using FluentValidation.Results;
using HrHub.Abstraction.Extensions;
using HrHub.Abstraction.Result;
using HrHub.Application.BusinessRules.CurrAccTrainingBusinesRules;
using HrHub.Core.Base;
using HrHub.Core.Data.Repository;
using HrHub.Core.Helpers;
using HrHub.Domain.Contracts.Dtos.CurrAccTrainingDtos;
using HrHub.Domain.Contracts.Responses.CommonResponse;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HrHub.Application.Managers.CurrAccTrainingManagers
{
    public class CurrAccTrainingManager : ManagerBase, ICurrAccTrainingManager
    {
        private readonly IHrUnitOfWork unitOfWork;
        private readonly Repository<CurrAccTraining> currAccTrainingRepository;
        private readonly Repository<CurrAccTrainingStatus> currAccTrainingStatusRepository;
        private readonly IMapper mapper;

        public CurrAccTrainingManager(IHttpContextAccessor httpContextAccessor,
                                      IHrUnitOfWork unitOfWork,
                                      IMapper mapper) : base(httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.currAccTrainingRepository = unitOfWork.CreateRepository<CurrAccTraining>();
            this.currAccTrainingStatusRepository = unitOfWork.CreateRepository<CurrAccTrainingStatus>();
            this.mapper = mapper;
        }

        public async Task<Response<ReturnIdResponse>> AddCurrAccTrainingAsync(AddCurrAccTrainingDto data, CancellationToken cancellationToken = default)
        {
            if (ValidationHelper.RuleBasedValidator<AddCurrAccTrainingDto>(data, typeof(IAddCurrAccTrainingBusinessRule))
                           is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<ReturnIdResponse>();

            var currAccTrainingStatusId = await currAccTrainingStatusRepository.GetAsync(predicate: p => p.Code == data.CurrAccTrainingStatusCode, selector: s => s.Id);
            var newTraining = mapper.Map<CurrAccTraining>(data);
            newTraining.CurrAccTrainingStatusId = currAccTrainingStatusId;
            newTraining.CreatedDate = DateTime.UtcNow;
            newTraining.CreateUserId = this.GetCurrentUserId();
            var result = await currAccTrainingRepository.AddAndReturnAsync(newTraining, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);
            return ProduceSuccessResponse(new ReturnIdResponse { Id = result.Id });

        }

        public async Task<Response<CommonResponse>> UpdateCurrAccTrainingAsync(UpdateCurrAccTrainingDto data, CancellationToken cancellationToken = default)
        {
            if (ValidationHelper.RuleBasedValidator<UpdateCurrAccTrainingDto>(data, typeof(IUpdateCurrAccTrainingBusinessRule))
                    is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<CommonResponse>();

            var existingTraining = await currAccTrainingRepository.GetAsync(predicate: p => p.Id == data.Id);
            if (existingTraining == null)
                return ProduceFailResponse<CommonResponse>("Güncellenecek eğitim bulunamadı.", StatusCodes.Status404NotFound);

            var mapperData = mapper.Map(data, existingTraining);
            mapperData.UpdateDate = DateTime.UtcNow;
            mapperData.UpdateUserId = this.GetCurrentUserId();

            currAccTrainingRepository.Update(mapperData);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return ProduceSuccessResponse(new CommonResponse { Message = "Güncelleme başarılı.", Code = StatusCodes.Status200OK, Result = true });

        }

        public async Task<Response<CommonResponse>> DeleteCurrAccTrainingAsync(DeleteCurrAccTrainingDto deleteCurrAccTrainingDto, CancellationToken cancellationToken = default)
        {
            if (ValidationHelper.RuleBasedValidator<DeleteCurrAccTrainingDto>(deleteCurrAccTrainingDto, typeof(IDeleteCurrAccTrainingBusinessRule))
                is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<CommonResponse>();

            var training = await currAccTrainingRepository.GetAsync(predicate: p => p.Id == deleteCurrAccTrainingDto.Id);
            training.IsDelete = true;
            training.DeleteDate = DateTime.UtcNow;
            training.DeleteUserId = this.GetCurrentUserId();
            currAccTrainingRepository.Update(training);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return ProduceSuccessResponse(new CommonResponse { Message = "Silme işlemi başarılı.", Code = StatusCodes.Status200OK, Result = true });

        }


        public async Task<Response<IEnumerable<GetCurrAccTrainingDto>>> GetAllCurrAccTrainingsAsync()
        {
            var trainings = await currAccTrainingRepository.GetListAsync(predicate: p => p.IsActive && p.IsDelete == false,
                                                                         include: i => i.Include(s => s.CurrAcc)
                                                                                        .Include(i => i.Training)
                                                                                        .Include(w => w.CurrAccTrainingStatus)
                                                                                        .Include(d => d.ConfirmUser),
                                                                         selector: s => mapper.Map<GetCurrAccTrainingDto>(s));
            return ProduceSuccessResponse(trainings);
        }

        public async Task<Response<GetCurrAccTrainingDto>> GetCurrAccTrainingByIdAsync(long id)
        {
            var trainings = await currAccTrainingRepository.GetAsync(predicate: p => p.IsActive && p.IsDelete == false && p.Id == id,
                                                                         include: i => i.Include(s => s.CurrAcc).ThenInclude(s => s.CurrAccType)
                                                                                        .Include(i => i.Training)
                                                                                        .Include(w => w.CurrAccTrainingStatus)
                                                                                        .Include(d => d.ConfirmUser),
                                                                         selector: s => mapper.Map<GetCurrAccTrainingDto>(s));
            return ProduceSuccessResponse(trainings);
        }
    }
}
