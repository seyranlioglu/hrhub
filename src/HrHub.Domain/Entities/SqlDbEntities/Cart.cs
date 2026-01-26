using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class Cart : CardEntity<long>
    {
        public Cart()
        {
            CartItems = new HashSet<CartItem>();
        }

        public long CurrAccId { get; set; }
        public string PromotionCode { get; set; }
        public decimal TotalAmount { get; set; }
        public long StatusId { get; set; }
        [AllowNull]
        public DateTime? ConfirmDate { get; set; }
        [AllowNull]
        public long? ConfirmUserId { get; set; }
        [AllowNull]
        public string? ConfirmNotes { get; set; }
        public long AddCartUserId { get; set; }

        [ForeignKey("StatusId")]
        public virtual CartStatus CartStatus { get; set; }

        [ForeignKey("ConfirmUserId")]
        public virtual User ConfirmUser { get; set; }

        [ForeignKey("AddCartUserId")]
        public virtual User AddCartUser { get; set; }

        [ForeignKey("CurrAccId")]
        public virtual CurrAcc CurrAcc { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}
