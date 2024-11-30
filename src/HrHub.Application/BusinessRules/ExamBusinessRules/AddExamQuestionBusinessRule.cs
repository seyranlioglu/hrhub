using HrHub.Abstraction.Consts;
using HrHub.Domain.Contracts.Dtos.ExamDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Application.BusinessRules.ExamBusinessRules
{
    public class AddExamQuestionBusinessRule : IAddExamQuestionBusinessRule
    {

        public (bool IsValid, string ErrorMessage) Validate(object value, string fieldName)
        {
            if (value is AddExamQuestionDto examQuestion)
            {
                return (!examQuestion.QuestionOptions.Any() && examQuestion.QuestionOptions.Count < 2)
                    ? (false, ValidationMessages.ExamQuestionCountError) : (true ,string.Empty);
            }
            else
                return (false, ValidationMessages.WrongValidationModelError);
        }
    }
}
