using HrHub.Identity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Identity.Abstraction
{
    public interface ITokenHandler
    {
        AccessToken CreateAccessToken(TokenModel user);
    }
}
