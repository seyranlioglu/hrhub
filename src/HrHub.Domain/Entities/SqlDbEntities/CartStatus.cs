using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class CartStatus : TypeCardEntity<long>
    {
        public CartStatus()
        {
            Carts = new HashSet<Cart>();
        }

        public virtual ICollection<Cart> Carts { get; set; } = null;
    }
}
