namespace HrHub.Domain.Contracts.Responses.UserResponses
{
    public class UserSignUpResponse
    {
        public bool Result { get; set; }
        public string Message { get; set; }
        public string Email { get; set; }
        public string  PhoneNumber { get; set; }
    }
}
