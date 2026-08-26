using ServiceCloud.UpdatesRepository.BusinessLogic.Helpers;
using ServiceCloud.UpdatesRepository.BusinessLogic.Models;
using ServiceCloud.UpdatesRepository.BusinessLogic.Properties;
using System.Collections.Concurrent;

namespace ServiceCloud.UpdatesRepository.BusinessLogic.Repository
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
        /// <param name="repositoryIndex">Индекс репозитория обновлений</param>
        /// <param name="rootDirectoryPath">Путь к директории, в которой находятся файлы обновлений</param>
        /// <exception cref="ArgumentNullException">Не задан один из обязательных параметров</exception>
        public UpdatesRepository(IRepositoryIndex repositoryIndex, string rootDirectoryPath)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(rootDirectoryPath);

            repository = repositoryIndex ?? throw new ArgumentNullException(nameof(repositoryIndex));
            RootDirectoryPath = rootDirectoryPath;
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
        /// <exception cref="InvalidOperationException">Путь к "корневому" каталогу не задан или содержит недопустимые знаки</exception>
        /// <exception cref="PathTooLongException">Путь к "корневому" каталогу слишком длинный</exception>
        /// <exception cref="Exception">Не удалось провести анализ "корневого" каталога</exception>
        public async Task Refresh()
        {
            await refreshLock.WaitAsync();

            try
            {
                var directory = new DirectoryInfo(RootDirectoryPath ?? Directory.GetCurrentDirectory());
                var parsedUpdates = new ConcurrentBag<(UpdateModel Update, string Directory)>();

                BuildRepository(directory, 0, parsedUpdates);

                var newIndex = new RepositoryIndex();
                var newLocationMap = new Dictionary<string, string>(parsedUpdates.Count);

                using var idGenerator = new UpdateIdGenerator();
                foreach (var (update, updateDirectory) in parsedUpdates)
                {
                    update.Id = idGenerator.NewId(update);

                    if (newLocationMap.TryAdd(update.Id!, updateDirectory))
                    {
                        newIndex.Add(update);
                    }
                }

                cacheAccessLock.EnterWriteLock();
                try
                {
                    repository = newIndex;
                    LocationMap = newLocationMap;
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

        private void BuildRepository(DirectoryInfo directory, int depth,
            ConcurrentBag<(UpdateModel Update, string Directory)> parsedUpdates)
        {
            if (depth++ > SEARCH_DEPTH)
            {
                return;
            }

            if (RepositoryParser.ValidateUpdateFiles(directory.FullName))
            {
                ParseUpdateDirectory(directory, parsedUpdates);
                return;
            }

            Parallel.ForEach(directory.EnumerateDirectories(), subDir =>
                BuildRepository(subDir, depth, parsedUpdates));
        }

        private static void ParseUpdateDirectory(DirectoryInfo directory,
            ConcurrentBag<(UpdateModel Update, string Directory)> parsedUpdates)
        {
            foreach (var cucFile in directory.EnumerateFiles("*.cuc"))
            {
                if (RepositoryParser.TryParseСUCFile(cucFile.FullName, out UpdateModel? update))
                {
                    parsedUpdates.Add((update, directory.FullName));
                }
            }
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
        public IEnumerable<UpdateModel> GetPotentialUpdates(ConfigurationModel? config)
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
