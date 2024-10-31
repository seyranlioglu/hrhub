using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HrHub.Abstraction.Enums;

namespace HrHub.Abstraction.Attributes
{
    public class LifeCircleAttribute : Attribute
    {
        public LifeCircleAttribute(LifeCircleTypes LifeCircleTypes)
        {
            this.LifeCircleTypes = LifeCircleTypes;
        }
        public LifeCircleTypes LifeCircleTypes { get; set; }
    }
}
