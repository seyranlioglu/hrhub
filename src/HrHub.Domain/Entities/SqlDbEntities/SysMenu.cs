using HrHub.Core.Domain.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class SysMenu : CardEntity<long> // Title, Description, IsActive, CreatedDate vb. buradan gelir
    {
        public SysMenu()
        {
            MenuRoles = new HashSet<SysMenuRole>();
            SubMenus = new HashSet<SysMenu>();
            MenuPolicies = new HashSet<SysMenuPolicy>();
        }
        public string Title { get; set; }
        public string Path { get; set; } // Örn: /dashboard/admin
        public string Icon { get; set; } // Örn: bx bx-home
        public long? ParentId { get; set; } // Alt menü mantığı için
        public int OrderNo { get; set; } // Sıralama

        [ForeignKey("ParentId")]
        public virtual SysMenu Parent { get; set; }
        public virtual ICollection<SysMenu> SubMenus { get; set; }
        public virtual ICollection<SysMenuRole> MenuRoles { get; set; }
        public virtual ICollection<SysMenuPolicy> MenuPolicies { get; set; }
    }
}