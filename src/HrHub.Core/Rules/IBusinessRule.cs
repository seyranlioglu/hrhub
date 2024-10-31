using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Core.Rules
{
    public interface IBusinessRule
    {
        int ErrorCode { get; }
        string Message { get; }
        List<object> Args { get; }
        bool IsBroken();
    }
}
