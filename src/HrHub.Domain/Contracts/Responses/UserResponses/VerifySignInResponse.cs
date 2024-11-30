namespace HrHub.Domain.Contracts.Responses.UserResponses
{
    public class VerifySignInResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}
