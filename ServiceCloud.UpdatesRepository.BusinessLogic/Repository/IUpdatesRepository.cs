using ServiceCloud.UpdatesRepository.BusinessLogic.Data;

namespace ServiceCloud.UpdatesRepository.BusinessLogic.Repository
{
    /// <summary>
    /// Интерфейс репозитория с обновлениями конфигураций информационных баз 1С
    /// </summary>
    public interface IUpdatesRepository
    {
        /// <summary>
        /// Коллекция доступных обновлений конфигураций информационных баз 1С
        /// </summary>
        IEnumerable<InfoBaseUpdate> Updates { get; }

        /// <summary>
        /// Количество доступных обновлений конфигураций информационных баз 1С
        /// </summary>
        int UpdatesCount { get; }

        /// <summary>
        /// Обновить данные о доступных обновлениях в репозитории
        /// </summary>
        Task Refresh();

        /// <summary>
        /// Проверить наличие обновления для заданной конфигурации информационной базы
        /// </summary>
        /// <param name="config">Конфигурации информационной базы</param>
        /// <returns>TRUE - обновление присутствует в репозитории, FALSE - в противном случае</returns>
        bool ContainsUpdate(InfobaseConfig ? config);

        /// <summary>
        /// Получить данные об обновлении информационной базы до указанной конфигурации с учётом необходимости перехода на новую редакцию
        /// </summary>
        /// <param name="config">Конфигурация информационной базы</param>
        /// <param name="transitive">TRUE - Обновление должно быть переходным (с редакции, отличной он указанной в параметре config), FALSE - обновление не изменяет редакцию базы</param>
        /// <exception cref="ArgumentException">Целевая конфигурация задана не полностью</exception>
        /// <exception cref="ArgumentNullException">Не задана целевая конфигурация</exception>
        /// <exception cref="KeyNotFoundException">Обновление для конфигурации не найдено в репозитории</exception>
        /// <returns>Данные об обновлении</returns>
        InfoBaseUpdate? GetUpdate(InfobaseConfig? config, bool transitive);

        /// <summary>
        /// Получить список обновлений, которые предположительно можно установить на указанную конфигурацию
        /// </summary>
        /// <param name="config">Конфигурация информационной базы 1С</param>
        /// <returns>Коллекция обновлений</returns>
        IEnumerable<InfoBaseUpdate> GetPotentialUpdates(InfobaseConfig? config);

        /// <summary>
        /// Получить информацию о расположении данных обновления конфигурации информационной базы 1С
        /// </summary>
        /// <param name="updateId">Идентификатор обновлении конфигурации информационной базы 1С в репозитории</param>
        /// <exception cref="ArgumentException">Не задан идентификатор обновления</exception>
        /// <exception cref="KeyNotFoundException">Обновление для конфигурации не найдено в репозитории</exception>
        /// <returns>Расположение данных обновления</returns>
        string GetLocation(string? updateId);

        /// <summary>
        /// Очистка индекса репозитория
        /// </summary>
        void Clear();
    }
}
