using HrHub.Abstraction.Attributes;
using HrHub.Core.BusinessRules;

namespace HrHub.Domain.Contracts.Dtos.TrainingAnnouncementCommentDtos
{
    public class AddTrainingAnnouncementCommentDto
    {
        [ValidationRules(typeof(ZeroCheckRule))]
        public long TrainingAnnouncementsId { get; set; }
        [ValidationRules(typeof(ZeroCheckRule))]
        public long UserId { get; set; }
        public string Description { get; set; }
    }
}
