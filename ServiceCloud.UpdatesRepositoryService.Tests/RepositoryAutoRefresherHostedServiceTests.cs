using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using ServiceCloud.UpdatesRepository.BusinessLogic.Repository;
using ServiceCloud.UpdatesRepositoryService.Properties;

namespace ServiceCloud.UpdatesRepositoryService.Tests
{
    [TestFixture]
    public class RepositoryAutoRefresherHostedServiceTests
    {
        private static RepositoryAutoRefresherHostedService CreateTarget(
            IUpdatesRepository? repository = null,
            RepositoryServiceOptions? options = null)
        {
            return new RepositoryAutoRefresherHostedService(
                repository ?? Mock.Of<IUpdatesRepository>(),
                NullLogger<RepositoryAutoRefresherHostedService>.Instance,
                Options.Create(options ?? new RepositoryServiceOptions()));
        }

        [Test]
        public void RepositoryAutoRefresherHostedService_Constructor_NullRepository_Test()
        {
            Assert.That(() => new RepositoryAutoRefresherHostedService(
                    null!,
                    NullLogger<RepositoryAutoRefresherHostedService>.Instance,
                    Options.Create(new RepositoryServiceOptions())),
                Throws.ArgumentNullException);
        }

        [Test]
        public void RepositoryAutoRefresherHostedService_Constructor_NullLogger_Test()
        {
            Assert.That(() => new RepositoryAutoRefresherHostedService(
                    Mock.Of<IUpdatesRepository>(),
                    null!,
                    Options.Create(new RepositoryServiceOptions())),
                Throws.ArgumentNullException);
        }

        [Test]
        public void RepositoryAutoRefresherHostedService_Constructor_NullOptions_Test()
        {
            Assert.That(() => new RepositoryAutoRefresherHostedService(
                    Mock.Of<IUpdatesRepository>(),
                    NullLogger<RepositoryAutoRefresherHostedService>.Instance,
                    null!),
                Throws.ArgumentNullException);
        }

        [Test]
        public void RepositoryAutoRefresherHostedService_RefreshInterval_DefaultValue_Test()
        {
            var target = CreateTarget(options: new RepositoryServiceOptions { RefreshInterval = default });

            Assert.That(target.RefreshInterval, Is.EqualTo(RepositoryServiceOptions.DefaultRefreshInterval));
        }

        [TestCase(2)]
        [TestCase(1)]
        public void RepositoryAutoRefresherHostedService_RefreshInterval_SetValue_Test(int expected)
        {
            var target = CreateTarget();

            target.RefreshInterval = TimeSpan.FromSeconds(expected);

            Assert.That(target.RefreshInterval, Is.EqualTo(TimeSpan.FromSeconds(expected)));
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void RepositoryAutoRefresherHostedService_RefreshInterval_SetIncorrectValue_Test(int expected)
        {
            var target = CreateTarget();

            Assert.That(() => target.RefreshInterval = TimeSpan.FromSeconds(expected),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public async Task RepositoryAutoRefresherHostedService_Start_Refresh_Test()
        {
            var refreshStarted = new TaskCompletionSource();
            var mockRepository = new Mock<IUpdatesRepository>();
            mockRepository
                .Setup(x => x.Refresh())
                .Callback(() => refreshStarted.TrySetResult());

            var target = CreateTarget(mockRepository.Object);
            target.RefreshInterval = TimeSpan.FromHours(1);

            await target.StartAsync(CancellationToken.None);
            await refreshStarted.Task.WaitAsync(TimeSpan.FromSeconds(2));
            await target.StopAsync(CancellationToken.None);

            mockRepository.Verify(x => x.Refresh(), Times.AtLeastOnce);
        }

        [Test]
        public async Task RepositoryAutoRefresherHostedService_RefreshError_ClearsRepository_Test()
        {
            var cleared = new TaskCompletionSource();
            var mockRepository = new Mock<IUpdatesRepository>();
            mockRepository.Setup(x => x.Refresh()).Throws(new Exception("refresh failed"));
            mockRepository.Setup(x => x.Clear()).Callback(() => cleared.TrySetResult());

            var target = CreateTarget(mockRepository.Object);
            target.RefreshInterval = TimeSpan.FromHours(1);

            await target.StartAsync(CancellationToken.None);
            await cleared.Task.WaitAsync(TimeSpan.FromSeconds(2));
            await target.StopAsync(CancellationToken.None);

            mockRepository.Verify(x => x.Clear(), Times.Once);
        }
    }
}
