using System.Text.RegularExpressions;
using System.Web;
using RoCMS.Base.Extentions;

namespace RoCMS.Base.Helpers
{
    public static class SearchHelper
    {
        public static string[] ExtractKeywords(string query)
        {
            char[] escapeChars = { ',', '!' };
            string searchPattern = query.ToLower();
            
            foreach (var c in escapeChars)
            {
                searchPattern = searchPattern.Replace(c, ' ');
            }
            searchPattern = searchPattern.CompactWhitespaces();
            var keywords = searchPattern.Split(' ');
            return keywords;
        }

        public static string RemoveHtml(string src)
        {
            string result = HttpUtility.HtmlDecode(src);
            const string HTML_TAG_PATTERN = "<.*?>";
            result = Regex.Replace(result, HTML_TAG_PATTERN, string.Empty, RegexOptions.Singleline);
            return result;
        }

        public static string ToSearchIndexText(string text)
        {
            string result = RemoveHtml(text).ToLower();
            result = Regex.Replace(result, @"\[\[.+?\]\]", string.Empty); // спецблоки разметки
            result = Regex.Replace(result, @" [—–-]+ ", " "); // тире 
            result = Regex.Replace(result, @"[,!\.?—…©®""/\\]", string.Empty); // символы
            result = Regex.Replace(result, @"\s+", " ").Trim(); // лишние пробелы
            return result;
        }
    }
}