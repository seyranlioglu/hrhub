using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class TrainingLevel : TypeCardEntity<long>
    {
        public TrainingLevel()
        {
            TrainingLevels = new HashSet<TrainingLevel>();
        }
        public long Priority { get; set; }
        public string Color { get; set; }

        public virtual ICollection<TrainingLevel> TrainingLevels { get; set; }
    }
}
