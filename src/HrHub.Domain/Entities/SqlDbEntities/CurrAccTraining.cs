using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class CurrAccTraining : CardEntity<int>
    {
        public int CurrAccId { get; set; }
        public int TrainingId { get; set; }
        public int StatusId { get; set; }
        public DateTime ConfirmDate { get; set; }
        public int ConfirmUserId { get; set; }
        public string ConfirmNotes { get; set; }
        public int LicenceCount { get; set; }
        public int CartItemId { get; set; }


        [ForeignKey("CurrAccId")]
        public virtual CurrAcc CurrAcc { get; set; }

        [ForeignKey("TrainingId")]
        public virtual Training Training { get; set; }

        [ForeignKey("StatusId")]
        public virtual CurrAccTrainingStatu CurrAccTrainingStatus { get; set; }

        [ForeignKey("ConfirmUserId")]
        public virtual User User { get; set; }
    }
}
