using FluentValidation.Results;
using HrHub.Core.HrFluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Core.Helpers
{
    public static class ValidationHelper
    {
        private static IServiceProvider serviceProvider;
        public static void ValidatorHelperConfigure(IServiceProvider _serviceProvider)
        {
            serviceProvider = _serviceProvider;
        }
        public static ValidationResult FieldBasedValidator<TData>(TData data)
        {
            var validator = new FieldBasedValidator<TData>();
            return validator.Validate(data);
        }

        public static ValidationResult RuleBasedValidator<TData>(TData data, params Type[] ruleTypes) where TData : class
        {
            var validator = new ClassBasedValidator<TData>(serviceProvider);
            return validator.Validate(data, ruleTypes);

        }
    }
}
