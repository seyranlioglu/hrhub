using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Abstraction.Attributes
{
    public class AppSettingAttribute : Attribute
    {
        public AppSettingAttribute(string Path)
        {
            this.Path = Path;
        }
        public string Path { get; set; }
    }
}
