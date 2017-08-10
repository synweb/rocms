using System;
using System.Collections.Generic;
using System.Linq;

namespace RoCMS.Base.Helpers
{
    public static class RandomHelper
    {
        private static readonly Random _random = new Random();

        public static int GetRandom()
        {
            return _random.Next();
        }

        public static int GetRandom(int max)
        {
            return _random.Next(max);
        }

        /// <summary>
        /// min - включительно, max - невключительно
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int GetRandom(int min, int max)
        {
            return _random.Next(min, max);
        }

        /// <summary>
        /// min - включительно, max - невключительно
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static int[] GetRandomArray(int min, int max, int count)
        {
            HashSet<int> check = new HashSet<int>();
            for (Int32 i = 0; i < count; i++)
            {
                int curValue = _random.Next(min, max);
                while (check.Contains(curValue))
                {
                    curValue = _random.Next(min, max);
                }
                check.Add(curValue);
            }
            return check.ToArray();
        }
    }
}
