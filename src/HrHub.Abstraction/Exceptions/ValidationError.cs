namespace HrHub.Abstraction.Exceptions
{
    public class ValidationErrors
    {
        public int ErrorCode { get; set; }

        public string Message { get; set; }

        public string FieldName { get; set; }
    }
}
