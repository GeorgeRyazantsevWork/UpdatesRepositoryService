using ServiceCloud.UpdatesRepositoryService.Models;

namespace ServiceCloud.UpdatesRepositoryService.Repository.Keys
{
    /// <summary>
    /// Уникальный ключ обновления конфигурации информационной базы
    /// Используется для быстрого поиска обновления по издателю,
    /// наименованию конфигурации и ее версии
    /// </summary>
    /// <param name="Publisher">Наименование издателя конфигурации</param>
    /// <param name="Name">Наименование конфигурации</param>
    /// <param name="Version">Версия конфигурации</param>
    public readonly record struct UpdateKey(string Publisher, InfoBaseConfigVersion Version, string Name);
}
