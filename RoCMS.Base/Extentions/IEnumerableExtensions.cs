using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RoCMS.Base.Helpers;

namespace RoCMS.Base.Extentions
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Итератор по элементам перечисления
        /// </summary>
        /// <typeparam name="T">Тип элементов в перечислении</typeparam>
        /// <param name="items">Элементы</param>
        /// <param name="action">Действие над элементом. Передается текущий элемент и его индекс в перечислении</param>
        public static void ForEach<T>(this IEnumerable items, Action<T, int> action)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            int index = 0;
            foreach (T item in items)
            {
                action(item, index);
                index++;
            }
        }

        /// <summary>
        /// Итератор по элементам перечисления
        /// </summary>
        /// <typeparam name="T">Тип элементов в перечислении</typeparam>
        /// <param name="items">Элементы</param>
        /// <param name="action">Действие над элементом. Передается текущий элемент</param>
        public static void ForEach<T>(this IEnumerable items, Action<T> action)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            foreach (T item in items)
            {
                action(item);
            }
        }

        /// <summary>
        /// Итератор по элементам перечисления
        /// </summary>
        /// <typeparam name="T">Тип элементов в перечислении</typeparam>
        /// <param name="items">Элементы</param>
        /// <param name="action">Действие над элементом. Передается текущий элемент и его индекс в перечислении</param>
        public static void ForEach<T>(this IEnumerable<T> items, Action<T, int> action)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            int index = -1;
            foreach (T item in items)
            {
                action(item, ++index);
            }
        }

        /// <summary>
        /// Итератор по элементам перечисления
        /// </summary>
        /// <typeparam name="T">Тип элементов в перечислении</typeparam>
        /// <param name="items">Элементы</param>
        /// <param name="action">Действие над элементом. Передается текущий элемент</param>
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            foreach (T item in items)
            {
                action(item);
            }
        }



        public static T RandomOrDefault<T>(this IEnumerable<T> items)
        {
            var array = items.ToArray();
            if (array.Count() == 0)
            {
                return default(T);
            }
            int index = RandomHelper.GetRandom(array.Length);
            return array[index];
        }

        public static IEnumerable<T> TakeRandom<T>(this IEnumerable<T> items, int count)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            var list = items.ToList();
            if (count <= 0)
            {
                throw new ArgumentException("count");
            }
            if (count > list.Count)
            {
                return list;
            }
            if (count == list.Count)
            {
                return list;
            }
            var res = new List<T>();
            for (int i = 0; i < count; i++)
            {
                int index = RandomHelper.GetRandom(list.Count);
                res.Add(list.ElementAt(index));
                list.RemoveAt(index);
            }
            return res;
        }


    }
}