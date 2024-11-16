using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class InstructorType : TypeCardEntity<long>
    {
        public InstructorType()
        {
            Instructors = new HashSet<Instructor>();
        }
        public string? LangCode { get; set; }


        public ICollection<Instructor> Instructors { get; set; }
    }
}
