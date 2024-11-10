using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class Review : CardEntity<int> 
    {
        public long TrainingId { get; set; }
        public long InstructorPoint { get; set; }
        public long TrainingPoint { get; set; }
        public string Status { get; set; }
        public string InstructorReview { get; set; }
        public string TrainingReview { get; set; }
        public long CommentedUserId { get; set; }

        [ForeignKey("TrainingId")]
        public virtual Training Training { get; set; }

        [ForeignKey("CommentedUserId")]
        public virtual User User { get; set; }
    }

}
