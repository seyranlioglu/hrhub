using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class TrainingSection : TypeCardEntity<int>
    {
        public int TrainingId { get; set; }
        public int RowNumber { get; set; }
        public string LangCode { get; set; }
    }

}
