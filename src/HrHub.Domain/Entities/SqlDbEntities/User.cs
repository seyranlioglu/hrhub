using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class User : CardEntity<int>
    {
        public User()
        {
            ConfirmCarts = new HashSet<Cart>();
            CommentVotes = new HashSet<CommentVote>();
            ContentComments = new HashSet<ContentComment>(); 
            AddCarts = new HashSet<Cart>();
        }

        public virtual ICollection<Cart> ConfirmCarts { get; set; } = null;
        public virtual ICollection<Cart> AddCarts { get; set; } = null;
        public virtual ICollection<CommentVote> CommentVotes { get; set; } = null;
        public virtual ICollection<ContentComment> ContentComments { get; set; } = null;


        public long CurrAccId { get; set; }
    }
}
