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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HrHub.Application.Managers.ExamOperationManagers
{
    public class ExamManager : ManagerBase//, IExamManager
    {

        private readonly IHrUnitOfWork unitOfWork;
        private readonly Repository<Exam> examRepository;
        private readonly Repository<ExamVersion> examVersionRepository;
        private readonly Repository<ExamTopic> examTopicRepository;
        private readonly Repository<ExamQuestion> examQuestionRepository;
        private readonly Repository<QuestionOption> questionOptionsRepository;
        private readonly Repository<Instructor> instructorRepository;
        private readonly ICommonTypeBaseManager<ExamVersionStatus> ExamVersionStatus;
        private readonly IMapper mapper;
        public ExamManager(IHttpContextAccessor httpContextAccessor,
                           IHrUnitOfWork unitOfWork,
                           ICommonTypeBaseManager<ExamVersionStatus> ExamVersionStatus,
                           IMapper mapper) : base(httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.examRepository = unitOfWork.CreateRepository<Exam>();
            this.examVersionRepository = unitOfWork.CreateRepository<ExamVersion>();
            this.examTopicRepository = unitOfWork.CreateRepository<ExamTopic>();
            this.examQuestionRepository = unitOfWork.CreateRepository<ExamQuestion>();
            this.questionOptionsRepository = unitOfWork.CreateRepository<QuestionOption>();
            this.instructorRepository = unitOfWork.CreateRepository<Instructor>();
            this.ExamVersionStatus = ExamVersionStatus;
            this.mapper = mapper;
        }

        //public async Task<Response<AddExamResponse>> AddExamAsync(AddExamDto data, CancellationToken cancellationToken = default)
        //{
        //    var validator = new FieldBasedValidator<AddExamDto>();
        //    var validateResult = validator.Validate(data);

        //    if (validateResult.IsValid is false)
        //        return validateResult.SendResponse<AddExamResponse>();

        //    var entity = mapper.Map<Exam>(data);
        //    entity.InstructorId = GetCurrentUserId();
        //    //entity.ExamVersions.Add(new ExamVersion
        //    //{
        //    //    IsPublished = false,
        //    //    VersionNumber = 1,
        //    //    ExamTime = data.VersionInfo.ExamTime,
        //    //    IsActive = true,
        //    //    PassingScore = data.VersionInfo.PassingScore,
        //    //    SuccessRate = data.VersionInfo.SuccesRate,
        //    //    VersionDescription = data.VersionInfo.VersionDescription,
        //    //    TotalQuestionCount = data.VersionInfo.TotalQuestionCount,

        //    //});
        //    var addResponse = await examRepository.AddAndReturnAsync(entity, cancellationToken);
        //    await unitOfWork.SaveChangesAsync();

        //    var lastVersion = addResponse.ExamVersions.Where(w => w.IsPublished == true).FirstOrDefault();

        //    return ProduceSuccessResponse(new AddExamResponse
        //    {
        //        Id = addResponse.Id,
        //        ExamVersionId = lastVersion.Id,
        //        VersionNumber = lastVersion.VersionNumber
        //    });
        //}

        public async Task<Response<ReturnIdResponse>> AddExamTopicAsync(AddExamTopicDto data, CancellationToken cancellationToken = default)
        {
            var validator = new FieldBasedValidator<AddExamTopicDto>();
            var validateResult = validator.Validate(data);

            return null;
        //    if (validateResult.IsValid is false)
        //        return validateResult.SendResponse<ReturnIdResponse>();

        //    var entity = mapper.Map<ExamTopic>(data);
        //    var addResponse = examTopicRepository.AddAndReturnAsync(entity);

        //    await unitOfWork.SaveChangesAsync();

        //    return ProduceSuccessResponse(new ReturnIdResponse
        //    {
        //        Id = addResponse.Id
        //    });
        }

        public async Task<Response<ReturnIdResponse>> AddExamQuestionAsync(AddExamQuestionDto question, CancellationToken cancellationToken = default)
        {
            if (ValidationHelper.FieldBasedValidator<AddExamQuestionDto>(question) is ValidationResult validationResult && !validationResult.IsValid)
                return validationResult.SendResponse<ReturnIdResponse>();

        //    if (ValidationHelper.RuleBasedValidator<AddExamQuestionDto>(question, typeof(AddExamQuestionBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
        //        return cBasedValidResult.SendResponse<ReturnIdResponse>();

            var newQuestion = mapper.Map<ExamQuestion>(question);
            var result = await examQuestionRepository.AddAndReturnAsync(newQuestion);
            await unitOfWork.SaveChangesAsync();
            return ProduceSuccessResponse(new ReturnIdResponse
            {
                Id = result.Id
            });
        }

        // TODO Şükrü: Parametreler için TrainingList ve ContentList merthodları yazılmalı 
        public async Task<Response<List<GetExamListResponse>>> GetExamListForGridAsync(GetExamListDto filter, CancellationToken cancellationToken = default)
        {
            long userId = GetCurrentUserId();
            ExpressionStarter<Exam> predicateBuilder = PredicateBuilder.New<Exam>();

            //if (filter.ContentId is not null)
            //    predicateBuilder = predicateBuilder.And(w => w.ContentExams.Any(w => w.TrainingContentId == filter.ContentId));
            if (filter.TrainingId is not null)
                predicateBuilder = predicateBuilder.And(w => w.TrainingId == filter.TrainingId);
            if (filter.IsActive is not null)
                predicateBuilder = predicateBuilder.And(w => w.IsActive == filter.IsActive);

            if (IsMainUser())
            {
                var currAccId = GetCurrAccId();

                var instructorList = instructorRepository
                .GetListWithNoLock(
                    predicate: ins => ins.User.CurrAccId == currAccId,
                    selector: s => s.Id).ToList();

                predicateBuilder = predicateBuilder.And(w => instructorList.Contains(w.InstructorId));
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
                //.Include(w => w.ContentExams)
                .Include(w => w.ExamVersions)
                .Include(w => w.ExamStatus)
                .Include(w => w.Training));

            //    var examList = examQuery
            //        .Select(exam =>
            //        {
            //            var publishedVersion = exam.ExamVersions
            //                .Where(ev => ev.IsPublished)
            //                .FirstOrDefault();
            //            return new GetExamListResponse
            //            {
            //                ExamStatus = exam.ExamStatus.Title,
            //                ExamTimeInMin = publishedVersion.ExamTime.TotalMinutes,
            //                PassingScore = publishedVersion.PassingScore,
            //                SuccessRate = publishedVersion.SuccessRate,
            //                Title = exam.Title,
            //                TotalQuestionCount = publishedVersion.TotalQuestionCount,
            //                TrainingTitle = exam.Training.Title,
            //                Versions = exam.ExamVersions.Select(vers => new GetExamVersionListResponse
            //                {
            //                    ExamId = vers.ExamId,
            //                    ExamTimeInMin = vers.ExamTime.TotalMinutes,
            //                    PassingScore = vers.PassingScore,
            //                    SuccessRate = vers.SuccessRate,
            //                    TotalQuestionCount = vers.TotalQuestionCount,
            //                    VersionId = vers.Id,
            //                    VersionNo = vers.VersionNumber
            //                }).ToList()
            //            };
            //        }
            //        );
            //   return ProduceSuccessResponse(examList.ToList());
            return null;
        }

        public async Task<Response<AddExamVersionReponse>> AddNewVersionAsync(AddNewVersionDto versionData, CancellationToken cancellationToken = default)
        {
            var oldVersion = await examVersionRepository.GetAsync(
                predicate: p => p.ExamId == versionData.ExamId
                && p.IsPublished == true,
                include: i => i.Include(w => w.ExamTopics)
                .ThenInclude(w => w.ExamQuestions)
                .ThenInclude(w => w.QuestionOptions));

            if (oldVersion is null)
                return ProduceFailResponse<AddExamVersionReponse>("Active Version Not Found", HrStatusCodes.Status111DataNotFound);

            var newEntity = mapper.Map<ExamVersion>(oldVersion);

            var response = await examVersionRepository.AddAndReturnAsync(newEntity, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return ProduceSuccessResponse(new AddExamVersionReponse
            {
                NewVersionId = response.Id,
                NewVersionNumber = response.VersionNumber
            });
        }

        public async Task<Response<CommonResponse>> UpdateExamInfoAsync(UpdateExamDto updateData, CancellationToken cancellationToken = default)
        {
            if (ValidationHelper.FieldBasedValidator<UpdateExamDto>(updateData) is ValidationResult validationResult && !validationResult.IsValid)
                return validationResult.SendResponse<CommonResponse>();

            if (ValidationHelper.RuleBasedValidator<UpdateExamDto>(updateData, typeof(ExistUserExamBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<CommonResponse>();



            var oldExam = await examRepository.GetAsync(
                predicate: p => p.Id == updateData.Id,
                include: i => i.Include(w => w.ExamVersions.Where(v => v.IsPublished)));
            return null;
        }

        public async Task<Response<GetExamResponse>> GetExamByIdAsync(GetExamDto filter, CancellationToken cancellationToken = default)
        {
            var validator = new FieldBasedValidator<GetExamDto>();
            var validateResult = validator.Validate(filter);

            if (validateResult.IsValid is false)
                return validateResult.SendResponse<GetExamResponse>();

            var examData = await examRepository.GetQuery(
                    e => e.Id == filter.ExamId
                    && e.IsActive == true
                    && (e.IsDelete == false || e.IsDelete == null)
                    && e.ExamVersions.Any(ev => ev.IsPublished == true
                                            && ev.IsActive == true
                                            && (ev.IsDelete == false || ev.IsDelete == null))
                    , include: q => q.Include(e => e.ExamVersions)
                                    .ThenInclude(ev => ev.ExamTopics)
                                    .ThenInclude(et => et.ExamQuestions)
                ).Select(e => new
                {
                    e.Id,
                    e.Title,
                    e.Description,
                    PublishedVersion = e.ExamVersions
                        .Where(ev => ev.IsPublished)
                        .Select(ev => new
                        {
                            ev.ExamTime,
                            ev.SuccessRate,
                            ev.PassingScore,
                            ev.TotalQuestionCount,
                            Topics = ev.ExamTopics.Select(et => new
                            {
                                et.Id,
                                et.QuestionCount,
                                et.Title,
                                et.ImgPath,
                                Questions = et.ExamQuestions
                                    .OrderBy(q => Guid.NewGuid()) // Rastgele sıralama
                                    .Take(et.QuestionCount) // Başlıktaki soru sayısına göre al
                                    .Select(q => new
                                    {
                                        q.Id,
                                        q.QuestionText
                                    }).ToList()
                            }).ToList()
                        }).FirstOrDefault()
                }).FirstOrDefaultAsync();

            if (examData is null
                || examData.PublishedVersion is null
                || examData.PublishedVersion.Topics is null
                || examData.PublishedVersion.Topics.Count == 0)
                ProduceFailResponse<GetExamResponse>("Exam Not Ready!", HrStatusCodes.Status111DataNotFound);

            var questionIds = examData.PublishedVersion.Topics
                    .SelectMany(t => t.Questions)
                    .Select(q => q.Id)
                    .ToList();

            var questionOptions = await questionOptionsRepository.GetQuery(
                o => questionIds.Contains(o.QuestionId)
            ).Select(o => new
            {
                o.Id,
                o.OptionText,
                o.QuestionId
            }).ToListAsync();

            var response = new GetExamResponse
            {
                ExamId = examData.Id,
                Title = examData.Title,
                Description = examData.Description,
                ExamTime = examData.PublishedVersion.ExamTime,
                SuccessRate = examData.PublishedVersion.SuccessRate,
                PassingScore = examData.PublishedVersion.PassingScore,
                TotalQuestionCount = examData.PublishedVersion.TotalQuestionCount,
                Topics = examData.PublishedVersion.Topics.Select(t => new GetExamTopicResponse
                {
                    Id = t.Id,
                    Title = t.Title,
                    ImgPath = t.ImgPath,
                    QuestionCount = t.QuestionCount,
                    Questions = t.Questions.Select(q => new GetQuestionResponse
                    {
                        Id = q.Id,
                        QuestionText = q.QuestionText,
                        Options = questionOptions
                            .Where(o => o.QuestionId == q.Id)
                            .Select(o => new GetQuestionOptionsResponse
                            {
                                Id = o.Id,
                                OptionText = o.OptionText
                            }).ToList()
                    }).ToList()
                }).ToList()
            };

            return ProduceSuccessResponse(response);
        }

    }
}
