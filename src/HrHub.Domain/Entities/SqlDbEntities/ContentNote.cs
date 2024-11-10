using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class ContentNote : CardEntity<int>
    {
        [ForeignKey("ContentId")]
        public long ContentId { get; set; }
        public DateTime? NoteTime { get; set; }
        public string Note { get; set; }

        [ForeignKey("UserId")]
        public long UserId { get; set; }

        public virtual User User { get; set; }
        public virtual TrainingContent TrainingContent { get; set; }
    }
}
