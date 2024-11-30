using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Identity.Model
{
    public class TokenModel
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Channel { get; set; }
        public int ExpirationTime { get; set; }
        public string SecurityKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public List<string> Roles { get; set; }
        public bool IsMainUser { get; set; }
        public long CurrAccId { get; set; }

    }
}
