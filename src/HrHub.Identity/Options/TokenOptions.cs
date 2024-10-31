using HrHub.Abstraction.Attributes;
using HrHub.Abstraction.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Identity.Options
{
    [AppSetting("TokenOptions")]
    public class AsisTokenOptions : ISettingsBase
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int AccessTokenExpiration { get; set; }
        public int RefreshTokenExpiration { get; set; }
        public string SecurityKey { get; set; }
    }
}
