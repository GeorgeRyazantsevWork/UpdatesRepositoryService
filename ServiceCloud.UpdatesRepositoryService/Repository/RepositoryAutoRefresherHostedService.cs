namespace ServiceCloud.UpdatesRepositoryService.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public class RepositoryAutoRefresherHostedService : BackgroundService
    {
        private IUpdatesRepository repository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatesRepository"></param>
        /// <param name="refreshInterval"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public RepositoryAutoRefresherHostedService(IUpdatesRepository updatesRepository, TimeSpan refreshInterval = default)
        {
            repository = updatesRepository ?? throw new ArgumentNullException(nameof(updatesRepository));
            RefreshInterval = refreshInterval == default ? TimeSpan.FromMinutes(30) : refreshInterval;
        }

        /// <summary>
        /// Интервал между запусками сканирования репозитория
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Значение не может быть меньше нуля</exception>
        public TimeSpan RefreshInterval
        {
            get;
            set => field = value > TimeSpan.Zero ? value : throw new ArgumentOutOfRangeException(nameof(RefreshInterval));
        }

        /// <inheritdoc/>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await repository.Refresh();
                using var timer = new PeriodicTimer(RefreshInterval);

                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    await repository.Refresh();
                }
            }
            catch (Exception ex)// TODO Добавить логирование ошибки
            {
                repository.Clear();
            }
        }
    }
}
