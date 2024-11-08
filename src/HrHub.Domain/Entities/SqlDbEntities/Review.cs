using HrHub.Core.Domain.Entity;

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
    }

}
