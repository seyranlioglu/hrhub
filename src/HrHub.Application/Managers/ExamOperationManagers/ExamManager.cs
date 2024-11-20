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

        public async Task<Response<AddExamResponse>> AddExamAsync(AddExamDto data,CancellationToken cancellationToken = default)
        {
            var validator = new FieldBasedValidator<AddExamDto>();
            var validateResult = validator.Validate(data);

            if (validateResult.IsValid is false)
                return validateResult.SendResponse<AddExamResponse>();

            var entity = mapper.Map<Exam>(data);
            entity.ExamVersions.Add(new ExamVersion
            {
                EffectiveFrom = DateTime.Now.AddDays(AppSettingsHelper.GetData<ApplicationSettings>().ExamValidityTime),
                IsPublished = false,
                VersionNumber = 1
            });
            var addResponse = await examRepository.AddAndReturnAsync(entity, cancellationToken);
            await unitOfWork.SaveChangesAsync();

            var lastVersion = addResponse.ExamVersions.Where(w => w.IsPublished == true).FirstOrDefault();

            return ProduceSuccessResponse(new AddExamResponse
            {
                Id = addResponse.Id,
                ExamVersionId = lastVersion.Id,
                VersionNumber = lastVersion.VersionNumber
            });
            //var validator2 = new ClassBasedValidator<AddExamDto>();
            //var result2 = validator2.Validate(data, typeof(UserBusinessRule));
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
    }
}
