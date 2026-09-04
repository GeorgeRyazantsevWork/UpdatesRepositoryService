using Grpc.Core;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using ServiceCloud.UpdatesRepository.BusinessLogic.Data;
using ServiceCloud.UpdatesRepository.BusinessLogic.Repository;
using ServiceCloud.UpdatesRepositoryService.Grpc;
using DataUpdate = ServiceCloud.UpdatesRepository.BusinessLogic.Data.InfoBaseUpdate;
using GrpcUpdate = ServiceCloud.UpdatesRepositoryService.Grpc.InfoBaseUpdate;

namespace ServiceCloud.UpdatesRepositoryService.Tests.Grpc
{
    [TestFixture]
    public partial class UpdatesRepositoryGrpcServiceTests
    {
        private static UpdatesRepositoryGrpcService CreateTarget(TestContext context)
        {
            return new UpdatesRepositoryGrpcService(
                context.MockRepository.Object,
                NullLogger<UpdatesRepositoryGrpcService>.Instance);
        }

        [Test]
        public void UpdatesRepositoryGrpcService_Constructor_WithNullRepository_Test()
        {
            Assert.That(
                () => new UpdatesRepositoryGrpcService(null, NullLogger<UpdatesRepositoryGrpcService>.Instance),
                Throws.ArgumentNullException);
        }

        [Test]
        public void UpdatesRepositoryGrpcService_Constructor_WithNullLogger_Test()
        {
            Assert.That(
                () => new UpdatesRepositoryGrpcService(Mock.Of<IUpdatesRepository>(), null),
                Throws.ArgumentNullException);
        }

        [Test]
        public async Task UpdatesRepositoryGrpcService_GetUpdatesCount_Test()
        {
            var context = new TestContext();
            var target = CreateTarget(context);

            var response = await target.GetUpdatesCount(new UpdatesCountRequest(), TestServerCallContext.Create());

            Assert.That(response.Count, Is.EqualTo(context.Updates.Count));
        }

        [Test]
        public void UpdatesRepositoryGrpcService_GetUpdatesCount_ErrorHandling_Test()
        {
            var context = new TestContext();
            context.MockRepository.Setup(x => x.UpdatesCount).Throws(new InvalidOperationException());
            var target = CreateTarget(context);

            var exception = Assert.ThrowsAsync<RpcException>(
                () => target.GetUpdatesCount(new UpdatesCountRequest(), TestServerCallContext.Create()));

            Assert.That(exception!.StatusCode, Is.EqualTo(StatusCode.Internal));
        }

        [Test]
        public async Task UpdatesRepositoryGrpcService_GetUpdatesList_Test()
        {
            var context = new TestContext();
            var stream = CreateStreamWriter();
            var target = CreateTarget(context);

            await target.GetUpdatesList(new UpdatesListRequest(), stream.Writer, TestServerCallContext.Create());

            Assert.That(stream.Written, Is.EqualTo(new[] { context.ExistingUpdateMessage }));
        }

        [Test]
        public void UpdatesRepositoryGrpcService_GetUpdatesList_ErrorHandling_Test()
        {
            var context = new TestContext();
            context.MockRepository.Setup(x => x.Updates).Throws(new InvalidOperationException());
            var target = CreateTarget(context);

            var exception = Assert.ThrowsAsync<RpcException>(
                () => target.GetUpdatesList(new UpdatesListRequest(), CreateStreamWriter().Writer, TestServerCallContext.Create()));

            Assert.That(exception!.StatusCode, Is.EqualTo(StatusCode.Internal));
        }

        [Test]
        public async Task UpdatesRepositoryGrpcService_ContainsUpdate_PositiveResult_Test()
        {
            var context = new TestContext();
            context.MockRepository.Setup(x => x.ContainsUpdate(It.IsAny<InfobaseConfig>())).Returns(true);
            var target = CreateTarget(context);

            var response = await target.ContainsUpdate(context.ExistingConfigMessage, TestServerCallContext.Create());

            Assert.That(response.Contains, Is.True);
        }

        [Test]
        public async Task UpdatesRepositoryGrpcService_ContainsUpdate_NegativeResult_Test()
        {
            var context = new TestContext();
            var target = CreateTarget(context);

            var response = await target.ContainsUpdate(context.UnknownConfig.ToGrpcMessage(), TestServerCallContext.Create());

            Assert.That(response.Contains, Is.False);
        }

        [Test]
        public async Task UpdatesRepositoryGrpcService_ContainsUpdate_InteractionWithRepository_Test()
        {
            var context = new TestContext();
            var target = CreateTarget(context);

            await target.ContainsUpdate(context.ExistingConfigMessage, TestServerCallContext.Create());

            context.MockRepository.Verify(x => x.ContainsUpdate(context.ExistingConfig), Times.Once);
        }

        [Test]
        public void UpdatesRepositoryGrpcService_ContainsUpdate_InvalidArgument_Test()
        {
            var context = new TestContext();
            context.MockRepository
                .Setup(x => x.ContainsUpdate(It.IsAny<InfobaseConfig>()))
                .Throws(new ArgumentException());
            var target = CreateTarget(context);

            var exception = Assert.ThrowsAsync<RpcException>(
                () => target.ContainsUpdate(context.ExistingConfigMessage, TestServerCallContext.Create()));

            Assert.That(exception!.StatusCode, Is.EqualTo(StatusCode.InvalidArgument));
        }

        [Test]
        public async Task UpdatesRepositoryGrpcService_GetUpdate_PositiveResult_Test()
        {
            var context = new TestContext();
            context.MockRepository
                .Setup(x => x.GetUpdate(It.IsAny<InfobaseConfig>(), It.IsAny<bool>()))
                .Returns(context.ExistingUpdate);
            var target = CreateTarget(context);

            var response = await target.GetUpdate(
                new InfoBaseUpdateRequest { Transitive = true, Config = context.ExistingConfigMessage },
                TestServerCallContext.Create());

            Assert.That(response, Is.EqualTo(context.ExistingUpdateMessage));
        }

        [Test]
        public void UpdatesRepositoryGrpcService_GetUpdate_NotFoundWhenNull_Test()
        {
            var context = new TestContext();
            context.MockRepository
                .Setup(x => x.GetUpdate(It.IsAny<InfobaseConfig>(), It.IsAny<bool>()))
                .Returns((DataUpdate?)null);
            var target = CreateTarget(context);

            var exception = Assert.ThrowsAsync<RpcException>(
                () => target.GetUpdate(
                    new InfoBaseUpdateRequest { Config = context.UnknownConfig.ToGrpcMessage() },
                    TestServerCallContext.Create()));

            Assert.That(exception!.StatusCode, Is.EqualTo(StatusCode.NotFound));
        }

        [Test]
        public void UpdatesRepositoryGrpcService_GetUpdate_NotFoundWhenMissing_Test()
        {
            var context = new TestContext();
            context.MockRepository
                .Setup(x => x.GetUpdate(It.IsAny<InfobaseConfig>(), It.IsAny<bool>()))
                .Throws(new KeyNotFoundException());
            var target = CreateTarget(context);

            var exception = Assert.ThrowsAsync<RpcException>(
                () => target.GetUpdate(
                    new InfoBaseUpdateRequest { Config = context.ExistingConfigMessage },
                    TestServerCallContext.Create()));

            Assert.That(exception!.StatusCode, Is.EqualTo(StatusCode.NotFound));
        }

        [Test]
        public void UpdatesRepositoryGrpcService_GetUpdate_InvalidArgument_Test()
        {
            var context = new TestContext();
            context.MockRepository
                .Setup(x => x.GetUpdate(It.IsAny<InfobaseConfig>(), It.IsAny<bool>()))
                .Throws(new ArgumentException());
            var target = CreateTarget(context);

            var exception = Assert.ThrowsAsync<RpcException>(
                () => target.GetUpdate(
                    new InfoBaseUpdateRequest { Config = context.ExistingConfigMessage },
                    TestServerCallContext.Create()));

            Assert.That(exception!.StatusCode, Is.EqualTo(StatusCode.InvalidArgument));
        }

        [Test]
        public async Task UpdatesRepositoryGrpcService_GetUpdate_InteractionWithRepository_Test()
        {
            var context = new TestContext();
            context.MockRepository
                .Setup(x => x.GetUpdate(It.IsAny<InfobaseConfig>(), It.IsAny<bool>()))
                .Returns(context.ExistingUpdate);
            var target = CreateTarget(context);

            await target.GetUpdate(
                new InfoBaseUpdateRequest { Transitive = true, Config = context.ExistingConfigMessage },
                TestServerCallContext.Create());

            context.MockRepository.Verify(x => x.GetUpdate(context.ExistingConfig, true), Times.Once);
        }

        [Test]
        public async Task UpdatesRepositoryGrpcService_GetPotentialUpdatesList_Test()
        {
            var context = new TestContext();
            context.MockRepository
                .Setup(x => x.GetPotentialUpdates(It.IsAny<InfobaseConfig>()))
                .Returns(context.Updates);
            var stream = CreateStreamWriter();
            var target = CreateTarget(context);

            await target.GetPotentialUpdatesList(context.ExistingConfigMessage, stream.Writer, TestServerCallContext.Create());

            Assert.That(stream.Written, Is.EqualTo(new[] { context.ExistingUpdateMessage }));
        }

        [Test]
        public async Task UpdatesRepositoryGrpcService_GetPotentialUpdatesList_InteractionWithRepository_Test()
        {
            var context = new TestContext();
            var target = CreateTarget(context);

            await target.GetPotentialUpdatesList(
                context.ExistingConfigMessage,
                CreateStreamWriter().Writer,
                TestServerCallContext.Create());

            context.MockRepository.Verify(x => x.GetPotentialUpdates(context.ExistingConfig), Times.Once);
        }

        [Test]
        public async Task UpdatesRepositoryGrpcService_GetLocation_PositiveResult_Test()
        {
            var context = new TestContext();
            context.MockRepository.Setup(x => x.GetLocation(It.IsAny<string>())).Returns(context.Location);
            var target = CreateTarget(context);

            var response = await target.GetLocation(
                new InfoBaseUpdateLocationRequest { UpdateId = context.ExistingUpdate.Id },
                TestServerCallContext.Create());

            Assert.That(response.Location, Is.EqualTo(context.Location));
        }

        [Test]
        public void UpdatesRepositoryGrpcService_GetLocation_NotFound_Test()
        {
            var context = new TestContext();
            context.MockRepository.Setup(x => x.GetLocation(It.IsAny<string>())).Throws(new KeyNotFoundException());
            var target = CreateTarget(context);

            var exception = Assert.ThrowsAsync<RpcException>(
                () => target.GetLocation(
                    new InfoBaseUpdateLocationRequest { UpdateId = "missing" },
                    TestServerCallContext.Create()));

            Assert.That(exception!.StatusCode, Is.EqualTo(StatusCode.NotFound));
        }

        [Test]
        public void UpdatesRepositoryGrpcService_GetLocation_InvalidArgument_Test()
        {
            var context = new TestContext();
            context.MockRepository.Setup(x => x.GetLocation(It.IsAny<string>())).Throws(new ArgumentException());
            var target = CreateTarget(context);

            var exception = Assert.ThrowsAsync<RpcException>(
                () => target.GetLocation(
                    new InfoBaseUpdateLocationRequest { UpdateId = string.Empty },
                    TestServerCallContext.Create()));

            Assert.That(exception!.StatusCode, Is.EqualTo(StatusCode.InvalidArgument));
        }

        [Test]
        public async Task UpdatesRepositoryGrpcService_GetLocation_InteractionWithRepository_Test()
        {
            var context = new TestContext();
            context.MockRepository.Setup(x => x.GetLocation(It.IsAny<string>())).Returns(context.Location);
            var target = CreateTarget(context);

            await target.GetLocation(
                new InfoBaseUpdateLocationRequest { UpdateId = context.ExistingUpdate.Id },
                TestServerCallContext.Create());

            context.MockRepository.Verify(x => x.GetLocation(context.ExistingUpdate.Id), Times.Once);
        }

        private static StreamWriterContext CreateStreamWriter()
        {
            var written = new List<GrpcUpdate>();
            var mock = new Mock<IServerStreamWriter<GrpcUpdate>>();
            mock.Setup(x => x.WriteAsync(It.IsAny<GrpcUpdate>()))
                .Callback<GrpcUpdate>(written.Add)
                .Returns(Task.CompletedTask);

            return new StreamWriterContext(mock.Object, written);
        }

        private sealed record StreamWriterContext(
            IServerStreamWriter<GrpcUpdate> Writer,
            List<GrpcUpdate> Written);
    }
}
