using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Application.Workers
{
    public class RecurringJobs
    {
        private readonly IServiceScopeFactory serviceProvider;

        public RecurringJobs(IServiceScopeFactory serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void AddRecurringJobs()
        { }
    }
}
