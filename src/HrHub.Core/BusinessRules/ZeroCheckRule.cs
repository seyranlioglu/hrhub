using HrHub.Abstraction.BusinessRules;
using HrHub.Abstraction.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Core.BusinessRules
{
    public class ZeroCheckRule : IBusinessRule
    {
        public (bool IsValid, string ErrorMessage) Validate(object value, string fieldName)
        {
            if (value == null)
            {
                return (true, string.Empty);
            }

            if (value is int intValue && intValue == 0 ||
                value is long longValue && longValue == 0L ||
                value is decimal decimalValue && decimalValue == 0m ||
                value is double doubleValue && doubleValue == 0.0 ||
                value is float floatValue && Math.Abs(floatValue) < 1e-6)
            {
                return (false, ValidationMessages.ZeroCheckError);
            }

            return (true, string.Empty);
        }
    }
}
