using HrHub.Core.Domain.Entity;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class PasswordHistory : CardEntity<long>
    {
        public long UserId { get; set; }
        public string Password { get; set; }
        public string ChangeReason { get; set; }
        public bool IsSuccess { get; set; }
        public DateTime PasswordChangeDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool PassForgotting { get; set; }
    }
}
