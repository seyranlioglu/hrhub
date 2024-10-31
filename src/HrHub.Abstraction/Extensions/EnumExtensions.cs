using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Abstraction.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum enumValue)
        {
            string result = enumValue.ToString();

            FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());

            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                result = attributes.First().Description;
            }

            return result;
        }

        public static string[] ToDescriptionArray<T>()
            where T : IConvertible //,struct
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            List<string> result = new List<string>();
            foreach (var e in Enum.GetValues(typeof(T)))
            {
                var fi = e.GetType().GetField(e.ToString());
                var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                var description = attributes[0].Description;
                if (!result.Contains(description))
                {
                    result.Add(description);
                }
            }

            return result.ToArray();
        }

        public static string ToStringValue(this Enum source)
        {
            return source.ToString("D");
        }

        public static int ToInt(this Enum source)
        {
            return Convert.ToInt32(source);
        }
    }
}
