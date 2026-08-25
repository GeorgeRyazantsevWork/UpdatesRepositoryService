namespace ServiceCloud.UpdatesRepository.BuisnessLogic.Helpers
{
    /// <summary>
    /// Вспомогательный класс для расчёта хэш-кода объектов
    /// </summary>
    public static class HashCalculator
    {
        /// <summary>
        /// Рассчитать хэш-код массива байт
        /// </summary>
        /// <param name="array">Массив данных</param>
        /// <returns>Хэш-код массива байт</returns>
        public static int CalculateHash(byte[] array)
        {
            if (array == null || array.Length == 0)
            {
                return 0;
            }
            int hash = 0;
            for (int i = 0; i < array.Length; i++)
            {
                hash ^= array[i];
            }
            return hash;
        }

        /// <summary>
        /// Рассчитать хэш-код коллекции объектов
        /// </summary>
        /// <typeparam name="T">Тип элементов коллекции</typeparam>
        /// <param name="collection">Коллекция объектов</param>
        /// <returns>Хэш-код коллекции объектов</returns>
        public static int CalculateHash<T>(IEnumerable<T>? collection)
        {
            if (collection == null)
            {
                return 0;
            }
            int hash = 0;
            foreach (var item in collection)
            {
                if (item != null)
                {
                    hash ^= item.GetHashCode();
                }
            }
            return hash;
        }

        /// <summary>
        /// Рассчитать хэш-код открытых свойств объекта
        /// </summary>
        /// <param name="input">Ссылка на объект</param>
        /// <returns>Хэш-код открытых свойств объекта</returns>
        public static int CalculatePropertiesHashCode(object? input)
        {
            int hash = 0;
            if (input != null)
            {
                foreach (var elem in input.GetType().GetProperties())
                {
                    int? elementHash = elem?.GetValue(input)?.GetHashCode();
                    if (elementHash.HasValue)
                    {
                        hash ^= elementHash.Value;
                    }
                }
            }
            return hash;
        }

        /// <summary>
        /// Рассчитать хэш-код коллекции объектов
        /// </summary>
        /// <param name="collection">Коллекция объектов</param>
        /// <returns>Хэш-код коллекции объектов</returns>
        public static int CalculateHash(params object[] collection)
        {
            int hash = 0;
            foreach (var item in collection)
            {
                if (item != null)
                {
                    hash ^= item.GetHashCode();
                }
            }
            return hash;
        }
    }
}