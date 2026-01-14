using HrHub.Abstraction.BusinessRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Core.BusinessRules
{
    public class TrainingExpiredRule : IBusinessRule
    {
        public (bool IsValid, string ErrorMessage) Validate(object value, string fieldName)
        {
            if (value == null) return (true, string.Empty);

            var dueDate = (DateTime)value;
            var isValid = DateTime.UtcNow <= dueDate;
            var message = isValid ? string.Empty : "Bu eğitimin süresi dolmuştur. Lütfen yöneticinizle iletişime geçin.";
            return (isValid, message);
        }
    }
}
