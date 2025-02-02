using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class Instructor : TypeCardEntity<long>
    {
        public Instructor()
        {
            Trainings = new HashSet<Training>();
        }
        public long UserId { get; set; }
        public string PicturePath { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Facebook { get; set; }
        public string Linkedin { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public string Title { get; set; }
        public long InstructorTypeId { get; set; }
        public string InstructorCode { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("InstructorTypeId")]
        public virtual InstructorType InstructorType { get; set; }
        public virtual ICollection<Training> Trainings { get; set; }

    }

}
