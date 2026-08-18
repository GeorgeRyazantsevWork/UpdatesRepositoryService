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
        private readonly UpdateIdGenerator idGenerator = new();

        /// <summary>
        /// Конструктор экземпляров класса. Инициализирует объект необходимыми данными
        /// </summary>
        public UpdatesRepository(IRepositoryIndex repositoryIndex, string rootDirPath)
        {
            repository = repositoryIndex ?? throw new ArgumentNullException(nameof(repositoryIndex));
            RootDirectoryPath = rootDirPath;
        }

        /// <summary>
        /// Полный путь к директории, в которой находятся файлы обновлений
        /// </summary>
        public string? RootDirectoryPath { get; set; }

        /// <summary>
        /// Словарь, позволяющий определить расположение обновлений, содержащихся в репозитории
        /// </summary>
        private Dictionary<string, string>? LocationMap { get; set; }

        /// <inheritdoc/>
        public IEnumerable<UpdateModel> Updates
        {
            get
            {
                cacheAccessLock.EnterReadLock();
                try
                {
                    return repository.GetUpdates();
                }
                finally
                {
                    cacheAccessLock.ExitReadLock();
                }
            }
        }

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
            ArgumentNullException.ThrowIfNull(config, Resource.Message_TargetConfigNotSpecified);

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
                var newRepository = new RepositoryIndex();

                BuildRepository(directory, 0, newRepository, internalLocationMap);

                cacheAccessLock.EnterWriteLock();
                try
                {
                    repository = newRepository;
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

        private void BuildRepository(DirectoryInfo directory, int depth, IRepositoryIndex repository, ConcurrentDictionary<string, string> map)
        {
            if (depth++ > SEARCH_DEPTH)
            {
                return;
            }
            if (RepositoryParser.ValidateUpdateFiles(directory.FullName))
            {
                foreach (var cucFile in directory.EnumerateFiles("*.cuc"))
                {
                    if (RepositoryParser.TryParseСUCFile(cucFile.FullName, out UpdateModel? update))
                    {
                        // TODO переписать реализацию NewId с поддержкой потокозащищенности
                        update.Id = idGenerator.NewId(update);

                        if (map.TryAdd(update.Id!, directory.FullName))
                        {
                            repository.Add(update);
                        }
                    }
                }
            }

            Parallel.ForEach(directory.EnumerateDirectories(), subDir =>
               BuildRepository(subDir, depth, repository, map));
        }

        /// <inheritdoc/>
        public string GetLocation(string? updateId)
        {
            ArgumentNullException.ThrowIfNull(updateId, Resource.Message_TargetConfigIDdNotSpecified);

            cacheAccessLock.EnterReadLock();
            try
            {
                return LocationMap![updateId!];
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
                LocationMap?.Clear();
            }
            finally
            {
                cacheAccessLock.ExitWriteLock();
            }
        }

        /// <inheritdoc/>
        public IEnumerable<UpdateModel>? GetPotentialUpdates(ConfigurationModel? config)
        {
            ArgumentNullException.ThrowIfNull(config, Resource.Message_TargetConfigNotSpecified);

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
            ArgumentNullException.ThrowIfNull(config, Resource.Message_TargetConfigNotSpecified);

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
