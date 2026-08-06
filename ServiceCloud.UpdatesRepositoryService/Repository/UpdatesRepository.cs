using ServiceCloud.UpdatesRepositoryService.Models;
using System.Reflection.Emit;

namespace ServiceCloud.UpdatesRepositoryService.Repository
{
    /// <summary>
    /// Описывает репозиторий обновлений для конфигураций 1С
    /// </summary>
    public class UpdatesRepository : IUpdatesRepository
    {
        private IRepositoryIndex repository;

        /// <summary>
        /// 
        /// </summary>
        public UpdatesRepository(IRepositoryIndex repositoryIndex, string rootDirPath)
        {
            repository = repositoryIndex ?? throw new ArgumentNullException(nameof(repositoryIndex));
            RootDirectoryPath = rootDirPath;
        }

        /// <summary>
        /// Полный путь к директории, в которой находятся файлы обновлений
        /// </summary>
        public string? RootDirectoryPath
        {
            get;
            set;
        }

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
        public void Refresh()
        {                  
        }



        /// <inheritdoc/>
        public string GetLocation(string? updateId)
        {
            throw new NotImplementedException();
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
