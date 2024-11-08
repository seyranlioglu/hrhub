using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class ContentNotes : CardEntity<int>
    {
        public int ContentId { get; set; }
        public DateTime? NoteTime { get; set; }
        public string Note { get; set; }
        public int UserId { get; set; }
    }
}
