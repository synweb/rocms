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

        public static string ToSearchIndexText(string text)
        {
            string result = ParsingHelper.RemoveHtml(text).ToLower();
            result = Regex.Replace(result, @"\[\[.+?\]\]", string.Empty); // спецблоки разметки
            result = Regex.Replace(result, @" [—–-]+ ", " "); // тире 
            result = Regex.Replace(result, @"[,!\.?—…©®""/\\]", string.Empty); // символы
            result = Regex.Replace(result, @"\s+", " ").Trim(); // лишние пробелы
            return result;
        }
    }
}