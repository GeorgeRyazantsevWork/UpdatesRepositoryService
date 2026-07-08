namespace ServiceCloud.UpdatesRepositoryService.Repository
{
    public class RepositoryScannerHostedService(RepositoryScanner repositoryScanner) : BackgroundService
    {
         private readonly RepositoryScanner scanner = repositoryScanner;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
