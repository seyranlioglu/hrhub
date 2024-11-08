using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

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
        public DateTime ConfirmDate { get; set; }
        public long ConfirmUserId { get; set; }   
        public string ConfirmNotes { get; set; }


        [ForeignKey("StatusId")]
        public virtual CartStatuses CartStatus { get; set; }

        [ForeignKey("ConfirmUserId")]
        public virtual User ConfirmUser { get; set; }
        public long AddCartUserId { get; set; }

        [ForeignKey("AddCartUserId")]
        public virtual User AddCartUser { get; set; }

        [ForeignKey("CurrAccId")]
        public virtual CurrAcc CurrAcc { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}
