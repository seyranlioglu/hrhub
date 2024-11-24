using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class CurrAcc : CardEntity<long>
    {
        public CurrAcc()
        {
            Carts = new HashSet<Cart>();
            Users = new HashSet<User>();
            UserContentsViewLogs = new HashSet<UserContentsViewLog>();
            CurrAccTrainings = new HashSet<CurrAccTraining>();
        }


        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public string TaxNumber { get; set; }
        public string IdentityNumber { get; set; }
        public string PostalCode { get; set; }
        public long CurrAccTypeId { get; set; }
        [ForeignKey("CurrAccTypeId")]
        public virtual CurrAccType CurrAccType { get; set; }
        public virtual ICollection<Cart> Carts { get; set; } = null;
        public virtual ICollection<User> Users { get; set; } = null;
        public virtual ICollection<UserContentsViewLog> UserContentsViewLogs { get; set; } = null;
        public virtual ICollection<CurrAccTraining> CurrAccTrainings { get; set; } = null;
    }
}
