using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class ContentNotes : CardEntity<int>
    {
        public long ContentId { get; set; }
        public DateTime? NoteTime { get; set; }
        public string Note { get; set; }
        public long UserId { get; set; }
    }
}
