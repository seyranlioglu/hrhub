using HrHub.Domain.Contracts.Dtos.CommonDtos;
using HrHub.Domain.Contracts.Dtos.ExamDtos;
using HrHub.Abstraction.Result;
using HrHub.Application.Managers.TypeManagers;
using HrHub.Core.Base;
using HrHub.Core.Data.Repository;
using HrHub.Core.Data.UnitOfWork;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HrHub.Domain.Contracts.Responses.ExamResponses;
using HrHub.Core.BusinessRules;
using HrHub.Abstraction.Extensions;
using HrHub.Abstraction.StatusCodes;
using HrHub.Core.HrFluentValidation;
using HrHub.Application.BusinessRules.UserBusinessRules;
using AutoMapper;
using HrHub.Domain.Contracts.Responses.CommonResponse;
using HrHub.Core.Helpers;
using HrHub.Abstraction.Settings;
using FluentValidation;
using FluentValidation.Results;
using HrHub.Application.BusinessRules.ExamBusinessRules;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace HrHub.Application.Managers.ExamOperationManagers
{
    public class ExamManager : ManagerBase, IExamManager
    {

        private readonly IHrUnitOfWork unitOfWork;
        private readonly Repository<Exam> examRepository;
        private readonly Repository<ExamVersion> examVersionRepository;
        private readonly Repository<ExamTopic> examTopicRepository;
        private readonly Repository<ExamQuestion> examQuestionRepository;
        private readonly ICommonTypeBaseManager<CertificateType> certificateManager;
        private readonly IMapper mapper;
        public ExamManager(IHttpContextAccessor httpContextAccessor,
                           IHrUnitOfWork unitOfWork,
                           ICommonTypeBaseManager<CertificateType> certificateManager,
                           IMapper mapper,
                           Repository<ExamVersion> examVersionRepository,
                           Repository<ExamTopic> examTopicRepository,
                           Repository<ExamQuestion> examQuestionRepository) : base(httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.examRepository = unitOfWork.CreateRepository<Exam>();
            this.examVersionRepository = unitOfWork.CreateRepository<ExamVersion>();
            this.examTopicRepository = unitOfWork.CreateRepository<ExamTopic>();
            this.examQuestionRepository = unitOfWork.CreateRepository<ExamQuestion>();
            this.certificateManager = certificateManager;
            this.mapper = mapper;
            this.examVersionRepository = examVersionRepository;
            this.examTopicRepository = examTopicRepository;
            this.examQuestionRepository = examQuestionRepository;
        }

        public async Task<Response<AddExamResponse>> AddExamAsync(AddExamDto data, CancellationToken cancellationToken = default)
        {
            var validator = new FieldBasedValidator<AddExamDto>();
            var validateResult = validator.Validate(data);

            if (validateResult.IsValid is false)
                return validateResult.SendResponse<AddExamResponse>();

            var entity = mapper.Map<Exam>(data);
            entity.InstructorId = GetCurrentUserId();
            //entity.ExamVersions.Add(new ExamVersion
            //{
            //    IsPublished = false,
            //    VersionNumber = 1,
            //    ExamTime = data.VersionInfo.ExamTime,
            //    IsActive = true,
            //    PassingScore = data.VersionInfo.PassingScore,
            //    SuccessRate = data.VersionInfo.SuccesRate,
            //    VersionDescription = data.VersionInfo.VersionDescription,
            //    TotalQuestionCount = data.VersionInfo.TotalQuestionCount,

            //});
            var addResponse = await examRepository.AddAndReturnAsync(entity, cancellationToken);
            await unitOfWork.SaveChangesAsync();

            var lastVersion = addResponse.ExamVersions.Where(w => w.IsPublished == true).FirstOrDefault();

            return ProduceSuccessResponse(new AddExamResponse
            {
                Id = addResponse.Id,
                ExamVersionId = lastVersion.Id,
                VersionNumber = lastVersion.VersionNumber
            });
        }

        public async Task<Response<ReturnIdResponse>> AddExamTopic(AddExamTopicDto data)
        {
            var validator = new FieldBasedValidator<AddExamTopicDto>();
            var validateResult = validator.Validate(data);

            if (validateResult.IsValid is false)
                return validateResult.SendResponse<ReturnIdResponse>();

            var entity = mapper.Map<ExamTopic>(data);
            var addResponse = examTopicRepository.AddAndReturnAsync(entity);

            await unitOfWork.SaveChangesAsync();

            return ProduceSuccessResponse(new ReturnIdResponse
            {
                Id = addResponse.Id
            });
        }

        public async Task<Response<ReturnIdResponse>> AddExamQuestion(AddExamQuestionDto question)
        {
            if (ValidationHelper.FieldBasedValidator<AddExamQuestionDto>(question) is ValidationResult validationResult && !validationResult.IsValid)
                return validationResult.SendResponse<ReturnIdResponse>();

            if (ValidationHelper.RuleBasedValidator<AddExamQuestionDto>(question, typeof(AddExamQuestionBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<ReturnIdResponse>();

            var newQuestion = mapper.Map<ExamQuestion>(question);
            var result = await examQuestionRepository.AddAndReturnAsync(newQuestion);
            await unitOfWork.SaveChangesAsync();
            return ProduceSuccessResponse(new ReturnIdResponse
            {
                Id = result.Id
            });
        }
        // TODO Şükrü: Parametreler için TrainingList ve ContentList merthodları yazılmalı 
        public async Task<Response<List<GetExamListResponse>>> GetExamListForGrid(GetExamListDto filter)
        {
            long userId = GetCurrentUserId();
            ExpressionStarter<Exam> predicateBuilder = PredicateBuilder.New<Exam>();

            if (filter.ContentId is not null)
                predicateBuilder = predicateBuilder.And(w => w.ContentExams.Any(w => w.TrainingContentId == filter.ContentId));
            if (filter.TrainingId is not null)
                predicateBuilder = predicateBuilder.And(w => w.TrainingId == filter.TrainingId);
            if (filter.IsActive is not null)
                predicateBuilder = predicateBuilder.And(w => w.IsActive == filter.IsActive);

            if (IsMainUser())
            {
                predicateBuilder = predicateBuilder.And(w => w.Instructor.CurrAcc.Users.Any());
            }
            else if (IsInstructor())
            {
                predicateBuilder = predicateBuilder.And(w => w.InstructorId == userId);
            }

            var examQuery = await examRepository
                .GetListAsync
                (predicate: predicateBuilder,
                include: i => i.Include(w => w.Instructor)
                .ThenInclude(w => w.CurrAcc)
                .Include(w => w.ContentExams)
                .Include(w => w.ExamVersions)
                .Include(w => w.ExamStatus)
                .Include(w => w.Training));

            var examList = examQuery
                .Select(exam =>
                {
                    var publishedVersion = exam.ExamVersions
                        .Where(ev => ev.IsPublished)
                        .FirstOrDefault();
                    return new GetExamListResponse
                    {
                        ExamStatus = exam.ExamStatus.Title,
                        ExamTimeInMin = publishedVersion.ExamTime.TotalMinutes,
                        PassingScore = publishedVersion.PassingScore,
                        SuccessRate = publishedVersion.SuccessRate,
                        Title = exam.Title,
                        TotalQuestionCount = publishedVersion.TotalQuestionCount,
                        TrainingTitle = exam.Training.Title,
                        Versions = exam.ExamVersions.Select(vers => new GetExamVersionListResponse
                        {
                            ExamId = vers.ExamId,
                            ExamTimeInMin = vers.ExamTime.TotalMinutes,
                            PassingScore = vers.PassingScore,
                            SuccessRate = vers.SuccessRate,
                            TotalQuestionCount = vers.TotalQuestionCount,
                            VersionId = vers.Id,
                            VersionNo = vers.VersionNumber
                        }).ToList()
                    };
                }
                );
            return ProduceSuccessResponse(examList.ToList());
        }


    }
}
