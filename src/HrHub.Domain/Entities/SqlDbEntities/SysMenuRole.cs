using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using HrHub.Identity.Entities; // AppRole için

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class SysMenuRole : CardEntity<long>
    {
        public long SysMenuId { get; set; }
        public long RoleId { get; set; } // Identity'deki AppRole Id'si

        [ForeignKey("SysMenuId")]
        public virtual SysMenu SysMenu { get; set; }

        [ForeignKey("RoleId")]
        public virtual AspNetRole Role { get; set; }
    }
}