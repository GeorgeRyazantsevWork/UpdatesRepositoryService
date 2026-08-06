using ServiceCloud.UpdatesRepositoryService.Helpers;
using ServiceCloud.UpdatesRepositoryService.Models;
using System.Reflection.Emit;

namespace ServiceCloud.UpdatesRepositoryService.Repository
{
    /// <summary>
    /// Описывает репозиторий обновлений для конфигураций 1С
    /// </summary>
    public class UpdatesRepository : IUpdatesRepository
    {
        private readonly SemaphoreSlim refreshLock = new(1, 1);
        private const int SEARCH_DEPTH = 64;
        private IRepositoryIndex repository;

        /// <summary>
        /// 
        /// </summary>
        public UpdatesRepository(IRepositoryIndex repositoryIndex, string rootDirPath)
        {
            repository = repositoryIndex ?? throw new ArgumentNullException(nameof(repositoryIndex));
            RootDirectoryPath = rootDirPath;
            LocationMap = new Dictionary<string, string>();
        }

        /// <summary>
        /// Полный путь к директории, в которой находятся файлы обновлений
        /// </summary>
        public string? RootDirectoryPath
        {
            get;
            set;
        }

        /// <summary>
        /// Словарь, позволяющий определить расположение обновлений, содержащихся в репозитории
        /// </summary>
        protected Dictionary<string, string> LocationMap { get; set; }

        /// <inheritdoc/>
        public IEnumerable<UpdateModel> Updates => repository.GetUpdates();

        /// <inheritdoc/>
        public int UpdatesCount => repository.Count;

        /// <inheritdoc/>
        public bool ContainsUpdate(ConfigurationModel? config)
        {
            if (config != null)
            {
                return repository.Contains(config);
            }

            return false;

            // TODO проверять из обоих словарей? + добавить cacheAccessLock
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
                BuildRepository(directory, 0);
            }
            catch (ArgumentException e)
            {
                throw new InvalidOperationException(
                    $"Недопустимое значение свойства \"{nameof(RootDirectoryPath)}\".", e);
            }
            finally
            {
                refreshLock.Release();
            }
        }

        private void BuildRepository(DirectoryInfo directory, int depth)
        {
            // TODO Реализовать потокозащищенность + parallel.foreach
            var internalLocationMap = new Dictionary<string, string>();
            depth++;
            if (depth > SEARCH_DEPTH)
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

                        if (internalLocationMap.TryAdd(update.Id!, directory.FullName))
                        {
                            repository.Add(update);
                        }
                    }
                }
            }
            LocationMap = internalLocationMap; // TODO нужен ли дубликат вообще
        }


        /// <inheritdoc/>
        public string GetLocation(string? updateId)
        {
            return LocationMap[updateId!];
        }

        /// <inheritdoc/>
        public IEnumerable<UpdateModel> GetPotentialUpdates(ConfigurationModel? config)
        {
            return repository.GetPotentialUpdates(config);
        }

        /// <inheritdoc/>
        public UpdateModel GetUpdate(ConfigurationModel? config, bool transitive)
        {
            return repository.GetUpdate(config, transitive);
        }
    }
}
