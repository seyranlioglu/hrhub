using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Application.BusinessRules.ExamBusinessRules
{
    public class GetExamFilterBusinessRule : IGetExamFilterBusinessRule
    {
        public (bool IsValid, string ErrorMessage) Validate(object value, string fieldName)
        {
            throw new NotImplementedException();
        }
    }
}
