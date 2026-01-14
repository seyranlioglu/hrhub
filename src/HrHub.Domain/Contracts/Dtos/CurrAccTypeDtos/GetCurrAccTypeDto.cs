using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Dtos.CurrAccTypeDtos
{
    public class GetCurrAccTypeDto
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public bool EnterpriseAcc { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
