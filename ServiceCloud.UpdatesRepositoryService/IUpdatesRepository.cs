using ServiceCloud.UpdatesRepositoryService.Models;

namespace ServiceCloud.UpdatesRepositoryService
{
    public interface IUpdatesRepository
    {
        /// <summary>
        /// Коллекция доступных обновлений конфигураций информационных баз 1С
        /// </summary>
        IEnumerable<UpdateModel> Updates { get; }

        /// <summary>
        /// Количество доступных обновлений конфигураций информационных баз 1С
        /// </summary>
        int UpdatesCount { get; }
    }
}
