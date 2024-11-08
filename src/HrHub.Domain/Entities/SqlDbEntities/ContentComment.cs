using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class ContentComment : TypeCardEntity<long>
    {
        public long ContentId { get; set; }
        public bool Pinned { get; set; }
        public long UserId { get; set; }
        public DateTime ContentDate { get; set; }
        public long MasterContentId { get; set; }
    }
}
