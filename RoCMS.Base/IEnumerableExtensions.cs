using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Base
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> Each<T>(this IEnumerable<T> @this, Action<T> action)
        {
            foreach (var x in @this)
            {
                action(x);
            }
            return @this;
        }

        public static IEnumerable<T> ConcatMany<T>(this IEnumerable<T> @this, params IEnumerable<T>[] items)
        {
            IEnumerable<T> res = @this;
            foreach (var item in items)
            {

                res = res.Concat(item);
            }
            return res;
        }
    }
}
