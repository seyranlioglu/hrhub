namespace HrHub.Abstraction.Exceptions
{
    public class AuthenticationException : ExceptionBase
    {
        public AuthenticationException()
        {
        }

        public AuthenticationException(string message)
            : base(message)
        {
        }

        public AuthenticationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public AuthenticationException(int errorCode, string message)
            : base(errorCode, message)
        {
        }

        public AuthenticationException(string module, int errorCode, string message, Exception innerException)
            : base(module, errorCode, message, innerException)
        {
        }

        public AuthenticationException(string module, int errorCode, string message, params object[] args)
            : base(module, errorCode, message)
        {
            Args = args?.ToList();
        }

        public List<object> Args { get; set; }
    }
}
