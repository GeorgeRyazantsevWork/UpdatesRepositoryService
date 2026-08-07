using ServiceCloud.UpdatesRepositoryService.Models;

namespace ServiceCloud.UpdatesRepositoryService.Repository
{
    /// <summary>
    /// Интерфейс индекса репозитория обновлений информационных баз 1С
    /// </summary>
    public interface IRepositoryIndex
    {
        /// <summary>
        /// Количество обновлений в репозитории
        /// </summary>
        public int Count { get; }

        /// <summary>
        /// Получить коллекцию доступных обновлений конфигураций информационных баз 1С
        /// </summary>
        /// <returns>Коллекция доступных обновлений</returns>
        public IEnumerable<UpdateModel> GetUpdates();

        /// <summary>
        /// Добавляет описание обновления в репозиторий
        /// </summary>
        /// <param name="update">добавляемое обновление</param>
        public void Add(UpdateModel update);

        /// <summary>
        /// Получить обновление по целевой конфигурации
        /// </summary>
        /// <param name="targetConfig">Целевая конфигурация</param>
        /// <param name="transitive">Признак, означающий является ли конфигурация переходной</param>
        /// <returns>Обновление для заданной конфигурации</returns>
        public UpdateModel? GetUpdate(ConfigurationModel targetConfig, bool transitive);

        /// <summary>
        /// Получить список обновлений, теоретически подходящих для указанной конфигурации
        /// </summary>
        /// <param name="config">Указанная конфигурация информационной базы 1С</param>
        /// <returns>Список обновлений</returns>
        public IEnumerable<UpdateModel>? GetPotentialUpdates(ConfigurationModel config);

        /// <summary>
        /// Определить по целевой конфигурации обновления содержится ли оно в репозитории 
        /// </summary>
        /// <param name="targetConfig">Конфигурация искомого обновления</param>
        /// <returns>true - обновление существует в репозитории, false - в обратном случае</returns>
        public bool Contains(ConfigurationModel targetConfig);

        /// <summary>ы
        /// Очистить индекс репозитория
        /// </summary>
        public void Clear();
    }
}
