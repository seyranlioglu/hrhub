using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class CurrAccTrainingUser : CardEntity<int>
    {
        public CurrAccTrainingUser()
        {
            UserCertificates = new HashSet<UserCertificate>();
        }
        public long CurrAccTrainingsId { get; set; }
        public long UserId { get; set; }
        public bool IsActive { get; set; }
        public bool QualifiedCertificate { get; set; }


        public virtual ICollection<UserCertificate> UserCertificates { get; set; }
    }
}
