using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Abstraction.Extensions
{
    public static class IEnumerableExtentions
    {
        public static IEnumerable<TEntity> AsHierarchyReview<TEntity, TKeyProperty>(
            this IEnumerable<TEntity> items,
            Func<TEntity, TKeyProperty> keyProperty,
            Func<TEntity, TKeyProperty> parentKeyProperty,
            Action<TEntity, TEntity> parentSetter,
            Action<TEntity, TEntity> childSetter)
        {
            return items.AsHierarchyReview(keyProperty, parentKeyProperty, parentSetter, childSetter,
                default, default);
        }

        private static IEnumerable<TEntity> AsHierarchyReview<TEntity, TKeyProperty>(
            this IEnumerable<TEntity> items,
            Func<TEntity, TKeyProperty> keyProperty,
            Func<TEntity, TKeyProperty> parentKeyProperty,
            Action<TEntity, TEntity> parentSetter,
            Action<TEntity, TEntity> childSetter,
            TKeyProperty parentKeyValue,
            TEntity parentValue)
        {
            foreach (var item in items.Where(item => parentKeyProperty(item).Equals(parentKeyValue)))
            {
                parentSetter?.Invoke(item, parentValue);

                var childrenValues = items.AsHierarchyReview(keyProperty, parentKeyProperty, parentSetter, childSetter,
                    keyProperty(item), item).ToList();
                foreach (var child in childrenValues)
                {
                    childSetter?.Invoke(child, item);
                }

                yield return item;
            }
        }

        public static bool IsNullOrEmpty<TEntity>(this IEnumerable<TEntity> entities)
        {
            if (entities != null && entities.Count() > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
