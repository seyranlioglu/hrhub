using HrHub.Abstraction.Attributes;
using HrHub.Abstraction.Settings;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Core.Helpers
{
    public static class AppSettingsHelper
    {
        private static IConfiguration configuration;
        public static void AppSettingsHelperConfigure(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public static TObj GetData<TObj>() where TObj : ISettingsBase
        {
            string path = AttributeHelper<AppSettingAttribute, TObj>.GetAttributeValue<string>("Path");
            return configuration.GetSection(path).Get<TObj>();
        }

        public static List<TBaseType> GetDataByList<TBaseType>() where TBaseType : ISettingsBase
        {
            string path = AttributeHelper<AppSettingAttribute, TBaseType>.GetAttributeValue<string>("Path");
            return configuration.GetSection(path).Get<List<TBaseType>>();
        }
        public static TList GetDataByList<TList, TBaseType>() where TList : IEnumerable<ISettingsBase>
                                                           where TBaseType : ISettingsBase
        {
            string path = AttributeHelper<AppSettingAttribute, TBaseType>.GetAttributeValue<string>("Path");
            return configuration.GetSection(path).Get<TList>();
        }
    }
}
