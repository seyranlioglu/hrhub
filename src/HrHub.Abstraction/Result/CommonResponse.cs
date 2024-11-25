using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Abstraction.Result
{
    public class CommonResponse
    {
        public string Message { get; set; }
        public int Code { get; set; }
        public bool Result { get; set; }
    }
}
