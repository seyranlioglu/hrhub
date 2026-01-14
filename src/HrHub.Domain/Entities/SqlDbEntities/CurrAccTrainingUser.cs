using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class CurrAccTrainingUser : CardEntity<long>
    {
        public CurrAccTrainingUser()
        {
            UserCertificates = new HashSet<UserCertificate>();
            UserExams = new HashSet<UserExam>();
        }
        public long CurrAccTrainingsId { get; set; }

        public long UserId { get; set; }
        public bool IsActive { get; set; }
        public bool QualifiedCertificate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("CurrAccTrainingsId")]
        public virtual CurrAccTraining CurrAccTrainings { get; set; }

        public virtual ICollection<UserCertificate> UserCertificates { get; set; } = null;
        public virtual ICollection<UserExam> UserExams { get; set; } = null;
    }
}
