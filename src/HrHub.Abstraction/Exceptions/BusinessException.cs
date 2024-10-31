namespace HrHub.Abstraction.Exceptions
{
    public class BusinessException : ExceptionBase
    {
        public BusinessException()
        {
        }

        public BusinessException(string message)
            : base(message)
        {
        }

        public BusinessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public BusinessException(int errorCode, string message)
            : base(errorCode, message)
        {
        }

        public BusinessException(string module, int errorCode, string message, Exception innerException)
            : base(module, errorCode, message, innerException)
        {
        }

        public BusinessException(string module, int errorCode, string message, params object[] args)
            : base(module, errorCode, message)
        {
            Args = args?.ToList();
        }

        public List<object> Args { get; set; }
    }
}
