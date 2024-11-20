using HrHub.Abstraction.BusinessRules;
using HrHub.Abstraction.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Core.BusinessRules
{
    public class EmailFormatRule : IBusinessRule
    {
        public (bool IsValid, string ErrorMessage) Validate(object value, string fieldName)
        {
            if (value is string email)
            {
                var isValid = !string.IsNullOrEmpty(email) &&
                              System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$");
                var message = isValid ? string.Empty : string.Format(ValidationMessages.InvalidEmailError, fieldName);
                return (isValid, message);
            }

            return (false, string.Format(ValidationMessages.InvalidEmailError, fieldName));
        }
    }
}
