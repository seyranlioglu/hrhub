using HrHub.Abstraction.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Abstraction.Settings
{
    [AppSetting("IntegrationServices")]
    public class HttpClientConfiguration : ISettingsBase
    {
        public List<HttpClientSettings> HttpClients { get; set; }
    }
}
