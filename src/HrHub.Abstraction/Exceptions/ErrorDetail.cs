namespace HrHub.Abstraction.Exceptions
{
    public sealed class ErrorDetail
    {
        public ErrorDetail(string code, string message, string fieldName)
        {
            Code = code;
            Message = message;
            FieldName = fieldName;
        }

        public string Code { get; }

        public string Message { get; }

        public string FieldName { get; }
    }
}
