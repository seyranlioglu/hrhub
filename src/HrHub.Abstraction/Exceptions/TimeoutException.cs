using HrHub.Abstraction.StatusCodes;

namespace HrHub.Abstraction.Exceptions
{
    public class TimeoutException : ExceptionBase
    {
        public TimeoutException() : base(HrStatusCodes.Status408TimeOut, "Timeout")
        {
        }

        public TimeoutException(string message)
            : base(message)
        {
        }

        public TimeoutException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public TimeoutException(int errorCode, string message)
            : base(errorCode, message)
        {
        }

        public TimeoutException(string module, int errorCode, string message, Exception innerException)
            : base(module, errorCode, message, innerException)
        {
        }

        public TimeoutException(string module, int errorCode, string message, params object[] args)
            : base(module, errorCode, message)
        {
            Args = args?.ToList();
        }

        public List<object> Args { get; set; }
    }
}
