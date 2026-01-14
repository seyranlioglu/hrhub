using System.Collections.Generic;

namespace HrHub.Domain.Contracts.Dtos.MenuDtos
{
    public class UserMenuResponseDto
    {
        public bool IsInstructor { get; set; } // Ön yüzdeki "Eğitmen Paneli" butonu için
        public List<UserMenuDto> MenuItems { get; set; } = new List<UserMenuDto>(); // Sol menü ağacı
    }
    public class UserMenuDto
    {
        public string Title { get; set; }
        public string Path { get; set; }
        public string Icon { get; set; }
        public List<UserMenuDto> Children { get; set; } = new List<UserMenuDto>();
    }
}