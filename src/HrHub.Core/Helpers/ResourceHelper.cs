using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;

namespace HrHub.Core.Helpers
{
    public static class ResourceHelper
    {
        private static ResourceManager _resourceManager;

        public static void ConfigureResourceHelper<TResource>(string namespacePath)
        {
            _resourceManager = new ResourceManager(namespacePath, typeof(TResource).Assembly);
        }

        public static string GetString(string key)
        {
            return _resourceManager.GetString(key);
        }

        public static string GetString(string key, params object[] args)
        {
            return string.Format(_resourceManager.GetString(key), args);
        }
        public static string GetString(Type type, string key)
        {
            ResourceManager resourceManager = new ResourceManager(type);
            var entry = resourceManager.GetResourceSet(Thread.CurrentThread.CurrentUICulture, true, true)
            .OfType<DictionaryEntry>()
                                       .FirstOrDefault(p => p.Key.ToString() == key);
            return entry.Value.ToString();
        }
    }
}
