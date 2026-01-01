using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class CurrAccTraining : CardEntity<long>
    {
        public CurrAccTraining()
        {
            CurrAccTrainingUsers = new HashSet<CurrAccTrainingUser>();
        }
        public long CurrAccId { get; set; }
        public long TrainingId { get; set; }
        public long CurrAccTrainingStatusId { get; set; }
        public DateTime? ConfirmDate { get; set; }
        public long? ConfirmUserId { get; set; }
        public string? ConfirmNotes { get; set; }
        public int? LicenceCount { get; set; }
        public long? CartItemId { get; set; }


        [ForeignKey("CurrAccId")]
        public virtual CurrAcc CurrAcc { get; set; }

        [ForeignKey("TrainingId")]
        public virtual Training Training { get; set; }

        [ForeignKey("CurrAccTrainingStatusId")]
        public virtual CurrAccTrainingStatus CurrAccTrainingStatus { get; set; }

        [ForeignKey("ConfirmUserId")]
        public virtual User ConfirmUser { get; set; }

        public virtual ICollection<CurrAccTrainingUser> CurrAccTrainingUsers { get; set; } = null;
    }
}
