using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Dtos.TrainingContentDtos
{
    public class LogWatchProgressDto
    {
        // Hangi içerik izleniyor?
        public long TrainingContentId { get; set; }

        // Hangi parça izlendi? (Örn: 0. parça=0-15sn, 1. parça=15-30sn)
        public int PartNumber { get; set; }

        // Bu son parça mı? (Video bitti mi?)
        public bool IsLastPart { get; set; }
    }
}
