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
            ValidateProperties(typeof(T));
        }

        private void ValidateProperties(Type type, string parentPath = "")
        {
            var properties = type.GetProperties()
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
                                    context.AddFailure($"{parentPath}{property.Name}: {errorMessage}");
                                }
                            }
                        }

                        // Eğer property bir complex type ise, recursive olarak tekrar işlem yap
                        if (value != null && !IsPrimitiveType(property.PropertyType))
                        {
                            ValidateNestedProperties(value, context, $"{parentPath}{property.Name}.");
                        }
                    });
            }
        }

        private void ValidateNestedProperties(object instance, ValidationContext<T> context, string parentPath)
        {
            var type = instance.GetType();
            var properties = type.GetProperties()
                                 .Where(p => p.IsDefined(typeof(ValidationRulesAttribute), false));

            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<ValidationRulesAttribute>();
                var value = property.GetValue(instance);

                foreach (var ruleType in attribute.RuleTypes)
                {
                    if (Activator.CreateInstance(ruleType) is IBusinessRule rule)
                    {
                        var (isValid, errorMessage) = rule.Validate(value, property.Name);
                        if (!isValid)
                        {
                            context.AddFailure($"{parentPath}{property.Name}: {errorMessage}");
                        }
                    }
                }

                // Eğer property bir complex type ise, recursive olarak tekrar işlem yap
                if (value != null && !IsPrimitiveType(property.PropertyType))
                {
                    ValidateNestedProperties(value, context, $"{parentPath}{property.Name}.");
                }
            }
        }

        private bool IsPrimitiveType(Type type)
        {
            // Primitive veya basit tipleri kontrol et
            return type.IsPrimitive || type.IsValueType || type == typeof(string);
        }
    }
}
