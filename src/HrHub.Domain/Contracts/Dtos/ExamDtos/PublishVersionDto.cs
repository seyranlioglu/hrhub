using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Dtos.ExamDtos
{
    public class PublishVersionDto
    {
        public long ExamId { get; set; }
        public int VersionNumber { get; set; }
        public long? VersionId { get; set; }
    }
}
