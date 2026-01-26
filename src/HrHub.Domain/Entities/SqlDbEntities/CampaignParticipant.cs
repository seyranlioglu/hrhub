using HrHub.Core.Domain.Entity;
using HrHub.Domain.Contracts.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    [Table("CampaignParticipants")]
    public class CampaignParticipant : CardEntity<long>
    {
        public long CampaignId { get; set; }

        // Hangi Eğitim? (Opsiyonel: Eğer null ise tüm eğitmen ürünleri olabilir)
        public long? TrainingId { get; set; }

        // Hangi Eğitmen?
        public long? InstructorId { get; set; }

        public ParticipationStatus Status { get; set; } = ParticipationStatus.Pending;
        public DateTime? ActionDate { get; set; } // Onay/Red tarihi

        // Navigation Properties
        [ForeignKey("CampaignId")]
        public virtual Campaign Campaign { get; set; }

        [ForeignKey("TrainingId")]
        public virtual Training? Training { get; set; }

        [ForeignKey("InstructorId")]
        public virtual Instructor? Instructor { get; set; }
    }
}
