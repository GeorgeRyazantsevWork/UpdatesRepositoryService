using ServiceCloud.UpdatesRepositoryService.Models;

namespace ServiceCloud.UpdatesRepositoryService.Repository
{
    public interface IRepositoryIndex
    {

        /// <summary>
        /// Количество обновлений в репозитории
        /// </summary>
        public int Count { get; }

        /// <summary>
        /// Позволяет получить список обновлений, теоретически подходящих для указанной конфигурации
        /// </summary>
        /// <param name="config">Указанная конфигурация информационной базы 1С</param>
        /// <returns>Список обновлений</returns>
        public IEnumerable<UpdateModel> GetPotentialUpdatesFor(ConfigurationModel? config);        

        /// <summary>
        /// Позволяет определить по целевой конфигурации обновления содержится ли оно в репозитории 
        /// </summary>
        /// <param name="targetConfig">Конфигурация искомого обновления</param>
        /// <returns>true - обновление существует в репозитории, false - в обратном случае</returns>
        public bool Contains(ConfigurationModel targetConfig);

        void Replace(IReadOnlyCollection<UpdateModel> updates);
    }
}
