using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Dtos.DashboardDtos
{
    public class ContinueTrainingDto
    {
        public long TrainingId { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public int Progress { get; set; }
        public string LastLessonName { get; set; }
    }
}
