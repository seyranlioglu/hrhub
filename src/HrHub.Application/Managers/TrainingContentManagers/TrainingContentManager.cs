using AutoMapper;
using FluentValidation.Results;
using HrHub.Abstraction.Attributes;
using HrHub.Abstraction.Contracts.Dtos.TrainingDtos;
using HrHub.Abstraction.Extensions;
using HrHub.Abstraction.Result;
using HrHub.Application.BusinessRules.TrainingBusinessRules;
using HrHub.Application.BusinessRules.TrainingContentBusinessRules;
using HrHub.Core.Base;
using HrHub.Core.Data.Repository;
using HrHub.Core.Data.UnitOfWork;
using HrHub.Core.Helpers;
using HrHub.Domain.Contracts.Dtos.TrainingContentDtos;
using HrHub.Domain.Contracts.Responses.CommonResponse;
using HrHub.Domain.Contracts.Responses.TrainingContentResponse;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.Repositories.Abstract;
using HrHub.Infrastructre.Repositories.Concrete;
using HrHub.Infrastructre.UnitOfWorks;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SharpCompress.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HrHub.Application.Managers.TrainingContentManagers
{
    [LifeCycle(Abstraction.Enums.LifeCycleTypes.Scoped)]
    public class TrainingContentManager : ManagerBase, ITrainingContentManager
    {
        private readonly IHrUnitOfWork unitOfWork;
        private readonly Repository<TrainingContent> contentRepository;
        private readonly Repository<ContentLibrary> contentLibraryRepository;
        private readonly IMapper mapper;

        public TrainingContentManager(IHttpContextAccessor httpContextAccessor,
                                      IHrUnitOfWork unitOfWork,
                                      IMapper mapper) : base(httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.contentRepository = unitOfWork.CreateRepository<TrainingContent>();
            this.mapper = mapper;
            this.contentLibraryRepository = unitOfWork.CreateRepository<ContentLibrary>();
        }



        public async Task<Response<ReturnIdResponse>> AddTrainingContentAsync(AddTrainingContentDto data, CancellationToken cancellationToken = default)
        {

            if (ValidationHelper.RuleBasedValidator<AddTrainingContentDto>(data, typeof(IAddTrainingContentBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<ReturnIdResponse>();



            var maxRowNum = (await contentRepository.GetListAsync(predicate: c => c.ContentTypeId == data.ContentTypeId
                                                                                && c.TrainingSectionId == data.TrainingSectionId,
                                                                                orderBy: o => o.OrderByDescending(i => i.OrderId),
                                                                                selector: s => s.OrderId)).FirstOrDefault();


            await unitOfWork.BeginTransactionAsync();
            try
            {
                var newContent = mapper.Map<TrainingContent>(data);
                newContent.OrderId = maxRowNum + 1;
                var result = await contentRepository.AddAndReturnAsync(newContent, cancellationToken);


                var contentLibrary = await contentLibraryRepository.ExistsAsync(predicate: p => p.FileName == data.ContentLibraryFileName);
                if (contentLibrary)
                    return ProduceFailResponse<ReturnIdResponse>("Bu içerik zaten yüklenmiş!", 404);

                var contentLibrayData = mapper.Map<ContentLibrary>(contentLibrary);
                contentLibrayData.TrainingContentId = result.Id;
                await contentLibraryRepository.AddAsync(contentLibrayData, cancellationToken);

                await unitOfWork.SaveChangesAsync(cancellationToken);
                await unitOfWork.CommitTransactionAsync();

                return ProduceSuccessResponse(new ReturnIdResponse
                {
                    Id = newContent.Id
                });
            }
            catch (Exception ex)
            {
                await unitOfWork.RollBackTransactionAsync();
                return ProduceFailResponse<ReturnIdResponse>("İşlem sırasında bir hata oluştu", 404);
            }

        }

        public async Task<Response<CommonResponse>> UpdateTrainingContentAsync(UpdateTrainingContentDto data, CancellationToken cancellationToken = default)
        {
            var validationResult = ValidationHelper.RuleBasedValidator<UpdateTrainingContentDto>(data, typeof(IUpdateTrainingContentBusinessRule));
            if (validationResult is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<CommonResponse>();



            var contentLibraryExists = await contentLibraryRepository.ExistsAsync(p => p.FileName == data.ContentLibraryFileName
                                                                                       && p.TrainingContentId != data.Id);
            if (contentLibraryExists)
                return ProduceFailResponse<CommonResponse>("Bu içerik zaten yüklenmiş!", 404);

            await unitOfWork.BeginTransactionAsync();
            try
            {
                var existingContent = await contentRepository.GetAsync(predicate: p => p.Id == data.Id);
                existingContent = mapper.Map(data, existingContent);
                existingContent.OrderId = data.OrderId;
                existingContent.ContentTypeId = data.ContentTypeId;
                contentRepository.Update(existingContent);

                var contentLibrary = await contentLibraryRepository.GetAsync(predicate: p => p.Id == data.Id);
                contentLibrary.FileName = data.ContentLibraryFileName;
                contentLibrary.FilePath = data.ContentLibraryFilePath;
                await contentLibraryRepository.UpdateAsync(contentLibrary);

                await unitOfWork.SaveChangesAsync(cancellationToken);
                await unitOfWork.CommitTransactionAsync();

                return ProduceSuccessResponse(new CommonResponse
                {
                    Code = StatusCodes.Status200OK,
                    Message = "Success",
                    Result = true
                });
            }
            catch (Exception ex)
            {
                await unitOfWork.RollBackTransactionAsync();
                return ProduceFailResponse<CommonResponse>("İşlem sırasında bir hata oluştu!", 500);
            }
        }

        public async Task<Response<CommonResponse>> DeleteTrainingContentAsync(long id, CancellationToken cancellationToken = default)
        {
            var contentDto = await contentRepository.GetAsync(predicate: p => p.Id == id, selector: s => mapper.Map<DeleteTrainingContentDto>(s));
            if (ValidationHelper.RuleBasedValidator<DeleteTrainingContentDto>(contentDto, typeof(IDeleteTrainingContentBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<CommonResponse>();

            var contentEntity = await contentRepository.GetAsync(predicate: p => p.Id == id);
            contentEntity.IsDelete = true;
            contentRepository.Update(contentEntity);


            var contentLibraryEntity = await contentLibraryRepository.GetListAsync(predicate: p => p.TrainingContent.Id == id);
            contentLibraryEntity.ForEach(i => i.IsDelete = true);
            contentLibraryRepository.UpdateList(contentLibraryEntity.ToList());

            await unitOfWork.BeginTransactionAsync();
            try
            {
                await unitOfWork.SaveChangesAsync(cancellationToken);
                await unitOfWork.CommitTransactionAsync();

                return ProduceSuccessResponse(new CommonResponse
                {
                    Code = StatusCodes.Status200OK,
                    Message = "İçerik başarıyla silindi!",
                    Result = true
                });
            }
            catch (Exception ex)
            {
                await unitOfWork.RollBackTransactionAsync();
                return ProduceFailResponse<CommonResponse>("İşlem sırasında bir hata oluştu!", 500);
            }
        }
        public async Task<Response<IEnumerable<GetTrainingContentDto>>> GetTrainingContentListAsync()
        {
            var trainingList = await contentRepository.GetListAsync(predicate: p => p.IsActive,
                                                                        include: i => i.Include(s => s.ContentType)
                                                                        .Include(s => s.TrainingSection)
                                                                        .Include(s => s.ContentLibraries));
            var trainingListDto = mapper.Map<IEnumerable<GetTrainingContentDto>>(trainingList);
            return ProduceSuccessResponse(trainingListDto);

        }

        public async Task<Response<GetTrainingContentDto>> GetTrainingContentAsync(long id)
        {
            var trainingListDto = await contentRepository.GetAsync(predicate: p => p.IsActive && p.Id == id,
                                                                        include: i => i.Include(s => s.ContentType)
                                                                        .Include(s => s.TrainingSection)
                                                                        .Include(s => s.ContentLibraries),
                                                                        selector: s => mapper.Map<GetTrainingContentDto>(s));
            return ProduceSuccessResponse(trainingListDto);

        }
    }
}
