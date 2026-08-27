using System.Collections.Generic;
using System.Linq;

namespace ServiceCloud.Repository1CService.Helpers
{
    /// <summary>
    /// Вспомогательный класс для сравнения коллекций на равенство
    /// </summary>
    public static class CollectionsComparer
    {
        /// <summary>
        /// Сравнивает коллекции поэлементно с учетом порядка следования элементов в них
        /// </summary>
        /// <typeparam name="T">Тип элементов коллекции</typeparam>
        /// <param name="collection1">Первая коллекция для сравнения</param>
        /// <param name="collection2">Вторая коллекция для сравнения</param>
        /// <returns>True - элементы в коллекциях совпадают, False - в противном случае</returns>
        public static bool CollectionsAreEqual<T>(IEnumerable<T>? collection1, IEnumerable<T>? collection2)
        {
            if (collection1 == null && collection2 == null)
            {
                return true;
            }
            else if (collection1 != null && collection2 != null)
            {
                int collectionSize = collection1.Count();
                if (collectionSize != collection2.Count())
                {
                    return false;
                }
                var enumerator1 = collection1.GetEnumerator();
                var enumerator2 = collection2.GetEnumerator();
                bool result = true;
                while (result && enumerator1.MoveNext() && enumerator2.MoveNext())
                {
                    var item1 = enumerator1.Current;
                    var item2 = enumerator2.Current;
                    if (item1 == null && item2 == null)
                    {
                        continue;
                    }
                    else if (item1 != null && item2 != null)
                    {
                        result = item1.Equals(item2);
                    }
                    else
                    {
                        result = false;
                    }
                }
                return result;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Сравнивает коллекции поэлементно без учета порядка следования элементов в них
        /// </summary>
        /// <typeparam name="T">Тип элементов коллекции</typeparam>
        /// <param name="collection1">Первая коллекция для сравнения</param>
        /// <param name="collection2">Вторая коллекция для сравнения</param>
        /// <returns>True - элементы в коллекциях совпадают, False - в противном случае</returns>
        public static bool CollectionsAreContainsSameElements<T>(IEnumerable<T>? collection1, IEnumerable<T>? collection2)
        {
            if (collection1 == null && collection2 == null)
            {
                return true;
            }
            else if (collection1 != null && collection2 != null)
            {
                var list1 = collection1.ToList();
                var list2 = collection2.ToList();

                if (list1.Count != list2.Count)
                {
                    return false;
                }
                foreach (var e in list1)
                {
                    if (!list2.Contains(e))
                    {
                        return false;
                    }
                    list2.Remove(e);
                }
                return list2.Count == 0;
            }
            return false;
        }
    }
}
