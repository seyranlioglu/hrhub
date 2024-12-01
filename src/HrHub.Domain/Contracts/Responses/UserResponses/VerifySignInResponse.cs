namespace HrHub.Domain.Contracts.Responses.UserResponses
{
    public class VerifySignInResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
        public long Id { get; set; }
        public long CurrAccId { get; set; }
        public string Name { get; set; }
        public string  SurName { get; set; }
        public string UserShortName { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
