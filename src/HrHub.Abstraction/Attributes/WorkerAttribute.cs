using Hangfire.Server;
using HrHub.Abstraction.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Abstraction.Attributes
{
    public class WorkerAttribute : Attribute
    {
        public Workers Worker { get; set; }
        public WorkerAttribute(Workers worker)
        {
            Worker = worker;
        }
    }
}
