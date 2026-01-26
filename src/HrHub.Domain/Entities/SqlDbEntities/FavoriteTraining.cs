using HrHub.Core.Base;
using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    [Table("FavoriteTrainings")]
    public class FavoriteTraining : CardEntity<long>
    {
        public long TrainingId { get; set; }
        public long UserId { get; set; }

        // Navigation Properties
        [ForeignKey("TrainingId")]
        public virtual Training Training { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}