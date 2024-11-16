using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Abstraction.Domain
{
    public interface IBaseTypeEntity : IBaseEntity
    {
        public long Id { get; set; }
        public long? CreateUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public long? UpdateUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public long? DeleteUserId { get; set; }
        public DateTime? DeleteDate { get; set; }
        public bool? IsDelete { get; set; }
        public bool IsActive { get; set; }
        public string? Title { get; set; }
        public string? Abbreviation { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
    }
}
