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
using HrHub.Core.Rules;
using HrHub.Abstraction.Extensions;
using HrHub.Abstraction.StatusCodes;
using HrHub.Core.HrFluentValidation;
using HrHub.Application.BusinessRules.UserBusinessRules;

namespace HrHub.Application.Managers.ExamOperationManagers
{
    public class ExamManager : ManagerBase, IExamManager
    {
        private readonly IHrUnitOfWork unitOfWork;
        private readonly Repository<Exam> examRepository;
        private readonly ICommonTypeBaseManager<CertificateType> certificateManager;
        public ExamManager(IHttpContextAccessor httpContextAccessor,
                           IHrUnitOfWork unitOfWork,
                           ICommonTypeBaseManager<CertificateType> certificateManager) : base(httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.examRepository = unitOfWork.CreateRepository<Exam>();
            this.certificateManager = certificateManager;
        }

        public async Task<Response<AddExamResponse>> AddExam(AddExamDto data)
        {
            var validator = new FieldBasedValidator<AddExamDto>();
            var result = validator.Validate(data);

            var validator2 = new ClassBasedValidator<AddExamDto>();
            var result2 = validator2.Validate(data, typeof(UserBusinessRule));

            if (!result2.IsValid)
               return result2.SendResponse<AddExamResponse>();

            return null;
        }
    }
}
