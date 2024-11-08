using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class UserContentsViewLogDetail  : CardEntity<long>
    {
        public long LogId { get; set; }
        public bool IsActive { get; set; }
        public int PartNumber { get; set; }
        public DateTime LogDate { get; set; }
    }
}
