using Microsoft.Extensions.Options;
using ServiceCloud.UpdatesRepository.BusinessLogic.Properties;
using ServiceCloud.UpdatesRepository.BusinessLogic.Repository;
using ServiceCloud.UpdatesRepositoryService.Properties;
using System.Diagnostics;

namespace ServiceCloud.UpdatesRepositoryService
{
    /// <summary>
    /// Фоновый сервис, выполняющий обновление репозитория с заданной периодичностью
    /// </summary>
    public class RepositoryAutoRefresherHostedService : BackgroundService
    {
        private IUpdatesRepository repository;
        private ILogger logger;

        /// <summary>
        /// Конструктор экземпляров класса. Инициализирует объект необходимыми данными
        /// </summary>
        /// <param name="updatesRepository">Репозиторий с обновлениями конфигураций ИБ</param>
        /// <param name="logger">Регистратор событий работы сервиса</param>
        /// <param name="options">Настройки репозитория обновлений</param>
        /// <exception cref="ArgumentNullException">Не задан обязательный аргумент</exception>
        public RepositoryAutoRefresherHostedService(IUpdatesRepository updatesRepository,
            ILogger<RepositoryAutoRefresherHostedService> logger, IOptions<RepositoryServiceOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);

            repository = updatesRepository ?? throw new ArgumentNullException(nameof(updatesRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            var refreshInterval = options.Value.RefreshInterval;
            RefreshInterval = refreshInterval == default
                ? RepositoryServiceOptions.DefaultRefreshInterval
                : refreshInterval;
        }

        /// <summary>
        /// Интервал между запусками обновления репозитория
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Значение не может быть меньше нуля</exception>
        public TimeSpan RefreshInterval
        {
            get;
            set => field = value > TimeSpan.Zero ?
                 value :
                 throw new ArgumentOutOfRangeException(nameof(RefreshInterval));
        }

        /// <inheritdoc/>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await RefreshRepository();
                using var timer = new PeriodicTimer(RefreshInterval);

                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    await RefreshRepository();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, Resource.LogMessage_RefreshRepositoryError);
                repository.Clear();
            }
        }

        private async Task RefreshRepository()
        {
            logger.LogInformation(Resource.LogMessage_RefreshingWasStarted);

            var stopwatch = Stopwatch.StartNew();
            try
            {
                await repository.Refresh();
                stopwatch.Stop();
                logger.LogInformation(string.Format(Resource.LogMessage_RefreshWasFinished, stopwatch.ElapsedMilliseconds));
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                logger.LogError(ex, string.Format(Resource.LogMessage_RefreshError, stopwatch.ElapsedMilliseconds));
                throw;
            }
        }
    }
}
