using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class TrainingCategory : TypeCardEntity<long>
    {
        public TrainingCategory()
        {
            SubTrainingCategories = new HashSet<TrainingCategory>();
        }

        public long? MasterCategoryId { get; set; }

        [ForeignKey("MasterCategoryId")]
        public virtual TrainingCategory? MasterTrainingCategory { get; set; }

        public ICollection<TrainingCategory> SubTrainingCategories { get; set; } = null;
    }
}
