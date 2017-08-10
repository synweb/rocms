using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Base.Extentions
{
    public static class StringExtensions
    {
        public static string UppercaseFirst(this string s)
        {

            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public static string LowercaseFirst(this string s)
        {

            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            return char.ToLower(s[0]) + s.Substring(1);
        }

        public static String CompactWhitespaces(this string s)
        {
            StringBuilder sb = new StringBuilder(s);

            CompactWhitespaces(sb);

            return sb.ToString();
        }

        private static void CompactWhitespaces(StringBuilder sb)
        {
            if (sb.Length == 0)
                return;

            int start = 0;

            while (start < sb.Length)
            {
                if (Char.IsWhiteSpace(sb[start]))
                    start++;
                else
                    break;
            }

            if (start == sb.Length)
            {
                sb.Length = 0;
                return;
            }

            int end = sb.Length - 1;

            while (end >= 0)
            {
                if (Char.IsWhiteSpace(sb[end]))
                    end--;
                else
                    break;
            }

            int dest = 0;
            bool previousIsWhitespace = false;

            for (int i = start; i <= end; i++)
            {
                if (Char.IsWhiteSpace(sb[i]))
                {
                    if (!previousIsWhitespace)
                    {
                        previousIsWhitespace = true;
                        sb[dest] = ' ';
                        dest++;
                    }
                }
                else
                {
                    previousIsWhitespace = false;
                    sb[dest] = sb[i];
                    dest++;
                }
            }

            sb.Length = dest;
        }
    }
}
