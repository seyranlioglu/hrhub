using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class TrainingLanguage : TypeCardEntity<long>
    {

        public TrainingLanguage()
        {
             Trainings = new HashSet<Training>();
        }

        public virtual ICollection<Training> Trainings{ get; set; } = null;
    }

}
