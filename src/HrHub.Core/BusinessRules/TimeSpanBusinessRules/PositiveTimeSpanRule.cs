using HrHub.Abstraction.BusinessRules;
using HrHub.Abstraction.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Core.BusinessRules.TimeSpanBusinessRules
{
    public class PositiveTimeSpanRule : IBusinessRule
    {
        public (bool IsValid, string ErrorMessage) Validate(object value, string fieldName)
        {
            if (value is TimeSpan timeSpan)
            {
                bool isValid = timeSpan < TimeSpan.Zero;
                string message = !isValid ? string.Empty : String.Format(ValidationMessages.PositiveTimeSpanError,fieldName);
                return (isValid, message);
            }

            return (false, String.Format(ValidationMessages.PositiveTimeSpanError, fieldName));
        }
    }
}
