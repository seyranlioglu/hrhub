using HrHub.Abstraction.Attributes;
using HrHub.Core.BusinessRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Dtos.TrainingDtos
{
    public class TrainingAccessCheckDto
    {
        [ValidationRules(typeof(TrainingNotStartedRule))]
        public DateTime? StartDate { get; set; }

        [ValidationRules(typeof(TrainingExpiredRule))]
        public DateTime? DueDate { get; set; }
    }
}
