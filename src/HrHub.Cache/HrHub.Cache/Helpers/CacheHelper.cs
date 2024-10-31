using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Cache.Helpers
{
    public static class CacheHelper
    {
        public static string GetHost(string ipAddress, string port)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(ipAddress);
            stringBuilder.Append(":");
            stringBuilder.Append(port);

            return stringBuilder.ToString();
        }
    }
}
