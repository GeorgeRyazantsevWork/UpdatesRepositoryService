using ServiceCloud.UpdatesRepository.BusinessLogic.Models;

namespace ServiceCloud.UpdatesRepository.BusinessLogic.Repository.Keys
{
    /// <summary>
    /// Уникальный ключ обновления конфигурации информационной базы.
    /// Используется для быстрого поиска обновления по издателю,
    /// наименованию конфигурации, версии и свойству <see cref="Transitive"/>
    /// </summary>
    /// <param name="Publisher">Наименование издателя конфигурации</param>
    /// <param name="Name">Наименование конфигурации</param>
    /// <param name="Version">Версия конфигурации</param>
    /// <param name="Transitive">Признак, указывающий является ли обновление переходным</param>
    public readonly record struct UpdateKey(string Publisher, string Name, InfoBaseConfigVersion Version, bool Transitive);
}
