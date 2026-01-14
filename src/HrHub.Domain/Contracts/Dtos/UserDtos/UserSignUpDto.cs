using HrHub.Abstraction.Attributes;
using HrHub.Core.BusinessRules;

namespace HrHub.Domain.Contracts.Dtos.UserDtos
{
    public class UserSignUpDto
    {
        [ValidationRules(typeof(NullCheckRule))]
        public string? Email { get; set; }

        [ValidationRules(typeof(NullCheckRule))]
        public string? PhoneNumber { get; set; }

        // YENİ: Şifre alanı eklendi ve zorunlu yapıldı
        [ValidationRules(typeof(NullCheckRule))]
        public string Password { get; set; }

        // Şahıs bilgileri zorunlu
        [ValidationRules(typeof(NullCheckRule))]
        public string? Name { get; set; }

        [ValidationRules(typeof(NullCheckRule))]
        public string? Surname { get; set; }

        // Kurumsal Bilgiler (Artık zorunlu oldukları için kural ekledik)
        [ValidationRules(typeof(NullCheckRule))]
        public string? Title { get; set; }

        [ValidationRules(typeof(NullCheckRule))]
        public string? Address { get; set; }

        [ValidationRules(typeof(NullCheckRule))]
        public string? TaxNumber { get; set; }

        [ValidationRules(typeof(NullCheckRule))]
        public string? PostalCode { get; set; }

        // TC Kimlik No kurumsal için zorunlu değil, boş geçebiliriz.
        public string? IdentityNumber { get; set; }

        public string CurrAccTypeCode { get; set; } = "K";
    }
}