using HrHub.Abstraction.Data.EfCore.UnitOfwork;
using HrHub.Domain.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Infrastructre.UnitOfWorks
{
    public interface IHrUnitOfWork : IUnitOfWork<HrHubDbContext>
    {
    }
}
