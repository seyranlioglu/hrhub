namespace HrHub.Abstraction.Exceptions
{
    public class DatabaseException : ExceptionBase
    {
        public DatabaseException()
        {
        }

        public DatabaseException(string message)
            : base(message)
        {
        }

        public DatabaseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public DatabaseException(int errorCode, string message)
            : base(errorCode, message)
        {
        }

        public DatabaseException(string module, int errorCode, string message, Exception innerException)
            : base(module, errorCode, message, innerException)
        {
        }

        public DatabaseException(string module, int errorCode, string message, params object[] args)
            : base(module, errorCode, message)
        {
            Args = args?.ToList();
        }

        public List<object> Args { get; set; }
    }
}
