using HrHub.Abstraction.BusinessRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Core.BusinessRules
{
    public class TrainingNotStartedRule : IBusinessRule
    {
        public (bool IsValid, string ErrorMessage) Validate(object value, string fieldName)
        {
            if (value == null) return (true, string.Empty); // Null ise kısıtlama yok demektir

            var startDate = (DateTime)value;
            var isValid = DateTime.UtcNow >= startDate;
            var message = isValid ? string.Empty : "Bu eğitim henüz erişime açılmamıştır.";
            return (isValid, message);
        }
    }
}
