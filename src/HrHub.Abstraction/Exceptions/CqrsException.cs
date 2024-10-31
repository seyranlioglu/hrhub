using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Abstraction.Exceptions
{
    public class CqrsException : ExceptionBase
    {
        public CqrsException()
        {
        }

        public CqrsException(string message)
            : base(message)
        {
        }

        public CqrsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public CqrsException(int errorCode, string message)
            : base(errorCode, message)
        {
        }

        public CqrsException(string module, int errorCode, string message, Exception innerException)
            : base(module, errorCode, message, innerException)
        {
        }

        public CqrsException(string module, int errorCode, string message, params object[] args)
            : base(module, errorCode, message)
        {
            Args = args?.ToList();
        }

        public List<object> Args { get; set; }
    }
}
