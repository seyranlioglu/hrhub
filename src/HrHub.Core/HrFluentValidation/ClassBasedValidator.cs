using FluentValidation;
using HrHub.Abstraction.BusinessRules;
using Microsoft.Extensions.DependencyInjection;

namespace HrHub.Core.HrFluentValidation
{
    public class ClassBasedValidator<T> : AbstractValidator<T> where T : class
    {
        private readonly IServiceProvider _serviceProvider;

        public ClassBasedValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public FluentValidation.Results.ValidationResult Validate(T instance, params Type[] ruleTypes)
        {
            var context = new ValidationContext<T>(instance);

            foreach (var ruleType in ruleTypes)
            {
                // Resolve the registered service for the given interface
                var rule = _serviceProvider.GetService(ruleType);

                if (rule is not IBusinessRule businessRule)
                {
                    throw new InvalidOperationException($"The resolved type '{ruleType.Name}' does not implement IBusinessRule.");
                }

                // Validate the instance using the resolved business rule
                var (isValid, errorMessage) = businessRule.Validate(instance, nameof(T));
                if (!isValid)
                {
                    context.AddFailure(errorMessage);
                }
                //// Get the constructor of the ruleType
                //var constructor = ruleType.GetConstructors().FirstOrDefault();
                //if (constructor == null)
                //    throw new InvalidOperationException($"No public constructor found for type '{ruleType.Name}'.");

                //// Resolve parameters dynamically
                //var parameters = constructor.GetParameters();
                //var paramsInstances = parameters.Select(p => _serviceProvider.GetService(p.ParameterType)).ToArray();

                //// Create an instance of the rule
                //var ruleInstance = Activator.CreateInstance(ruleType, parameters) as IBusinessRule;
                //if (ruleInstance == null)
                //    throw new InvalidOperationException($"The type '{ruleType.Name}' does not implement IBusinessRule.");

                //// Validate the instance using the resolved rule
                //var (isValid, errorMessage) = ruleInstance.Validate(instance, nameof(T));
                //if (!isValid)
                //{
                //    context.AddFailure(errorMessage);
                //}
            }

            // Return the validation result
            return base.Validate(context);
        }
    }
}
