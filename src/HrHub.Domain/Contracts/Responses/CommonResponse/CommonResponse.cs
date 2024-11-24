namespace HrHub.Domain.Contracts.Responses.CommonResponse
{
    public class CommonResponse
    {
        public string Message { get; set; }
        public int Code { get; set; }
        public bool Result { get; set; } = false;
    }
}
