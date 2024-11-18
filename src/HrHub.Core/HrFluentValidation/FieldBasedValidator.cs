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
    public class FieldBasedValidator<T> : AbstractValidator<T>
    {
        public FieldBasedValidator()
        {
            var properties = typeof(T).GetProperties()
                                      .Where(p => p.IsDefined(typeof(ValidationRulesAttribute), false));

            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<ValidationRulesAttribute>();

                RuleFor(instance => property.GetValue(instance))
                    .Custom((value, context) =>
                    {
                        foreach (var ruleType in attribute.RuleTypes)
                        {
                            if (Activator.CreateInstance(ruleType) is IBusinessRule rule)
                            {
                                var (isValid, errorMessage) = rule.Validate(value, property.Name);
                                if (!isValid)
                                {
                                    context.AddFailure(errorMessage);
                                }
                            }
                        }
                    });
            }
        }
    }
}
