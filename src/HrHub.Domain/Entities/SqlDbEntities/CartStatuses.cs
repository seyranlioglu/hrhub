using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class CartStatuses : TypeCardEntity<int>
    {
        public CartStatuses()
        {
            Carts = new HashSet<Cart>();
        }

        public virtual ICollection<Cart> Carts { get; set; } = null;
    }
}
