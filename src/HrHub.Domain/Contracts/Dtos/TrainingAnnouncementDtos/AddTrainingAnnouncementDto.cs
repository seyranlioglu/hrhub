using HrHub.Abstraction.Attributes;
using HrHub.Core.BusinessRules;

namespace HrHub.Domain.Contracts.Dtos.TrainingAnnouncementDtos
{
    public class AddTrainingAnnouncementDto
    {
        [ValidationRules(typeof(ZeroCheckRule))]
        public long TrainingId { get; set; }
        [ValidationRules(typeof(ZeroCheckRule))]
        public long UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
