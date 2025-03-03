using HrHub.Abstraction.Attributes;
using HrHub.Core.BusinessRules;

namespace HrHub.Domain.Contracts.Dtos.TrainingAnnouncementDtos
{
    public class UpdateTrainingAnnouncementDto
    {
        [ValidationRules(typeof(ZeroCheckRule))]
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
