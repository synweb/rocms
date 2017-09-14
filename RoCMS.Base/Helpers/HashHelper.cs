using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Base.Helpers
{
    public static class HashHelper
    {
        /// <summary>
        /// Хеш-алгоритм Кнута для быстрого хеширования строки
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static UInt64 GetQuickHash(string input)
        {
            UInt64 hashedValue = 3074457345618258791ul;
            for (int i = 0; i < input.Length; i++)
            {
                hashedValue += input[i];
                hashedValue *= 3074457345618258799ul;
            }
            return hashedValue;
        }
    }
}
