using HrHub.Core.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrHub.Domain.Entities.SqlDbEntities
{
    public class SysMenuPolicy : CardEntity<long>
    {
        public long SysMenuId { get; set; }

        // Örn: "Instructor", "MainUser" gibi Policy isimleri buraya yazılacak
        public string PolicyName { get; set; }

        [ForeignKey("SysMenuId")]
        public virtual SysMenu SysMenu { get; set; }
    }
}