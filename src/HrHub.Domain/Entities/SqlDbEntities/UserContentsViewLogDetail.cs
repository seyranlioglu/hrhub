using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class UserContentsViewLogDetail  : CardEntity<long>
    {
        public long UserContentsViewLogId { get; set; }
        public bool IsActive { get; set; }
        public int PartNumber { get; set; }
        public DateTime LogDate { get; set; }

        [ForeignKey("UserContentsViewLogId")]
        public virtual UserContentsViewLog UserContentsViewLog { get; set; }
    }
}
