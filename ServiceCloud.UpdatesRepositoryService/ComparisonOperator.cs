namespace ServiceCloud.UpdatesRepositoryService
{
    /// <summary>
    /// Оператор сравнения
    /// </summary>
    public enum ComparisonOperator
    {
        /// <summary>
        /// Сравниваемые значения равны
        /// </summary>
        Equal,
        /// <summary>
        /// Сравниваемые значения не равны
        /// </summary>
        NotEqual,
        /// <summary>
        /// Первое сравниваемое значение строго больше второго
        /// </summary>
        Greater,
        /// <summary>
        /// Первое сравниваемое значение больше или равно второму
        /// </summary>
        GreaterOrEqual,
        /// <summary>
        /// Первое сравниваемое значение строго меньше второго
        /// </summary>
        Less,
        /// <summary>
        /// Первое сравниваемое значение меньше или равно второму
        /// </summary>
        LessOrEqual
    }
}
