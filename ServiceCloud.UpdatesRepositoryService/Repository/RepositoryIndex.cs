using ServiceCloud.UpdatesRepositoryService.Models;

namespace ServiceCloud.UpdatesRepositoryService.Repository
{
    /// <summary>
    /// Представляет индекс репозитория обновлений информационных баз 1С.
    /// Хранит информацию о найденных обновлениях и обеспечивает быстрый поиск
    /// по различным критериям с использованием внутренних индексов
    /// </summary>
    public class RepositoryIndex : IRepositoryIndex
    {
        private Dictionary<string, UpdateModel> updates;
        private Dictionary<string, UpdateModel> updatesByConfig;

       /// <summary>
       /// Конструктор экземпляров класса. Инициализирует объект необходимыми данными
       /// </summary>
        public RepositoryIndex()
        {
            updates = new();
            updatesByConfig = new();
        }

        /// <inheritdoc/>
        public int Count => updates.Count;

        /// <inheritdoc/>
        public bool Contains(ConfigurationModel targetConfig)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IEnumerable<UpdateModel> GetPotentialUpdates(ConfigurationModel? config)
        {
            throw new NotImplementedException();
        }
    }
}
