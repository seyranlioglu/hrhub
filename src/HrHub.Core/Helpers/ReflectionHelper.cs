using System.Reflection;

namespace HrHub.Core.Helpers
{
    public static class ReflectionHelper
    {
        public static IEnumerable<MethodInfo> GetMethodsMarkedWithAttribute<T>(Assembly assembly) where T : Attribute
        {
            return assembly.GetTypes()
                .SelectMany(t => t.GetMethods())
                .Where(m => m.GetCustomAttribute<T>() != null);
        }
    }
}
