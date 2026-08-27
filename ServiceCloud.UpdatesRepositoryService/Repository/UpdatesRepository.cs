using Microsoft.Extensions.Options;
using ServiceCloud.UpdatesRepositoryService.Helpers;
using ServiceCloud.UpdatesRepositoryService.Models;
using ServiceCloud.UpdatesRepositoryService.Properties;
using System.Collections.Concurrent;

namespace ServiceCloud.UpdatesRepositoryService.Repository
{
    /// <summary>
    /// Описывает репозиторий обновлений для конфигураций 1С
    /// </summary>
    public class UpdatesRepository : IUpdatesRepository, IDisposable
    {
        private readonly ReaderWriterLockSlim cacheAccessLock = new(LockRecursionPolicy.NoRecursion);
        private IRepositoryIndex repository;
        private const int SEARCH_DEPTH = 64;
        private const int MAX_SUBLEVEL_CONCURRENCY = 10;
        private bool disposed;

        /// <summary>
        /// Конструктор экземпляров класса. Инициализирует объект необходимыми данными
        /// </summary>
        /// <param name="options">Настройки репозитория обновлений</param>
        /// <exception cref="ArgumentNullException">Не задан один из обязательных параметров</exception>
        public UpdatesRepository(IOptions<RepositoryOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);

            repository = new RepositoryIndex();
            RootDirectoryPath = options.Value.Path;
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
        /// <exception cref="Exception">Не удалось провести анализ "корневого" каталога</exception>
        public void Refresh()
        {
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

            var subDirs = directory.EnumerateDirectories();

            Parallel.ForEach(
                directory.EnumerateDirectories(),
                new ParallelOptions { MaxDegreeOfParallelism = MAX_SUBLEVEL_CONCURRENCY }, // ограничение параллельности: каждый конкретный вызов Parallel.ForEach                                                                                           выполняет
                                                                                           // выполняет максимум заданное число параллельных операций
                subDir => BuildRepository(subDir, depth, parsedUpdates)); 
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


        #region Члены IDisposable

        /// <inheritdoc/>
        public void Dispose()
        {
            if (disposed)
            {
                return;
            }
            cacheAccessLock.Dispose();
            disposed = true;
        }

        #endregion
    }
}
