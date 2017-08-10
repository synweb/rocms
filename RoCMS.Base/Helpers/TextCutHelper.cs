using System.Text;

namespace RoCMS.Base.Helpers
{
    public static class TextCutHelper
    {
        public static string Cut(string str, int length)
        {
            if (str.Length <= length) return str;

            string result = str.Substring(0, length - 1);
            char ch = str[length];

            StringBuilder sb = new StringBuilder(result);

            while (char.IsLetter(ch))
            {
                sb.Remove(sb.Length - 1, 1);
                ch = sb[sb.Length - 1];
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("...");

            return sb.ToString();
        }
    }
}