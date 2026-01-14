using HrHub.Core.Domain.Entity;
using HrHub.Domain.Contracts.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class TrainingProcessRequest : CardEntity<long>
    {
        public long TrainingId { get; set; }
        public TrainingRequestType RequestType { get; set; } // DB'de int tutulur

        // Talebin Durumu (Beklemede, Onaylandı, Reddedildi)
        public long RequestStatusId { get; set; }

        // İşlem Onaylanırsa Eğitimin Geçeceği Yeni Statü (Opsiyonel)
        public long? TargetStatusId { get; set; }

        public long RequesterUserId { get; set; }
        public long? ResponderUserId { get; set; }
        public long? CurrAccTrainingUserId { get; set; } // Süre uzatma için kritik

        public string? Note { get; set; }
        public DateTime? ResponseDate { get; set; }

        // --- Relations ---
        [ForeignKey(nameof(TrainingId))]
        public virtual Training Training { get; set; }

        [ForeignKey(nameof(RequestStatusId))]
        public virtual TrainingStatus RequestStatus { get; set; }

        [ForeignKey(nameof(TargetStatusId))]
        public virtual TrainingStatus? TargetStatus { get; set; }

        [ForeignKey(nameof(RequesterUserId))]
        public virtual User RequesterUser { get; set; }

        [ForeignKey(nameof(CurrAccTrainingUserId))]
        public virtual CurrAccTrainingUser CurrAccTrainingUser { get; set; }
    }
}