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
        private readonly ConcurrentDictionary<ConfigKey, List<UpdateModel>> updatesByConfig;

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
        public IEnumerable<UpdateModel> GetPotentialUpdates(ConfigurationModel config)
        {
            return ValidateConfig(config) ?
               SelectUpdates(config) :
               throw new ArgumentException(Resource.Message_IncorrectConfigProperties);
        }

        private IEnumerable<UpdateModel> SelectUpdates(ConfigurationModel config)
        {
            var version = config.Version!;
            var result = new List<UpdateModel>();

            if (config.Edition == InfoBaseConfigEdition.BASE)
            {
                var newConfigName = config.Name!.Substring(0, config.Name!.Length - 7);
                if (updatesByConfig.TryGetValue(CreateConfigKey(config.Publisher!, newConfigName), out var candidates))
                {
                    lock (candidates)
                    {
                        foreach (var update in candidates)
                        {
                            if (update.IsTransitive && update.DestinationConfig!.Version == version)
                            {
                                result.Add(update);
                            }
                        }
                    }
                }

                return result;
            }

            if (updatesByConfig.TryGetValue(CreateConfigKey(config), out var ownUpdates))
            {
                var includeEqualVersion = config.Edition == InfoBaseConfigEdition.PROF;
                lock (ownUpdates)
                {
                    foreach (var update in ownUpdates)
                    {
                        var destination = update.DestinationConfig!;
                        if (destination == config)
                        {
                            continue;
                        }

                        if (includeEqualVersion ? destination.Version >= version : destination.Version > version)
                        {
                            result.Add(update);
                        }
                    }
                }
            }

            if (config.Edition == InfoBaseConfigEdition.PROF &&
                updatesByConfig.TryGetValue(CreateConfigKey(config.Publisher!, config.Name + "КОРП"), out var corpUpdates))
            {
                lock (corpUpdates)
                {
                    foreach (var update in corpUpdates)
                    {
                        if (update.IsTransitive && update.DestinationConfig!.Version >= version)
                        {
                            result.Add(update);
                        }
                    }
                }
            }

            return result;
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
            var updatesOfConfig = updatesByConfig.GetOrAdd(configKey, static _ => []);
            lock (updatesOfConfig)
            {
                updatesOfConfig.Add(update);
            }
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
