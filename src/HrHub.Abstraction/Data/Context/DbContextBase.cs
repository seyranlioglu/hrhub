using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Abstraction.Data.Context
{
    public class DbContextBase : DbContext
    {
        public DbContextBase([NotNull] DbContextOptions options) : base(options)
        {

        }

        protected DbContextBase()
        {
        }
    }
}
