using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class CurrAcc : CardEntity<int>
    {
        public CurrAcc()
        {
            Carts = new HashSet<Cart>();
        }

        public virtual ICollection<Cart> Carts { get; set; } = null;

        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public string TaxNumber { get; set; }
        public string IdentityNumber { get; set; }
        public string PostalCode { get; set; }
    }
}
