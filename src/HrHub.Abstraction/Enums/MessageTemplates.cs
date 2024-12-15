using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Abstraction.Enums
{
    public enum MessageTemplates : int
    {
        NewUser = 1,
        RegisterOTP = 2,
        Login = 3,
        ForgotPassword = 4
    }
}
