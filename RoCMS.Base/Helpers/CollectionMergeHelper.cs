using System;
using System.Collections.Generic;
using System.Linq;

namespace RoCMS.Base.Helpers
{
    public static class CollectionMergeHelper
    {
        /// <summary>
        /// Смёрджить коллекции из новых и старых элементов. Для новых, старых и существующих элементов будут вызываться фукнции.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newItems">Коллекция добавляемых элементов</param>
        /// <param name="existingItems">Коллекция имеющихся элементов</param>
        /// <param name="comparer">Функция сравнения элементов</param>
        /// <param name="create">Функция для новых (например, добавление)</param>
        /// <param name="update">Функция для тех, которые есть в обеих коллекциях (например, обновление)</param>
        /// <param name="delete">Функция для старых (например, удаление)</param>
        public static void MergeNewAndOld<T>(IEnumerable<T> newItems, IEnumerable<T> existingItems, 
            Func<T,T, bool> comparer, 
            Action<T> create, Action<T> update, Func<T, bool> delete)
        {
            var newList = newItems as IList<T> ?? newItems.ToList();
            var oldList = existingItems as IList<T> ?? existingItems.ToList();
            foreach (var item in newList.Where(x => oldList.Any(y => comparer(x,y))))
            {
                // элемент есть и в новой, и в старой коллекции
                update(item);
            }
            foreach (var item in newList.Where(x => oldList.All(y => !comparer(x, y))))
            {
                // элемент есть только в новой коллекции
                create(item);
            }
            foreach (var item in oldList.Where(x => newList.All(y => !comparer(x, y))))
            {
                // элемент есть только в старой коллекции
                delete(item);
            }
        }
    }
}