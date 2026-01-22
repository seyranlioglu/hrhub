namespace HrHub.Domain.Contracts.Dtos.UserDtos
{
    public class InviteUserDto
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool ForceTransfer { get; set; } = false; // Varsayılan false
    }
}