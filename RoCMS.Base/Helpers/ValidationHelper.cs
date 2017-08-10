using System;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace RoCMS.Base.Helpers
{
    public static class ValidationHelper
    {
        public static bool ValidateEmail(string email)
        {
            return Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        public static bool ValidatePhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                return false;
            }
            const string ALLOWED_SYMBOLS_REGEX = @"[-()+ ]";
            string phoneWithoutSymbols = Regex.Replace(phone, ALLOWED_SYMBOLS_REGEX, "");
            if (!Regex.IsMatch(phoneWithoutSymbols, @"\d+"))
            {
                return false;
            }
            if (phoneWithoutSymbols.Length < 9 || phoneWithoutSymbols.Length > 20)
            {
                return false;
            }
            return Regex.IsMatch(phone, @"^\+?(\d[\d- ]+)(\([\d- ]+\))?[\d- ]+\d$");

        }

        public static bool ValidateTags(string tags)
        {
            if (string.IsNullOrEmpty(tags))
            {
                return false;
            }
            var collection = tags.Split(',');

            if (collection.Length != collection.Distinct().Count())
            {
                return false;
            }

            return collection.All(tag => tag.Length <= 200 && Regex.IsMatch(tag, @"^\w[\w -]*\w$"));
        }

        public static bool ValidateThumbnailSizes(string sizes)
        {
            if (string.IsNullOrEmpty(sizes))
                return true;
            var elements = sizes.Split(',');
            string pattern = @"^\d+(w|h)$";

            for (int i = 0; i < elements.Length; i++)
            {
                var element = elements[i];
                if (!Regex.IsMatch(element, pattern))
                    return false;
                // проверка, нет ли таких же элементов
                for (int j = i + 1; j < elements.Length; j++)
                {
                    if (elements[i].Equals(elements[j]))
                        return false;
                }
            }


            return true;
        }
    }
}
