using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HrHub.Abstraction.Extensions
{
    public static class StringExtensions
    {
        // Trims and checks if the string is empty or only contains whitespace
        public static bool IsNullOrWhitespace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        // Safely converts a string to an integer, with a default value if it fails
        public static int ToInt(this string str, int defaultValue = 0)
        {
            if (int.TryParse(str, out int result))
            {
                return result;
            }
            return defaultValue;
        }

        // Safely converts a string to a nullable integer
        public static int? ToNullableInt(this string str)
        {
            if (int.TryParse(str, out int result))
            {
                return result;
            }
            return null;
        }

        // Converts a string to a boolean, with support for various truthy/falsey values
        public static bool ToBool(this string str)
        {
            return str.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                   str.Equals("yes", StringComparison.OrdinalIgnoreCase) ||
                   str.Equals("1");
        }

        // Converts a string to a nullable boolean
        public static bool? ToNullableBool(this string str)
        {
            if (str.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                str.Equals("yes", StringComparison.OrdinalIgnoreCase) ||
                str.Equals("1"))
            {
                return true;
            }
            if (str.Equals("false", StringComparison.OrdinalIgnoreCase) ||
                str.Equals("no", StringComparison.OrdinalIgnoreCase) ||
                str.Equals("0"))
            {
                return false;
            }
            return null;
        }

        // Safely converts a string to a DateTime with an optional format
        public static DateTime? ToNullableDateTime(this string str, string format = null)
        {
            if (string.IsNullOrEmpty(format))
            {
                if (DateTime.TryParse(str, out DateTime result))
                {
                    return result;
                }
            }
            else
            {
                if (DateTime.TryParseExact(str, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                {
                    return result;
                }
            }
            return null;
        }

        // Checks if the string matches a regular expression pattern
        public static bool MatchesPattern(this string str, string pattern)
        {
            return Regex.IsMatch(str, pattern);
        }

        // Converts a string to a proper title case
        public static string ToTitleCase(this string str)
        {
            var cultureInfo = new CultureInfo("en-US");
            var textInfo = cultureInfo.TextInfo;
            return textInfo.ToTitleCase(str.ToLower());
        }

        // Truncates a string to a specified length with an optional suffix
        public static string Truncate(this string str, int maxLength, string suffix = "...")
        {
            if (str.Length <= maxLength)
            {
                return str;
            }

            return str.Substring(0, maxLength - suffix.Length) + suffix;
        }
    }

}
