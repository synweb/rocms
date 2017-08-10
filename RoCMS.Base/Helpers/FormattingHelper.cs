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

        public static string ToRussianURL(string url)
        {
            return Regex.Replace(url, @"([а-яё])|([\s_-])|([^a-z\d])",
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

        public static string ToTranslitURL(string url)
        {
            return TranslitHelper.ToTranslitedUrl(url);
        }
    }
}
