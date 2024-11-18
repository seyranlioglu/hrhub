using HrHub.Abstraction.Attributes;
using HrHub.Core.Data.UnitOfWork;
using HrHub.Domain.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Infrastructre.UnitOfWorks
{
    [LifeCycle(Abstraction.Enums.LifeCycleTypes.Scoped)]
    public class HrUnitOfWork : UnitOfWork<HrHubDbContext>, IHrUnitOfWork
    {
        public HrUnitOfWork(HrHubDbContext context) : base(context)
        {
        }
    }
}
