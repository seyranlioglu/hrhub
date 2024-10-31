using HrHub.Abstraction.Settings;
using System.Reflection;

namespace HrHub.Core.Helpers
{
    public static class AttributeHelper<TAttribute, TObj> where TAttribute : Attribute where TObj : ISettingsBase
    {
        public static TType GetAttributeValue<TType>(string propertyName) where TType : class
        {
            var instance = Activator.CreateInstance(typeof(TObj));
            Type type = instance.GetType();

            TAttribute attribute = type.GetCustomAttribute<TAttribute>();

            if (attribute != null)
            {
                var property = attribute.GetType().GetProperty(propertyName);
                var saltValue = property.GetValue(attribute);
                var value = Convert.ChangeType(saltValue, typeof(TType));
                return (TType)value;
            }
            return null;
        }
    }
}
