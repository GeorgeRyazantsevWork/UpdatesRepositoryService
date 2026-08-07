using ServiceCloud.UpdatesRepositoryService.Helpers;
using ServiceCloud.UpdatesRepositoryService.Models;
using ServiceCloud.UpdatesRepositoryService.Properties;
using System.Collections.Concurrent;

namespace ServiceCloud.UpdatesRepositoryService.Repository
{
    /// <summary>
    /// Описывает репозиторий обновлений для конфигураций 1С
    /// </summary>
    public class UpdatesRepository : IUpdatesRepository
    {
        private readonly SemaphoreSlim refreshLock = new(1, 1);
        private readonly ReaderWriterLockSlim cacheAccessLock = new(LockRecursionPolicy.NoRecursion);
        private IRepositoryIndex repository;
        private const int SEARCH_DEPTH = 64;

        /// <summary>
        /// Конструктор экземпляров класса. Инициализирует объект необходимыми данными
        /// </summary>
        public UpdatesRepository(IRepositoryIndex repositoryIndex, string rootDirPath)
        {
            repository = repositoryIndex ?? throw new ArgumentNullException(nameof(repositoryIndex));
            RootDirectoryPath = rootDirPath;
            LocationMap = new();
        }

        /// <summary>
        /// Полный путь к директории, в которой находятся файлы обновлений
        /// </summary>
        public string? RootDirectoryPath { get; set; }

        /// <summary>
        /// Словарь, позволяющий определить расположение обновлений, содержащихся в репозитории
        /// </summary>
        private Dictionary<string, string> LocationMap { get; set; }

        /// <inheritdoc/>
        public IEnumerable<UpdateModel> Updates => repository.GetUpdates();

        /// <inheritdoc/>
        public int UpdatesCount
        {
            get
            {
                cacheAccessLock.EnterReadLock();
                try
                {
                    return repository.Count;
                }
                finally
                {
                    cacheAccessLock.ExitReadLock();
                }
            }
        }

        /// <inheritdoc/>
        public bool ContainsUpdate(ConfigurationModel? config)
        {
            if (config is null)
            {
                return false;
            }

            cacheAccessLock.EnterReadLock();
            try
            {
                return repository.Contains(config);
            }
            finally
            {
                cacheAccessLock.ExitReadLock();
            }
        }

        /// <inheritdoc/>
        /// <exception cref="SecurityException">Не удалось получить доступ к "корневому" каталогу</exception>
        /// <exception cref="InvalidOperationException">Путь к "корневому" каталогу не задан или содержит недопустимые знаки</exception>
        /// <exception cref="PathTooLongException">Путь к "корневому" каталогу слишком длинный</exception>
        /// <exception cref="Exception">Не удалось провести анализ "корневого" каталога</exception>
        public async Task Refresh()
        {
            await refreshLock.WaitAsync();

            try
            {
                var directory = new DirectoryInfo(RootDirectoryPath ?? Directory.GetCurrentDirectory());
                var internalLocationMap = new ConcurrentDictionary<string, string>();

                BuildRepository(directory, 0, internalLocationMap);

                cacheAccessLock.EnterWriteLock();
                try
                {
                    LocationMap = new Dictionary<string, string>(internalLocationMap);
                }
                finally
                {
                    cacheAccessLock.ExitWriteLock();
                }
            }
            catch (ArgumentException)
            {
                throw new InvalidOperationException(string.Format(Resource.MessageTemplate_IncorrectPropertyValue, 
                    nameof(RootDirectoryPath), RootDirectoryPath));
            }
            finally
            {
                refreshLock.Release();
            }
        }

        private void BuildRepository(DirectoryInfo directory, int depth, ConcurrentDictionary<string, string> map)
        {
            if (depth++ > SEARCH_DEPTH)
            {
                return;
            }
            if (RepositoryParser.ValidateUpdateFiles(directory.FullName))
            {
                using var idGenerator = new UpdateIdGenerator();
                foreach (var cucFile in directory.EnumerateFiles("*.cuc"))
                {
                    if (RepositoryParser.TryParseСUCFile(cucFile.FullName, out UpdateModel? update))
                    {
                        update.Id = idGenerator.NewId(update);

                        if (map.TryAdd(update.Id!, directory.FullName))
                        {
                            cacheAccessLock.EnterWriteLock();
                            try
                            {
                                repository.Add(update);
                            }
                            finally
                            {
                                cacheAccessLock.ExitWriteLock();
                            }
                        }
                    }
                }
            }

            Parallel.ForEach(directory.EnumerateDirectories(), subDir =>
               BuildRepository(subDir, depth, map));
        }

        /// <inheritdoc/>
        public string GetLocation(string? updateId)
        {
            cacheAccessLock.EnterReadLock();
            try
            {
                return LocationMap[updateId!];
            }
            finally
            {
                cacheAccessLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Очистка индекса репозитория
        /// </summary>
        public void Clear()
        {
            cacheAccessLock.EnterWriteLock();
            try
            {
                repository.Clear();
                LocationMap.Clear();
            }
            finally
            {
                cacheAccessLock.ExitWriteLock();
            }
        }

        /// <inheritdoc/>
        public IEnumerable<UpdateModel>? GetPotentialUpdates(ConfigurationModel? config)
        {
            ArgumentNullException.ThrowIfNull(config, nameof(config));


            cacheAccessLock.EnterReadLock();
            try
            {
                return repository.GetPotentialUpdates(config);
            }
            finally
            {
                cacheAccessLock.ExitReadLock();
            }
        }

        /// <inheritdoc/>
        public UpdateModel? GetUpdate(ConfigurationModel? config, bool transitive)
        {
            ArgumentNullException.ThrowIfNull(config, nameof(config));

            cacheAccessLock.EnterReadLock();
            try
            {
                return repository.GetUpdate(config, transitive)!;
            }
            finally
            {
                cacheAccessLock.ExitReadLock();
            }
        }
    }
}
