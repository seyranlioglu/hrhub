using HrHub.Abstraction.BusinessRules;
using HrHub.Abstraction.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Core.BusinessRules
{
    public class NullCheckRule : IBusinessRule
    {
        public (bool IsValid, string ErrorMessage) Validate(object value, string fieldName)
        {
            var isValid = value != null;
            var message = isValid ? string.Empty : string.Format(ValidationMessages.NullError, fieldName);
            return (isValid, message);
        }
    }
}
