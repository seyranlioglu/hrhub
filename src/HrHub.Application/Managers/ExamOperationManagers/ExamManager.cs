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
using HrHub.Abstraction.Consts;
using HrHub.Application.Factories;
using HrHub.Domain.Contracts.Dtos.NotificationDtos;
using HrHub.Infrastructre.Repositories.Concrete;
using HrHub.Application.Managers.Trainings;
using HrHub.Infrastructre.Repositories.Abstract;

namespace HrHub.Application.Managers.ExamOperationManagers
{
    public class ExamManager : ManagerBase, IExamManager
    {

        private readonly IHrUnitOfWork unitOfWork;
        private readonly Repository<Exam> examRepository;
        private readonly Repository<ExamVersion> examVersionRepository;
        private readonly Repository<ExamTopic> examTopicRepository;
        private readonly Repository<ExamQuestion> examQuestionRepository;
        private readonly Repository<QuestionOption> questionOptionsRepository;
        private readonly Repository<Instructor> instructorRepository;
        private readonly Repository<UserExam> userExamRepository;
        private readonly Repository<ExamAction> examActionRepository;
        private readonly Repository<UserAnswer> userAnswerRepository;
        private readonly ICommonTypeBaseManager<ExamVersionStatus> ExamVersionStatus;
        private readonly MessageSenderFactory messageSenderFactory;
        private readonly IMapper mapper;
        public ExamManager(IHttpContextAccessor httpContextAccessor,
                           IHrUnitOfWork unitOfWork,
                           ICommonTypeBaseManager<ExamVersionStatus> ExamVersionStatus,
                           IMapper mapper,
                           MessageSenderFactory messageSenderFactory) : base(httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.examRepository = unitOfWork.CreateRepository<Exam>();
            this.examVersionRepository = unitOfWork.CreateRepository<ExamVersion>();
            this.examTopicRepository = unitOfWork.CreateRepository<ExamTopic>();
            this.examQuestionRepository = unitOfWork.CreateRepository<ExamQuestion>();
            this.questionOptionsRepository = unitOfWork.CreateRepository<QuestionOption>();
            this.instructorRepository = unitOfWork.CreateRepository<Instructor>();
            this.userExamRepository = unitOfWork.CreateRepository<UserExam>();
            this.examActionRepository = unitOfWork.CreateRepository<ExamAction>();
            this.userAnswerRepository = unitOfWork.CreateRepository<UserAnswer>();
            this.ExamVersionStatus = ExamVersionStatus;
            this.mapper = mapper;
            this.messageSenderFactory = messageSenderFactory;
        }

        public async Task<Response<AddExamResponse>> AddExamAsync(AddExamDto data, CancellationToken cancellationToken = default)
        {
            var validator = new FieldBasedValidator<AddExamDto>();
            var validateResult = validator.Validate(data);

            if (validateResult.IsValid is false)
                return validateResult.SendResponse<AddExamResponse>();

            var entity = mapper.Map<Exam>(data);
            entity.InstructorId = GetCurrentUserId();

            var addResponse = await examRepository.AddAndReturnAsync(entity, cancellationToken);
            await unitOfWork.SaveChangesAsync();

            var lastVersion = addResponse.ExamVersions.Where(w => w.IsPublished == true).FirstOrDefault();

            if (lastVersion is null)
                lastVersion = addResponse.ExamVersions
                    .OrderBy(w => w.CreatedDate)
                    .FirstOrDefault();

            return ProduceSuccessResponse(new AddExamResponse
            {
                Id = addResponse.Id,
                ExamVersionId = lastVersion.Id,
                VersionNumber = lastVersion.VersionNumber
            });
        }

        public async Task<Response<ReturnIdResponse>> AddExamTopicAsync(AddExamTopicDto data, CancellationToken cancellationToken = default)
        {
            var validator = new FieldBasedValidator<AddExamTopicDto>();
            var validateResult = validator.Validate(data);

            //return null;
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

        public async Task<Response<ReturnIdResponse>> AddExamQuestionAsync(AddExamQuestionDto question, CancellationToken cancellationToken = default)
        {
            if (ValidationHelper.FieldBasedValidator<AddExamQuestionDto>(question) is ValidationResult validationResult && !validationResult.IsValid)
                return validationResult.SendResponse<ReturnIdResponse>();

            if (ValidationHelper.RuleBasedValidator<AddExamQuestionDto>(question, typeof(IAddExamQuestionBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
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
        public async Task<Response<List<GetExamListResponse>>> GetExamListForGridAsync(GetExamListDto filter, CancellationToken cancellationToken = default)
        {
            long userId = GetCurrentUserId();
            ExpressionStarter<Exam> predicateBuilder = PredicateBuilder.New<Exam>();

            if (filter.ContentId is not null)
                predicateBuilder = predicateBuilder.And(w => w.TrainingContents.Any(w => w.Id == filter.ContentId));
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
                .Include(w => w.TrainingContents)
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
                        ExamTimeInMin = publishedVersion.ExamTime?.TotalMinutes,
                        PassingScore = publishedVersion.PassingScore,
                        SuccessRate = publishedVersion.SuccessRate,
                        Title = exam.Title,
                        TotalQuestionCount = publishedVersion.TotalQuestionCount,
                        TrainingTitle = exam.Training.Title,
                        Versions = exam.ExamVersions.Select(vers => new GetExamVersionListResponse
                        {
                            ExamId = vers.ExamId,
                            ExamTimeInMin = vers.ExamTime?.TotalMinutes,
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

            if (ValidationHelper.RuleBasedValidator<UpdateExamDto>(updateData, typeof(IExistUserExamBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<CommonResponse>();

            var oldExam = await examRepository.GetAsync(
                predicate: p => p.Id == updateData.Id,
                include: i => i.Include(w => w.ExamVersions.Where(v => v.Id == updateData.VersionInfo.Id)));

            if (ValidationHelper.RuleBasedValidator<Exam>(oldExam, typeof(GetExamFilterBusinessRule)) is ValidationResult oldExamValidator && !oldExamValidator.IsValid)
                return oldExamValidator.SendResponse<CommonResponse>();
            // TODO Şükrü : Burada versionu publish olanı aramak üzerine kurduk ama publish edilmemiş bir version olabilir.
            // O yüzden güncellenmek istenen examın en son versionunu UI a göndermemiz lazım. UI bize versionId yi getirmesi lazım.
            oldExam.Description = updateData.Description;
            oldExam.Title = updateData.Title;
            oldExam.ActionId = updateData.ActionId;
            var versionData = oldExam.ExamVersions.FirstOrDefault();
            versionData.VersionDescription = updateData.VersionInfo.VersionDescription;
            versionData.ExamTime = updateData.VersionInfo.ExamTime;
            versionData.SuccessRate = updateData.VersionInfo.SuccesRate;
            versionData.PassingScore = updateData.VersionInfo.PassingScore;
            versionData.TotalQuestionCount = updateData.VersionInfo.TotalQuestionCount;
            versionData.ExamVersionStatusId = updateData.VersionInfo.StatusId;

            await examRepository.UpdateAsync(oldExam, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return ProduceSuccessResponse(new CommonResponse
            {
                Code = 0,
                Message = "Sınav Bilgileri Başarıyla Güncellendi.",
                Result = true
            });
        }

        public async Task<Response<CommonResponse>> UpdateTopicInfoAsync(UpdateExamTopicDto updateData, CancellationToken cancellationToken = default)
        {
            if (ValidationHelper.FieldBasedValidator<UpdateExamTopicDto>(updateData) is ValidationResult validationResult && !validationResult.IsValid)
                return validationResult.SendResponse<CommonResponse>();

            if (ValidationHelper.RuleBasedValidator<UpdateExamTopicDto>(updateData, typeof(IAddUpdateExamTopicBusinessRule)) is ValidationResult cBasedValidResult && !cBasedValidResult.IsValid)
                return cBasedValidResult.SendResponse<CommonResponse>();

            var oldTopic = await examTopicRepository.GetAsync(
                predicate: p => p.Id == updateData.Id
                );
            oldTopic.ImgPath = updateData.ImgPath;
            oldTopic.SeqNumber = updateData.SeqNumber;
            oldTopic.QuestionCount = updateData.QuestionCount;
            oldTopic.Title = updateData.Title;

            await examTopicRepository.UpdateAsync(oldTopic, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return ProduceSuccessResponse(new CommonResponse
            {
                Code = 0,
                Message = "Sınav Bölüm Bilgileri Başarıyla Güncellendi.",
                Result = true
            });
        }

        public async Task<Response<CommonResponse>> UpdateSeqNoAsync(UpdateExamTopicSeqNumDto updateData, CancellationToken cancellationToken = default)
        {
            if (ValidationHelper.FieldBasedValidator<UpdateExamTopicSeqNumDto>(updateData) is ValidationResult validationResult && !validationResult.IsValid)
                return validationResult.SendResponse<CommonResponse>();

            var topicToMove = await examTopicRepository.GetAsync(
                predicate: p => p.Id == updateData.TopicId,
                include: i => i.Include(w => w.ExamVersion)
                .ThenInclude(w => w.ExamTopics));

            if (ValidationHelper.RuleBasedValidator<Tuple<ExamTopic, int>>(Tuple.Create(topicToMove, updateData.NewSeqNumber), typeof(IUpdateExamTopicSeqNumBusinesRule)) is ValidationResult cValidResult && !cValidResult.IsValid)
                return cValidResult.SendResponse<CommonResponse>();

            int oldSeqNumber = topicToMove.SeqNumber;

            var topics = topicToMove.ExamVersion.ExamTopics
                .OrderBy(w => w.SeqNumber)
                .ToList();

            foreach (var topic in topics)
            {
                if (topic.Id == updateData.TopicId)
                {
                    topic.SeqNumber = updateData.NewSeqNumber;
                }
                else if (oldSeqNumber < updateData.NewSeqNumber && topic.SeqNumber > oldSeqNumber && topic.SeqNumber <= updateData.NewSeqNumber)
                {
                    topic.SeqNumber--;
                }
                else if (oldSeqNumber > updateData.NewSeqNumber && topic.SeqNumber >= updateData.NewSeqNumber && topic.SeqNumber < oldSeqNumber)
                {
                    topic.SeqNumber++;
                }
            }

            await examTopicRepository.UpdateAsync(topicToMove, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return ProduceSuccessResponse(new CommonResponse
            {
                Code = 0,
                Message = "Sınav Bölüm Sırası Başarıyla Güncellendi.",
                Result = true
            });
        }

        public async Task<Response<CommonResponse>> UpdateQuestionAsync(UpdateExamQuestionDto updateData, CancellationToken cancellationToken = default)
        {
            if (ValidationHelper.FieldBasedValidator<UpdateExamQuestionDto>(updateData) is ValidationResult validationResult && !validationResult.IsValid)
                return validationResult.SendResponse<CommonResponse>();

            if (ValidationHelper.RuleBasedValidator<UpdateExamQuestionDto>(updateData, typeof(IExistUserExamBusinessRule)) is ValidationResult rValidResult && !rValidResult.IsValid)
                return rValidResult.SendResponse<CommonResponse>();

            var existingQuestion = await examQuestionRepository.GetAsync(
                predicate: p => p.Id == updateData.Id,
                include: i => i.Include(w => w.QuestionOptions));

            if (existingQuestion is null)
                return ProduceFailResponse<CommonResponse>(ValidationMessages.DataNotFound, HrStatusCodes.Status111DataNotFound);

            existingQuestion.QuestionText = updateData.QuestionText;
            existingQuestion.Score = updateData.Score;

            var existingOptions = existingQuestion.QuestionOptions.ToList();

            // Güncellenecek veya eklenecek seçenekleri belirle
            foreach (var optionDto in updateData.QuestionOptions)
            {
                var existingOption = existingOptions.FirstOrDefault(o => o.Id == optionDto.Id);

                if (existingOption != null)
                {
                    // Mevcut option'ı güncelle
                    existingOption.OptionText = optionDto.OptionText;
                    existingOption.IsCorrect = optionDto.IsCorrect;
                }
                else
                {
                    // Yeni option ekle
                    existingQuestion.QuestionOptions.Add(new QuestionOption
                    {
                        OptionText = optionDto.OptionText,
                        IsCorrect = optionDto.IsCorrect
                    });
                }
            }

            // Silinecek seçenekleri belirle
            var optionsToRemove = existingOptions
                .Where(o => updateData.QuestionOptions.All(uo => uo.Id != o.Id))
                .ToList();

            // Silinecek seçenekleri kaldır
            foreach (var option in optionsToRemove)
            {
                existingQuestion.QuestionOptions.Remove(option);
            }

            await examQuestionRepository.UpdateAsync(existingQuestion);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return ProduceSuccessResponse(new CommonResponse
            {
                Result = true,
                Message = "Soru başarıyla güncellendi.",
                Code = 0
            });
        }

        public async Task<Response<GetExamResponse>> GetExamByIdWithStudentAsync(GetExamDto filter, CancellationToken cancellationToken = default)
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
                                    .Take(et.QuestionCount ?? 5) // Başlıktaki soru sayısına göre al
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
                ExamTime = examData.PublishedVersion.ExamTime.Value,
                SuccessRate = examData.PublishedVersion.SuccessRate.Value,
                PassingScore = examData.PublishedVersion.PassingScore.Value,
                TotalQuestionCount = examData.PublishedVersion.TotalQuestionCount.Value,
                Topics = examData.PublishedVersion.Topics.Select(t => new GetExamTopicResponse
                {
                    Id = t.Id,
                    Title = t.Title,
                    ImgPath = t.ImgPath,
                    QuestionCount = t.QuestionCount.Value,
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

        public async Task<Response<PublishExamVersionResponse>> PublishExamVersionAsync(PublishExamVersionDto request, CancellationToken cancellationToken = default)
        {
            var response = new PublishExamVersionResponse { Result = true };

            // Validate the incoming DTO using the FieldBasedValidator
            if (ValidationHelper.FieldBasedValidator<PublishExamVersionDto>(request) is ValidationResult validationResult && !validationResult.IsValid)
            {
                response.Result = false;
                response.ExamError = validationResult.Errors.FirstOrDefault()?.ErrorMessage;
                return ProduceSuccessResponse(response);
            }

            // Fetch the exam along with its related data into DTOs using the selector
            var examDto = await examRepository.GetAsync(
                predicate: e => e.Id == request.ExamId,
                selector: e => new ExamCheckDto
                {
                    ExamId = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    ExamVersions = e.ExamVersions.Where(ev => ev.Id == request.VersionId && ev.VersionNumber == request.VersionNumber)
                                                 .Select(ev => new ExamVersionCheckDto
                                                 {
                                                     VersionId = ev.Id,
                                                     VersionNumber = ev.VersionNumber,
                                                     VersionDescription = ev.VersionDescription,
                                                     IsPublished = ev.IsPublished,
                                                     ExamTime = ev.ExamTime,
                                                     PassingScore = ev.PassingScore,
                                                     ExamTopics = ev.ExamTopics.Select(et => new ExamTopicCheckDto
                                                     {
                                                         TopicId = et.Id,
                                                         Title = et.Title,
                                                         QuestionCount = et.QuestionCount,
                                                         ExamQuestions = et.ExamQuestions.Select(eq => new ExamQuestionCheckDto
                                                         {
                                                             QuestionId = eq.Id,
                                                             QuestionText = eq.QuestionText,
                                                             Score = eq.Score,
                                                             QuestionOptions = eq.QuestionOptions.Select(qo => new QuestionOptionCheckDto
                                                             {
                                                                 OptionId = qo.Id,
                                                                 OptionText = qo.OptionText,
                                                                 IsCorrect = qo.IsCorrect
                                                             }).ToList()
                                                         }).ToList()
                                                     }).ToList()
                                                 }).ToList()
                });

            // Validate the Exam DTO
            var examValidationResult = ValidationHelper.FieldBasedValidator<ExamCheckDto>(examDto);
            if (examValidationResult is ValidationResult examResult && !examResult.IsValid)
            {
                response.Result = false;
                response.ExamError = examResult.Errors.FirstOrDefault()?.ErrorMessage;
            }

            // Validate the ExamVersion DTO
            var examVersionDto = examDto.ExamVersions.FirstOrDefault();
            var versionValidationResult = ValidationHelper.FieldBasedValidator<ExamVersionCheckDto>(examVersionDto);
            if (versionValidationResult is ValidationResult versionResult && !versionResult.IsValid)
            {
                response.Result = false;
                response.ExamVersionError = versionResult.Errors.FirstOrDefault()?.ErrorMessage;
            }

            // Validate ExamTopics and ExamQuestions within the DTO
            foreach (var topic in examVersionDto.ExamTopics)
            {
                var topicValidationResult = ValidationHelper.FieldBasedValidator<ExamTopicCheckDto>(topic);
                if (topicValidationResult is ValidationResult topicResult && !topicResult.IsValid)
                {
                    response.Result = false;
                    response.ExamTopicErrors.AddRange(topicResult.Errors.Select(e => e.ErrorMessage));
                }

                foreach (var question in topic.ExamQuestions)
                {
                    var questionValidationResult = ValidationHelper.FieldBasedValidator<ExamQuestionCheckDto>(question);
                    if (questionValidationResult is ValidationResult questionResult && !questionResult.IsValid)
                    {
                        response.Result = false;
                        response.ExamQuestionErrors.AddRange(questionResult.Errors.Select(e => e.ErrorMessage));
                    }
                }
            }

            if (!response.Result)
            {
                return ProduceSuccessResponse(response);
            }

            // Fetch the original target version entity
            var targetVersion = await examVersionRepository.GetAsync(ev => ev.Id == request.VersionId);
            if (targetVersion == null)
            {
                return ProduceFailResponse<PublishExamVersionResponse>("Target exam version not found.", HrStatusCodes.Status111DataNotFound);
            }

            // Check and update previously published version if exists
            var previouslyPublishedVersion = await examVersionRepository.GetAsync(ev => ev.ExamId == request.ExamId && ev.IsPublished);
            if (previouslyPublishedVersion != null)
            {
                previouslyPublishedVersion.IsPublished = false;
                await examVersionRepository.UpdateAsync(previouslyPublishedVersion, cancellationToken);
            }

            // Update the target version as published
            targetVersion.IsPublished = true;
            targetVersion.PublishedDate = DateTime.UtcNow;
            await examVersionRepository.UpdateAsync(targetVersion, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            // Return a success response
            return ProduceSuccessResponse(response);
        }

        public async Task<Response<GetExamInstructionResponse>> PrePrepareExamForStudentAsync(GetExamDto filter, CancellationToken cancellationToken = default)
        {
            var validator = new FieldBasedValidator<GetExamDto>();
            var validateResult = validator.Validate(filter);

            if (!validateResult.IsValid)
                return validateResult.SendResponse<GetExamInstructionResponse>();

            // Fetch only exam and question details dynamically with includes
            var examData = await examRepository.GetAsync(
                    e => e.Id == filter.ExamId
                    && e.IsActive == true
                    && (e.IsDelete == false || e.IsDelete == null)
                    && e.ExamVersions.Any(ev => ev.IsPublished == true
                                                && ev.IsActive == true
                                                && (ev.IsDelete == false || ev.IsDelete == null)),
                    include: q => q.Include(e => e.ExamVersions)
                                    .ThenInclude(ev => ev.ExamTopics)
                                    .ThenInclude(et => et.ExamQuestions)
                                    .ThenInclude(eq => eq.QuestionOptions),
                    selector: e => new
                    {
                        ExamVersionId = e.ExamVersions.Where(ev => ev.IsPublished).Select(ev => ev.Id).FirstOrDefault(),
                        Title = e.Title,
                        Description = e.Description,
                        SuccessRate = e.ExamVersions.Where(ev => ev.IsPublished).Select(ev => ev.SuccessRate).FirstOrDefault(),
                        ExamTime = e.ExamVersions.Where(ev => ev.IsPublished).Select(ev => ev.ExamTime).FirstOrDefault(),
                        TopicsName = e.ExamVersions.Where(ev => ev.IsPublished).SelectMany(ev => ev.ExamTopics).Select(t => t.Title).ToArray(),
                        PassingScore = e.ExamVersions.Where(ev => ev.IsPublished).Select(ev => ev.PassingScore).FirstOrDefault(),
                        Questions = e.ExamVersions.Where(ev => ev.IsPublished)
                                               .SelectMany(ev => ev.ExamTopics)
                                               .SelectMany(et => et.ExamQuestions)
                                               .Select((q, index) => new
                                               {
                                                   QuestionId = q.Id,
                                                   QuestionText = q.QuestionText,
                                                   SeqNumber = index + 1,
                                                   Options = q.QuestionOptions.Select(qo => new
                                                   {
                                                       OptionId = qo.Id,
                                                       OptionText = qo.OptionText
                                                   }).ToList()
                                               }).ToList()
                    });

            if (examData == null || !examData.Questions.Any())
                return ProduceFailResponse<GetExamInstructionResponse>("Exam not ready!", HrStatusCodes.Status111DataNotFound);

            // Save UserExam and UserAnswers in a single transaction
            var userExam = new UserExam
            {
                ExamVersionId = examData.ExamVersionId,
                IsCompleted = false,
                StartDate = null, // Not started yet
                SuccessRate = examData.SuccessRate ?? 0,
                PassingScore = examData.PassingScore ?? 0,
                UserAnswers = examData.Questions.Select(q => new UserAnswer
                {
                    QuestionId = q.QuestionId,
                    SeqNumber = q.SeqNumber
                }).ToList()
            };

            await userExamRepository.AddAsync(userExam, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            var response = new GetExamInstructionResponse
            {
                ExamVersionId = examData.ExamVersionId,
                UserExamId = userExam.Id,
                Title = examData.Title,
                Description = examData.Description,
                ExamTime = examData.ExamTime,
                TotalQuestionCount = examData.Questions.Count,
                TopicsNames = examData.TopicsName.ToList()
            };

            return ProduceSuccessResponse(response);
        }

        public async Task<Response<GetNextQuestionResponse>> GetNextQuestionAsync(GetNextQuestionDto filter, CancellationToken cancellationToken = default)
        {
            var userAnswers = await userAnswerRepository.GetQuery(
                ua => ua.UserExamId == filter.UserExamId && ua.SeqNumber > filter.CurrentQuestionSeqNum,
                include: ua => ua.Include(u => u.Question)
                                 .ThenInclude(q => q.QuestionOptions)
                                 .Include(u => u.Question)
                                 .ThenInclude(q => q.ExamTopics))
                .OrderBy(ua => ua.SeqNumber)
                .FirstOrDefaultAsync();

            if (userAnswers == null)
            {
                // Mark exam as completed
                var userExam = await userExamRepository.GetAsync(ue => ue.Id == filter.UserExamId);
                if (userExam != null)
                {
                    userExam.IsCompleted = true;
                    await userExamRepository.UpdateAsync(userExam, cancellationToken);
                    await unitOfWork.SaveChangesAsync(cancellationToken);
                }

                return ProduceSuccessResponse(new GetNextQuestionResponse
                {
                    UserExamId = filter.UserExamId,
                    IsCompleted = true,
                    ExamEndMessage = "Exam completed successfully."
                });
            }

            // Check if this is the last question in the exam
            bool isLastQuestion = !await userAnswerRepository.GetQuery(
                ua => ua.UserExamId == filter.UserExamId && ua.SeqNumber > userAnswers.SeqNumber
            ).AnyAsync();

            var response = new GetNextQuestionResponse
            {
                UserExamId = filter.UserExamId,
                IsCompleted = false,
                IsLastQuestion = isLastQuestion,
                CurrentQuestion = new GetQuestionResponse
                {
                    Id = userAnswers.Question.Id,
                    QuestionText = userAnswers.Question.QuestionText,
                    Options = userAnswers.Question.QuestionOptions.Select(qo => new GetQuestionOptionsResponse
                    {
                        Id = qo.Id,
                        OptionText = qo.OptionText
                    }).ToList()
                },
                TopicTitle = userAnswers.Question.ExamTopics.Title,
                TopicImgPath = userAnswers.Question.ExamTopics.ImgPath
            };

            return ProduceSuccessResponse(response);
        }

        public async Task<Response<CalculateExamResultResponse>> CalculateUserExamResultAsync(long userExamId, CancellationToken cancellationToken = default)
        {
            // Fetch UserExam with associated answers and exam details
            var userExam = await userExamRepository.GetQuery(
                ue => ue.Id == userExamId && ue.IsCompleted,
                include: ue => ue.Include(e => e.ExamVersion)
                                 .ThenInclude(ev => ev.ExamTopics)
                                 .ThenInclude(et => et.ExamQuestions)
                                 .ThenInclude(eq => eq.QuestionOptions)
                                 .Include(u => u.UserAnswers))
                .FirstOrDefaultAsync();

            if (userExam == null)
                return ProduceFailResponse<CalculateExamResultResponse>("Completed exam not found.", HrStatusCodes.Status111DataNotFound);

            var totalScore = 0m;
            var userScore = 0m;

            foreach (var topic in userExam.ExamVersion.ExamTopics)
            {
                foreach (var question in topic.ExamQuestions)
                {
                    totalScore += question.Score;

                    var userAnswer = userExam.UserAnswers.FirstOrDefault(ua => ua.QuestionId == question.Id);
                    if (userAnswer != null)
                    {
                        var correctOption = question.QuestionOptions.FirstOrDefault(qo => qo.IsCorrect);
                        if (correctOption != null && userAnswer.SelectedOptionId == correctOption.Id)
                        {
                            userScore += question.Score;
                        }
                    }
                }
            }

            var successRate = (userScore / totalScore) * 100;
            var passingScore = userExam.ExamVersion.PassingScore ?? 0;

            // Determine if the user passed the exam
            var isSuccess = successRate >= passingScore;

            // Update UserExam with the results
            userExam.TotalScore = totalScore;
            userExam.ExamScore = userScore;
            userExam.SuccessRate = successRate;
            userExam.IsSuccess = isSuccess;
            await userExamRepository.UpdateAsync(userExam, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            // Execute actions based on the result
            var examAction = await examActionRepository.GetQuery(
            ea => ea.Exams.Any(e => e.Id == userExam.ExamVersionId))
            .FirstOrDefaultAsync();
            if (examAction != null)
            {
                if (!isSuccess)
                {
                    if (examAction.Title == "ResetTraining")
                    {
                        // TODO : Eğitim resetleme işlemi burada yapılacak.
                        //await trainingManager.ResetTrainingAsync(userExam.UserId, cancellationToken);
                    }
                    else if (examAction.Title == "ResetTopic")
                    {
                        // TODO : Eğitim resetleme işlemi burada yapılacak.
                        //await trainingManager.ResetTopicAsync(userExam.UserId, userExam.Exam.Id, cancellationToken);
                    }
                }
            }

            return ProduceSuccessResponse(new CalculateExamResultResponse
            {
                UserExamId = userExam.Id,
                TotalScore = totalScore,
                UserScore = userScore,
                SuccessRate = successRate,
                IsSuccess = isSuccess,
                Message = isSuccess ? "Congratulations, you passed the exam!" : "Unfortunately, you did not meet the passing criteria."
            });
        }
    }
}
