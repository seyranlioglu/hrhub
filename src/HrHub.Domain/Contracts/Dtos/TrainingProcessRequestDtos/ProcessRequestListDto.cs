using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Dtos.TrainingProcessRequestDtos
{
    public class ProcessRequestListDto
    {
        public long Id { get; set; }
        public string TrainingTitle { get; set; }
        public string RequesterFullName { get; set; }
        public string RequestTypeName { get; set; } // Enum string karşılığı
        public string StatusName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Note { get; set; }
    }
}
