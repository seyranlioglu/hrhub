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
    public class AddUpdateExamTopicBusinessRule : IAddUpdateExamTopicBusinessRule
    {
        private readonly HrUnitOfWork unitOfWork;
        private readonly Repository<ExamTopic> examTopicRepository;
        private readonly Repository<Exam> examRepository;
        public AddUpdateExamTopicBusinessRule(HrUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            examTopicRepository = unitOfWork.CreateRepository<ExamTopic>();
            examRepository = unitOfWork.CreateRepository<Exam>();
        }

        public (bool IsValid, string ErrorMessage) Validate(object value, string fieldName)
        {
            var result = (true, string.Empty);
            if (value is UpdateExamTopicDto updateData) 
            {
                result = examRepository.Exists(w => w.ExamVersions.Any(v => v.ExamTopics.Any(t => t.Title == updateData.Title))) ?
                     (false, ValidationMessages.DataAlreadyExists) : result;
            }
            else
                result = (false, ValidationMessages.WrongValidationModelError);

            return result;
        }
    }
}
