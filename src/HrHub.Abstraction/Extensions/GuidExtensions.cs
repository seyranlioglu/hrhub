using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Abstraction.Extensions
{
    public static class GuidExtensions
    {
        /// <summary>
        /// GUID gibi dataların içindeki - işaretini temizler.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string TrimHyphen(this Guid id)
        {
            return id.ToString().Replace("-", "");
        }
    }
}
