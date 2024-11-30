using HrHub.Abstraction.Consts;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Contracts.Dtos.ExamDtos;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Application.BusinessRules.ExamBusinessRules
{
    public class ExistUserExamBusinessRule : IExistUserExamBusinessRule
    {
        private readonly IHrUnitOfWork unitOfWork;
        private readonly Repository<UserExam> userExamRepository;
        public ExistUserExamBusinessRule(IHrUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            userExamRepository = unitOfWork.CreateRepository<UserExam>();
        }

        /// <summary>
        /// value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public (bool IsValid, string ErrorMessage) Validate(object value, string fieldName)
        {
            if (value is UpdateExamDto exam && exam is not null) 
            {
                var existUserExam = userExamRepository
                    .Exists(
                    predicate: p => p.ExamVersion.ExamId == exam.Id);

                (bool isValid, string errorMessage) result = existUserExam ? (false, ValidationMessages.ExamUserExistsError)
                : (true, string.Empty);

                return result;
            }
            else
                return (false, ValidationMessages.WrongValidationModelError);
        }
    }
}
