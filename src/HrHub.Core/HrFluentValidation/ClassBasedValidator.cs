using FluentValidation;
using HrHub.Abstraction.Attributes;
using HrHub.Abstraction.BusinessRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Core.HrFluentValidation
{
    public class ClassBasedValidator<T> : AbstractValidator<T> where T : class
    {
        public ClassBasedValidator()
        {
        }

        public FluentValidation.Results.ValidationResult Validate(T instance, params Type[] ruleTypes)
        {
            var context = new ValidationContext<T>(instance);

            foreach (var ruleType in ruleTypes)
            {
                if (Activator.CreateInstance(ruleType) is IBusinessRule rule)
                {
                    var (isValid, errorMessage) = rule.Validate(instance, nameof(T));
                    if (!isValid)
                    {
                        context.AddFailure(errorMessage);
                    }
                }
            }

            // Return the validation result
            return base.Validate(context);
        }
    }
}
