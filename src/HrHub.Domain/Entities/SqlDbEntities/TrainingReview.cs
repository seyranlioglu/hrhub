using HrHub.Core.Base;
using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    [Table("TrainingReviews")]
    public class TrainingReview : CardEntity<long>
    {
        public long TrainingId { get; set; }
        public long UserId { get; set; }

        public int Rating { get; set; } // 1-5 Yıldız
        public string Comment { get; set; }
        public bool IsApproved { get; set; } = true;

        // Navigation Properties
        [ForeignKey("TrainingId")]
        public virtual Training Training { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}