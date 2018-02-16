using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace RoCMS.Base.Helpers
{
    public static class ParsingHelper
    {
        /// <summary>
        ///     Парсит NameValueCollection по заданному ключу и типу возвращаемого объекта
        ///     Кидает NotSupported для простых типов, кроме string, int(?), bool(?)
        /// </summary>
        /// <typeparam name="T">тип возвращаемого объекта</typeparam>
        /// <param name="dict">GET-словарь</param>
        /// <param name="key">ключ объекта</param>
        public static T ParseObject<T>(NameValueCollection dict, string key)
        {
            T defaultValue = default(T);
            string value = dict[key];

            if (String.IsNullOrWhiteSpace(value))
            {
                return defaultValue;
            }

            Type type = typeof(T);
            if (type.IsEnum)
            {
                int intValue;
                if (Int32.TryParse(value, out intValue))
                {
                    return (T)Enum.ToObject(type, intValue);
                }

                return defaultValue;
            }

            if (type == typeof(bool) || type == typeof(bool?))
            {
                bool bVal;
                if (!Boolean.TryParse(value, out bVal))
                {
                    int iVal;
                    if (!Int32.TryParse(value, out iVal))
                    {
                        return defaultValue;
                    }

                    return (T)(object)(iVal != 0);
                }

                return (T)(object)bVal;
            }
            if (type == typeof(int) || type == typeof(int?))
            {
                int iVal;
                if (!Int32.TryParse(value, out iVal))
                {
                    return defaultValue;
                }

                return (T)(object)iVal;
            }

            if (type == typeof(string))
            {
                return (T)(object)value;
            }

            throw new NotSupportedException();
        }

        public static string RemoveHtml(string src)
        {
            string result = HttpUtility.HtmlDecode(src);
            const string HTML_TAG_PATTERN = "<.*?>";
            result = Regex.Replace(result, HTML_TAG_PATTERN, String.Empty, RegexOptions.Singleline);
            return result;
        }
    }
}
