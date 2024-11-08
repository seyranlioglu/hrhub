using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class TrainingLevel : TypeCardEntity<long>
    {
        public long Priority { get; set; }
        public string Color { get; set; }
    }
}
