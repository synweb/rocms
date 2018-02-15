using System;
using System.Linq;
using System.Text.RegularExpressions;
using RoCMS.Base.Exceptions;

namespace RoCMS.Base.Helpers
{
    public static class FormattingHelper
    {
        public static string FormatPhone(string phone)
        {
            if (!ValidationHelper.ValidatePhone(phone))
            {
                throw new PhoneValidationException();
            }
            string[] forDelete = {"+", "(", ")", " ", "-"};
            string result = RemoveSubstrings(phone, forDelete);
            return result;
        }

        private static string RemoveSubstrings(string input, string[] substrings)
        {
            string result = input;
            foreach (var substring in substrings)
            {
                result = result.Replace(substring, "");
            }
            return result;
        }

        public static string AddRelNofollowToAnchors(string html)
        {
            var anchors = Regex.Matches(html, @"<a.+?>").Cast<Match>().Select(x => x.Value);
            var res = html;
            foreach (var anchor in anchors)
            {
                if (anchor.Contains("rel=\"nofollow\"")) // этот анкор уже Nofollow. его не трогаем.
                    continue;
                var newAnchor = anchor.Replace("<a ", "<a rel=\"nofollow\" ");
                res = res.Replace(anchor, newAnchor);
            }
            return res;
        }

        public static string ToRussianURL(string title)
        {
            return Regex.Replace(title, @"([а-яё])|([\s_-])|([^a-z\d])",
                (ch) =>
                {
                    if (ch.Value.Equals(" ") || ch.Value.Equals("-"))
                    {
                        return "-";
                    }
                    if (Char.IsPunctuation(ch.Value[0]))
                    {
                        return "";
                    }
                    if (Char.IsLetter(ch.Value[0]))
                    {
                        return ch.Value[0].ToString();
                    }
                    return "";
                });
        }

        public static string ToTranslitedUrl(string title, int? maxLength = null)
        {
            var value = Regex.Replace(title.ToLower(), @"([а-яё])|([\s_-])|([^a-z\d])",
                (ch) =>
                {
                    if (ch.Value.Equals(" ") || ch.Value.Equals("-"))
                    {
                        return "-";
                    }
                    if (Char.IsPunctuation(ch.Value[0]))
                    {
                        return "";
                    }
                    if (Char.IsSymbol(ch.Value[0]))
                    {
                        return "";
                    }
                    if (Char.IsLetter(ch.Value[0]))
                    {
                        int code = ch.Value[0];
                        int index = code == 1025 || code == 1105
                            ? 0
                            : code > 1071 ? code - 1071 : code - 1039;

                        if (index < 0) return "";

                        string[] t =
                        {
                            "yo", "a", "b", "v", "g", "d", "e", "zh",
                            "z", "i", "y", "k", "l", "m", "n", "o", "p",
                            "r", "s", "t", "u", "f", "h", "c", "ch", "sh",
                            "shch", "", "y", "", "e", "yu", "ya"
                        };

                        if (index >= t.Length) return "";

                        return t[index];

                    }
                    return "";
                });

            value = Regex.Replace(value, @"--+", "-"); //заменяем -- и более подряд на -

            if (maxLength.HasValue)
            {
                value = Cut(value, maxLength.Value);
            }
            return value;
        }

        private static string Cut(string str, int length)
        {

            if (str.Length <= length) return str;

            string result = str.Substring(0, length - 1);
            //char ch = str[length];

            //StringBuilder sb = new StringBuilder(result);

            //while (char.IsLetter(ch))
            //{
            //    sb.Remove(sb.Length - 1, 1);
            //    ch = sb[sb.Length - 1];
            //}
            //sb.Remove(sb.Length - 1, 1);

            return result;
        }
    }
}
