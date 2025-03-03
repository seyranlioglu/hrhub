using HrHub.Abstraction.Attributes;
using HrHub.Core.BusinessRules;

namespace HrHub.Domain.Contracts.Dtos.TrainingAnnouncementCommentDtos
{
    public class UpdateTrainingAnnouncementCommentDto
    {
        [ValidationRules(typeof(ZeroCheckRule))]
        public long Id { get; set; }
        public string Description { get; set; }
    }
}
