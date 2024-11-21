using HrHub.Abstraction.BusinessRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Abstraction.Attributes
{
    public class ValidationRulesAttribute : Attribute
    {
        public Type[] RuleTypes { get; }

        public ValidationRulesAttribute(params Type[] ruleTypes)
        {
            RuleTypes = ruleTypes;
        }
    }
}
