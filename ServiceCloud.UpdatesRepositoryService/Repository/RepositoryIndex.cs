using ServiceCloud.UpdatesRepositoryService.Models;
using ServiceCloud.UpdatesRepositoryService.Properties;
using ServiceCloud.UpdatesRepositoryService.Repository.Keys;
using System.Collections.Concurrent;

namespace ServiceCloud.UpdatesRepositoryService.Repository
{
    /// <summary>
    /// Представляет индекс репозитория обновлений информационных баз 1С.
    /// Хранит информацию о найденных обновлениях и обеспечивает быстрый поиск
    /// по различным критериям с использованием внутренних индексов
    /// </summary>
    public class RepositoryIndex : IRepositoryIndex
    {
        private readonly ConcurrentDictionary<UpdateKey, UpdateModel> updates;
        private readonly ConcurrentDictionary<ConfigKey, ConcurrentBag<UpdateModel>> updatesByConfig;

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
        public IEnumerable<UpdateModel> GetUpdates()
        {
            return updates.Values;
        }

        /// <inheritdoc/>
        public bool Contains(ConfigurationModel targetConfig)
        {
            if (!ValidateConfig(targetConfig))
            {
                throw new ArgumentException(Resource.Message_IncorrectConfigProperties);
            }

            return updates.ContainsKey(CreateUpdateKey(targetConfig)) ||
                updates.ContainsKey(CreateUpdateKey(targetConfig, transitive: true));
        }

        /// <inheritdoc/>
        public UpdateModel? GetUpdate(ConfigurationModel targetConfig, bool transitive)
        {
            return ValidateConfig(targetConfig) ?
                updates.GetValueOrDefault(CreateUpdateKey(targetConfig, transitive)) :
                throw new ArgumentException(Resource.Message_IncorrectConfigProperties);
        }

        /// <inheritdoc/>
        public IEnumerable<UpdateModel>? GetPotentialUpdates(ConfigurationModel config)
        {
            return ValidateConfig(config) ?
               SelectUpdates(config) :
               throw new ArgumentException(Resource.Message_IncorrectConfigProperties);
        }

        private IEnumerable<UpdateModel>? SelectUpdates(ConfigurationModel config)
        {
            switch (config.Edition)
            {
                case InfoBaseConfigEdition.BASE:
                    {
                        var newConfigName = config.Name!.Substring(0, config.Name!.Length - 7);
                        var candidateUpdates = updatesByConfig.GetValueOrDefault(CreateConfigKey(config.Publisher!, newConfigName));

                        return candidateUpdates?.Where(update => update.IsTransitive
                                                && update.DestinationConfig!.Version == config.Version);

                    }
                case InfoBaseConfigEdition.PROF:
                    {
                        var filteredCorpTransitives = updatesByConfig
                            .GetValueOrDefault(CreateConfigKey(config.Publisher!, config.Name + "КОРП"))?
                            .Where(u => u.IsTransitive);

                        var candidateUpdates = updatesByConfig.GetValueOrDefault(CreateConfigKey(config));
                        var fullcandidateUpdates = candidateUpdates?.Concat(filteredCorpTransitives);

                        return fullcandidateUpdates?.Where(update => update.DestinationConfig != config
                                                         && update.DestinationConfig!.Version >= config.Version);
                    }

                default:
                    {
                        var candidateUpdates = updatesByConfig.GetValueOrDefault(CreateConfigKey(config));

                        return candidateUpdates?.Where(update => update.DestinationConfig != config
                                                    && update.DestinationConfig!.Version > config.Version);
                    }
            }
        }

        /// <inheritdoc/>
        public void Add(UpdateModel update)
        {
            var config = update.DestinationConfig;

            if (config == null)
            {
                return;
            }

            var updateKey = CreateUpdateKey(config, update.IsTransitive);

            if (!updates.TryAdd(updateKey, update))
            {
                return;
            }

            var configKey = CreateConfigKey(config);
            var bag = updatesByConfig.GetOrAdd(configKey, static _ => []);
            bag.Add(update);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            updates.Clear();
            updatesByConfig.Clear();
        }

        private bool ValidateConfig(ConfigurationModel config)
        {
            return !(string.IsNullOrEmpty(config.Publisher) ||
                string.IsNullOrEmpty(config.Name) ||
                string.IsNullOrEmpty(config.VersionString));
        }

        private UpdateKey CreateUpdateKey(ConfigurationModel config, bool transitive = false)
        {
            return new(config.Publisher!, config.Name!, config.Version!, transitive);
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
