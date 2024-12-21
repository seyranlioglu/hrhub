using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Dtos.TrainingCategoryDtos
{
    public class AddTrainingCategoryDto
    {
        public long? MasterCategoryId { get; set; } = null;
        public string? Title { get; set; }
        public string? Abbreviation { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
    }
}
