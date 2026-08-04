using ServiceCloud.UpdatesRepositoryService.Models;
using ServiceCloud.UpdatesRepositoryService.Repository.Keys;

namespace ServiceCloud.UpdatesRepositoryService.Repository
{
    /// <summary>
    /// Представляет индекс репозитория обновлений информационных баз 1С.
    /// Хранит информацию о найденных обновлениях и обеспечивает быстрый поиск
    /// по различным критериям с использованием внутренних индексов
    /// </summary>
    public class RepositoryIndex : IRepositoryIndex
    {
        private Dictionary<UpdateKey, UpdateModel> updates;
        private Dictionary<ConfigKey, List<UpdateModel>> updatesByConfig;

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
        public bool Contains(ConfigurationModel? targetConfig)
        {
            if (targetConfig is not null && CheckConfig(targetConfig))
            {
                return updates.ContainsKey(CreateUpdateKey(targetConfig));
            }

            return false;
        }

        public UpdateModel? GetUpdate(ConfigurationModel? targetConfig, bool transitive)
        {
            if (targetConfig is not null)
            {
                return CheckConfig(targetConfig) ?
                    updates.GetValueOrDefault(CreateUpdateKey(targetConfig)) :
                    throw new ArgumentException();
            }

            throw new ArgumentException(); // TODO Дописать 
        }

        /// <inheritdoc/>
        public IEnumerable<UpdateModel> GetPotentialUpdates(ConfigurationModel? config)
        {
            if (CheckConfig(config))
            {
                throw new ArgumentException(); // TODO Дописать 
            }

            return SelectUpdates(config);
        }

        private IEnumerable<UpdateModel> SelectUpdates(ConfigurationModel config)
        {
            switch (config.Edition)
            {
                case InfoBaseConfigEdition.BASE:

                    var newConfigName = config.Name!.Substring(0, config.Name!.Length - 7);
                    var candidateUpdates = updatesByConfig.GetValueOrDefault(CreateConfigKey(config.Publisher, newConfigName));

                    return candidateUpdates.Where(update => update.IsTransitive
                                            && update.DestinationConfig!.Version == config.Version);

                case InfoBaseConfigEdition.PROF:

                    var filteredCorpTransitives = updatesByConfig.GetValueOrDefault(CreateConfigKey(config.Publisher, config.Name + "КОРП"))
                         .Where(u => u.IsTransitive);

                    candidateUpdates = updatesByConfig.GetValueOrDefault(CreateConfigKey(config));
                   // candidateUpdates = candidateUpdates.Concat(filteredCorpTransitives);

                    return candidateUpdates.Where(update => update.DestinationConfig != config
                                                         && update.DestinationConfig!.Version >= config.Version);

                default:

                    candidateUpdates = updatesByConfig.GetValueOrDefault(CreateConfigKey(config));

                    return candidateUpdates.Where(update => update.DestinationConfig != config
                                                && update.DestinationConfig!.Version > config.Version);
            }

        }

        private bool CheckConfig(ConfigurationModel config)
        {
            return !(string.IsNullOrEmpty(config.Publisher) ||
                string.IsNullOrEmpty(config.Name) ||
                string.IsNullOrEmpty(config.VersionString));
        }

        private UpdateKey CreateUpdateKey(ConfigurationModel config)
        {
            return new(config.Publisher!, config.Name!, config.Version!);
        }

        private ConfigKey CreateConfigKey(ConfigurationModel config)
        {
            return new(config.Publisher!, config.Name!);
        }

        private ConfigKey CreateConfigKey(string сonfigPublisher, string configName)
        {
            return new(сonfigPublisher, configName);
        }
    }
}
