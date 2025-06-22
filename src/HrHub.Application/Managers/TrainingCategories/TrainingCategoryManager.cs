using AutoMapper;
using HrHub.Abstraction.Extensions;
using HrHub.Abstraction.Result;
using HrHub.Application.BusinessRules.TrainingCategoryBusinessRule;
using HrHub.Core.Data.Repository;
using HrHub.Core.Helpers;
using HrHub.Domain.Contracts.Dtos.TrainingCategoryDtos;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using HrHub.Core.Base;
using HrHub.Application.BusinessRules.WhatYouWillLearnBusinessRule;
using HrHub.Domain.Contracts.Dtos.WhatYouWillLearnsDtos;
using HrHub.Infrastructre.Repositories.Concrete;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using HrHub.Infrastructre.Repositories.Abstract;

namespace HrHub.Application.Managers.TrainingCategories
{
    public class TrainingCategoryManager : ManagerBase, ITrainingCategoryManager
    {

        private readonly IHrUnitOfWork unitOfWork;
        private readonly Repository<TrainingCategory> categoryRepository;
        private readonly IMapper mapper;

        public TrainingCategoryManager(IHttpContextAccessor httpContextAccessor, IHrUnitOfWork unitOfWork, IMapper mapper) : base(httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.categoryRepository = unitOfWork.CreateRepository<TrainingCategory>();
            this.mapper = mapper;
        }

        public async Task<Response<CommonResponse>> AddTrainingCategoryAsync(AddTrainingCategoryDto data, CancellationToken cancellationToken = default)
        {
            if (ValidationHelper.RuleBasedValidator<AddTrainingCategoryDto>(data, typeof(IAddTrainingCategoryBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<CommonResponse>();

            var trainingCategory = mapper.Map<TrainingCategory>(data);
            await categoryRepository.AddAsync(trainingCategory, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return ProduceSuccessResponse(new CommonResponse
            {
                Message = "Success",
                Code = StatusCodes.Status200OK,
                Result = true
            });

        }

        public async Task<Response<CommonResponse>> UpdateTrainingCategoryAsync(UpdateTrainingCategoryDto data, CancellationToken cancellationToken = default)
        {
            if (ValidationHelper.RuleBasedValidator<UpdateTrainingCategoryDto>(data, typeof(IUpdateTrainingCategoryBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<CommonResponse>();

            var updData = await categoryRepository.GetAsync(predicate: p => p.Id == data.Id);
            mapper.Map(data, updData);

            categoryRepository.Update(updData);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return ProduceSuccessResponse(new CommonResponse
            {
                Message = "Success",
                Code = StatusCodes.Status200OK,
                Result = true
            });

        }

        public async Task<Response<CommonResponse>> DeleteTrainingCategoryAsync(long id, CancellationToken cancellationToken = default)
        {
            var entityDto = await categoryRepository.GetAsync(predicate: t => t.Id == id, selector: s => mapper.Map<DeleteTrainingCategoryDto>(s));

            if (ValidationHelper.RuleBasedValidator<DeleteTrainingCategoryDto>(entityDto, typeof(IDeleteTrainingCategoryBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<CommonResponse>();

            var updData = await categoryRepository.GetAsync(predicate: p => p.Id == entityDto.Id);
            updData.IsDelete = false;
            updData.DeleteDate = DateTime.UtcNow;
            //updData.DeleteUserId = this.GetCurrentUserId;
            categoryRepository.Update(updData);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return ProduceSuccessResponse(new CommonResponse
            {
                Message = "Success",
                Code = StatusCodes.Status200OK,
                Result = true
            });
        }

        public async Task<Response<IEnumerable<GetTrainingCategoryDto>>> GetListTrainingCategoryAsync()
        {
            var allCategories = await categoryRepository.GetListAsync(predicate: p => (p.IsDelete == false || p.IsDelete == null));
            var allMapped = mapper.Map<List<GetTrainingCategoryDto>>(allCategories);

            var topLevel = allMapped.Where(x => x.MasterCategoryId == null);
            foreach (var parent in topLevel)
            {
                parent.SubCategories = allMapped
                    .Where(x => x.MasterCategoryId == parent.Id)
                    .ToList();
            }

            return ProduceSuccessResponse(topLevel);
        }

        public async Task<Response<GetTrainingCategoryDto>> GetTrainingCategoryAsync(long id)
        {
            var allCategories = await categoryRepository.GetListAsync(
                predicate: p => (p.IsDelete == false || p.IsDelete == null),
                include: i => i.Include(c => c.MasterTrainingCategory)
            );

            var mappedCategories = mapper.Map<List<GetTrainingCategoryDto>>(allCategories);

            foreach (var parent in mappedCategories)
            {
                parent.SubCategories = mappedCategories
                    .Where(x => x.MasterCategoryId == parent.Id)
                    .ToList();
            }

            var selectedCategory = mappedCategories.FirstOrDefault(x => x.Id == id);
            return ProduceSuccessResponse(selectedCategory);
        }

    }
}
