using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class UserContentsViewLog: CardEntity<long>
    {
        public long ContentId { get; set; }
        public long CurrAccTrainingUserId { get; set; }
        public bool IsActive { get; set; }
        public bool State { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime CompletedDate { get; set; }
    }
}
