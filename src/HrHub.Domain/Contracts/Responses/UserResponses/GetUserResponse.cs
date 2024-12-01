namespace HrHub.Domain.Contracts.Responses.UserResponses
{
    public class GetUserResponse
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsMainUser { get; set; }
        public long CurrAccId { get; set; }
    }
}
