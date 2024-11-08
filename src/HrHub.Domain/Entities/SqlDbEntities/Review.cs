using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class Review : CardEntity<int> 
    {
        public int TrainingId { get; set; }
        public int InstructorPoint { get; set; }
        public int TrainingPoint { get; set; }
        public string Status { get; set; }
        public string InstructorReview { get; set; }
        public string TrainingReview { get; set; }
        public int CommentedUserId { get; set; }
    }

}
