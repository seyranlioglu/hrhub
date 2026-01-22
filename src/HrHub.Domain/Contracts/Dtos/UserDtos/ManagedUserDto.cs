namespace HrHub.Domain.Contracts.Dtos.UserDtos
{
    public class ManagedUserDto
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; } // Eğitmene maskelenebilir
        public string PhoneNumber { get; set; } // Eğitmene gitmez
        public string Title { get; set; } // Unvan (Sadece MainUser/Admin)
        public string CompanyName { get; set; } // Sadece Admin
        public bool IsActive { get; set; }
        public string Status { get; set; } // "Aktif", "Pasif", "Transfer Bekliyor"
        public DateTime CreatedDate { get; set; }

        // UI'da "Hangi moddayım?" bilgisini göstermek için
        public string ViewMode { get; set; } // "AdminView", "CompanyView", "InstructorView"
    }
}
