using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class TrainingLevel : TypeCardEntity<int>
    {
        public int Priority { get; set; }
        public string Color { get; set; }
    }
}
