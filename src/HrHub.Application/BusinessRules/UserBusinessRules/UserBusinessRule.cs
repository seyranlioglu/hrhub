using HrHub.Abstraction.BusinessRules;
using HrHub.Abstraction.Consts;
using HrHub.Domain.Entities.SqlDbEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Application.BusinessRules.UserBusinessRules
{
    public class UserBusinessRule : IUserBusinessRule
    {
        public (bool IsValid, string ErrorMessage) Validate(object value, string fieldName)
        {
            User user = (User)value;

            return (false, string.Format(ValidationMessages.InvalidEmailError, fieldName));
        }
    }
}
