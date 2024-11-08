using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class Instructor : TypeCardEntity<int>
    {
        public int UserId { get; set; }
        public string PicturePath { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Facebook { get; set; }
        public string Linkedin { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public string Title { get; set; }
        public int InstructorTypeId { get; set; }
    }

}
