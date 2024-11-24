using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HrHub.Abstraction.Enums;

namespace HrHub.Abstraction.Attributes
{
    public class LifeCycleAttribute : Attribute
    {
        public LifeCycleAttribute(LifeCycleTypes LifeCycleTypes)
        {
            this.LifeCycleTypes = LifeCycleTypes;
        }
        public LifeCycleTypes LifeCycleTypes { get; set; }
    }
}
