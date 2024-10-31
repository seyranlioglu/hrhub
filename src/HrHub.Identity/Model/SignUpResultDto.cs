using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Identity.Model
{
    public class SignUpResultDto
    {
        public bool Result { get; set; } = false;
        public string ConfirmToken { get; set; }
    }
}
