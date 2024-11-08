using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class ContentComment : TypeCardEntity<int>
    {
        public int ContentId { get; set; }
        public bool Pinned { get; set; }
        public int UserId { get; set; }
        public DateTime ContentDate { get; set; }
        public int MasterContentId { get; set; }
    }
}
