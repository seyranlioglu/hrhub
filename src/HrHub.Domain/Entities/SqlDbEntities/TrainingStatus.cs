using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class TrainingStatus : TypeCardEntity<long>
    {
        public TrainingStatus()
        {
            Trainings = new HashSet<Training>();
        }
        public string? LangCode { get; set; }


        public virtual ICollection<Training> Trainings { get; set; } 
    }
}
