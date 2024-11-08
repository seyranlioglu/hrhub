using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class TrainingSection : TypeCardEntity<long>
    {
        public long TrainingId { get; set; }
        public long RowNumber { get; set; }
        public string LangCode { get; set; }
    }

}
