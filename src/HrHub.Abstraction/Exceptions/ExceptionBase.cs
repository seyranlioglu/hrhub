namespace HrHub.Abstraction.Exceptions
{
    public class ExceptionBase : Exception
    {
        public ExceptionBase()
        {
        }

        public ExceptionBase(int errorCode, string message)
         : this(message)
        {
            ErrorCode = errorCode;
        }

        public ExceptionBase(string module, int errorCode, string message)
            : this(message)
        {
            Module = module;
            ErrorCode = errorCode;
        }

        public ExceptionBase(string module, int errorCode, string message, Exception innerException)
            : this(message, innerException)
        {
            Module = module;
            ErrorCode = errorCode;
        }

        public ExceptionBase(string message)
            : base(message)
        {
        }

        public ExceptionBase(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public string Module { get; set; }

        public int ErrorCode { get; set; }
    }
}
