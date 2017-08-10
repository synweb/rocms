using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Base.Helpers
{
    public static class GrammarHelper
    {
        /// <summary>
        /// Склоняет существительное в зависимости от числительного идущего перед ним.
        /// </summary>
        /// <param name="num">Число идущее перед существительным.</param>
        /// <param name="nominative">Именительный падеж слова.</param>
        /// <param name="singular">Родительный падеж, ед. число.</param>
        /// <param name="plural">Родительный падеж, множ. число.</param>
        public static string Decline(this int num, string nominative, string singular, string plural)
        {
            if (num > 10 && ((num % 100) / 10) == 1) return plural;

            switch (num % 10)
            {
                case 1:
                    return nominative;
                case 2:
                case 3:
                case 4:
                    return singular;
                default: // case 0, 5-9
                    return plural;
            }
        }

    }
}
