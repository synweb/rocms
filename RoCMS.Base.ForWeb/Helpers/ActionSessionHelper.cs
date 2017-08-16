using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RoCMS.Base.ForWeb.Helpers
{
    /// <summary>
    /// Хелпер, который хранит урлы для методов API, в которых нужен SessionState
    /// </summary>
    public static class ActionSessionHelper
    {
        private static HashSet<string> _urls = new HashSet<string>();

        /// <summary>
        /// Зарегистрировать регулярное выражение для URL, которому требуется сессия.
        /// Пример: "^~/api/module/item/(.+?)/view"
        /// </summary>
        /// <param name="urlRegex"></param>
        public static void RegisterUrlRegex(string urlRegex)
        {
            _urls.Add(urlRegex);
        }

        public static bool SessionStateRequired(string relativeUrl)
        {
            if (!relativeUrl.StartsWith("~/api/"))
                return false;
            foreach (var urlRegex in _urls)
            {
                if (Regex.IsMatch(relativeUrl, urlRegex))
                    return true;
            }
            return false;
        }
    }
}
