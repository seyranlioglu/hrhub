using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Dtos.DashboardDtos
{
    public class DashboardStatsDto
    {
        public int CompletedTrainingsCount { get; set; } // Tamamlanan Eğitim Sayısı
        public int InProgressTrainingsCount { get; set; } // Devam Eden Eğitim Sayısı
        public int TotalCertificates { get; set; } // Kazanılan Sertifikalar
        // Eğer puan sistemi varsa: public int TotalPoints { get; set; } 
    }
}
