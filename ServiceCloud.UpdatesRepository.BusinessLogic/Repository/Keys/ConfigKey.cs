namespace ServiceCloud.UpdatesRepository.BusinessLogic.Repository.Keys
{
    /// <summary>
    /// Уникальный ключ конфигурации информационной базы
    /// Используется для группировки всех обновлений одной конфигурации
    /// </summary>
    /// <param name="Publisher">Наименование издателя конфигурации</param>
    /// <param name="Name">Наименование конфигурации</param>
    public readonly record struct ConfigKey(string Publisher, string Name);
}
