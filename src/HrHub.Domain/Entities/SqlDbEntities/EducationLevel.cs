using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class EducationLevel : TypeCardEntity<long>
    {
        public EducationLevel()
        {
            Trainings = new HashSet<Training>();
        }
        public virtual ICollection<Training> Trainings { get; set; }
    }
}
