using AutoMapper;
using FluentValidation.Results;
using HrHub.Abstraction.Contracts.Dtos.TrainingDtos;
using HrHub.Abstraction.Extensions;
using HrHub.Abstraction.Result;
using HrHub.Application.BusinessRules.TrainingBusinessRules;
using HrHub.Application.BusinessRules.WhatYouWillLearnBusinessRule;
using HrHub.Core.Base;
using HrHub.Core.Data.Repository;
using HrHub.Core.Helpers;
using HrHub.Domain.Contracts.Dtos.WhatYouWillLearns;
using HrHub.Domain.Contracts.Dtos.WhatYouWillLearnsDtos;
using HrHub.Domain.Contracts.Responses.CommonResponse;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.Repositories.Abstract;
using HrHub.Infrastructre.Repositories.Concrete;
using HrHub.Infrastructre.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HrHub.Application.Managers.WhatYouWillLearnManagers
{
    public class WhatYouWillLearnManager : ManagerBase, IWhatYouWillLearnManager
    {
        private readonly IHrUnitOfWork hrUnitOfWork;
        private readonly IMapper mapper;
        private readonly Repository<WhatYouWillLearn> whatYouWillLearnRepository;

        public WhatYouWillLearnManager(IHttpContextAccessor httpContextAccessor,
                                       IHrUnitOfWork hrUnitOfWork,
                                       IMapper mapper) : base(httpContextAccessor)
        {
            this.hrUnitOfWork = hrUnitOfWork;
            this.whatYouWillLearnRepository = hrUnitOfWork.CreateRepository<WhatYouWillLearn>();
            this.mapper = mapper;
        }
        public async Task<Response<ReturnIdResponse>> AddWhatYouWillLearnAsync(AddWhatYouWillLearnDto data, CancellationToken cancellationToken = default)
        {
            if (ValidationHelper.RuleBasedValidator<AddWhatYouWillLearnDto>(data, typeof(IAddWhatYouWillLearnBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<ReturnIdResponse>();

            var mapperData = mapper.Map<WhatYouWillLearn>(data);
            var result = await whatYouWillLearnRepository.AddAndReturnAsync(mapperData);
            await hrUnitOfWork.SaveChangesAsync();
            return ProduceSuccessResponse(new ReturnIdResponse
            {
                Id = result.Id,
            });
        }
        public async Task<Response<CommonResponse>> UpdateWhatYouWillLearnAsync(UpdateWhatYouWillLearnDto dto, CancellationToken cancellationToken = default)
        {
            var entity = await whatYouWillLearnRepository.GetAsync(predicate: t => t.Id == dto.Id);

            if (ValidationHelper.RuleBasedValidator<UpdateWhatYouWillLearnDto>(dto, typeof(IUpdateWhatYouWillLearnBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<CommonResponse>();

            mapper.Map(dto, entity);

            whatYouWillLearnRepository.Update(entity);
            await hrUnitOfWork.SaveChangesAsync(cancellationToken);

            return ProduceSuccessResponse(new CommonResponse
            {
                Message = "Success",
                Code = StatusCodes.Status200OK,
                Result = true
            });
        }
        public async Task<Response<CommonResponse>> DeleteWhatYouWillLearnAsync(long id, CancellationToken cancellationToken = default)
        {
            var entityDto = await whatYouWillLearnRepository.GetAsync(predicate: t => t.Id == id, selector: s => mapper.Map<DeleteWhatYouWillLearnDto>(s));

            if (ValidationHelper.RuleBasedValidator<DeleteWhatYouWillLearnDto>(entityDto, typeof(IExistWhatYouWillLearnBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<CommonResponse>();

            var entity = await whatYouWillLearnRepository.GetAsync(predicate: p => p.Id == id);
            entity.IsDelete = true;
            entity.DeleteDate = DateTime.UtcNow;
            //entity.DeleteUserId = this.GetCurrentUserId();

            whatYouWillLearnRepository.Update(entity);
            await hrUnitOfWork.SaveChangesAsync(cancellationToken);
            return ProduceSuccessResponse(new CommonResponse
            {
                Message = "Success",
                Code = StatusCodes.Status200OK,
                Result = true
            });
        }

        public async Task<Response<IEnumerable<GetWhatYouWillLearnDto>>> GetWhatYouWillLearnListAsync()
        {
            var entityDto = await whatYouWillLearnRepository.GetListAsync(predicate: p => p.IsDelete != true,
                                                                        include: i => i.Include(s => s.Training),
                                                                        selector: s => mapper.Map<GetWhatYouWillLearnDto>(s)

                                                                        );
            return ProduceSuccessResponse(entityDto);
        }
        public async Task<Response<GetWhatYouWillLearnDto>> GetWhatYouWillLearnByIdAsync(long id)
        {
            var entityDto = await whatYouWillLearnRepository.GetAsync(predicate: p => p.IsDelete != true && p.Id == id,
                                                                              include: i => i.Include(s => s.Training),
                                                                              selector: s => mapper.Map<GetWhatYouWillLearnDto>(s)
                                                                        );
            return ProduceSuccessResponse(entityDto);
        }
    }
}
