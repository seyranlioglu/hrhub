using HrHub.Abstraction.Domain;
using System.ComponentModel.DataAnnotations;

namespace HrHub.Core.Domain.Entity
{
    public abstract class BaseEntity<TKey> : IBaseEntity
    {
        [Key]
        public TKey Id { get; set; }

        public BaseEntity()
        {
            if (typeof(TKey) == typeof(string))
            {
                Id = (TKey)(object)Guid.NewGuid().ToString();
            }
            else if (typeof(TKey) == typeof(Guid))
            {
                Id = (TKey)(object)Guid.NewGuid();
            }
        }
    }
}
