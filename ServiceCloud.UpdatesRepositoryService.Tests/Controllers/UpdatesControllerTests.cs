using Microsoft.AspNetCore.Mvc;
using Moq;
using ServiceCloud.UpdatesRepository.BusinessLogic.Data;
using ServiceCloud.UpdatesRepositoryService.Controllers.v1;
using ServiceCloud.UpdatesRepositoryService.Responses.v1;

namespace ServiceCloud.UpdatesRepositoryService.Tests.Controllers
{
    [TestFixture]
    public partial class UpdatesControllerTests
    {
        private UpdatesController CreateTarget(TestContext context)
        {
            return new UpdatesController(context.MockRepository.Object);
        }

        [Test]
        public void UpdatesController_GetUpdatesCount_Test()
        {
            var context = new TestContext();
            var target = CreateTarget(context);

            var result = target.GetUpdatesCount();

            Assert.That(GetOkValue<UpdatesCountResponse>(result).Count, Is.EqualTo(context.UpdatesCount));
        }

        [Test]
        public void UpdatesController_GetUpdatesCount_InteractionWithRepository_Test()
        {
            var context = new TestContext();
            var target = CreateTarget(context);

            target.GetUpdatesCount();

            context.MockRepository.Verify(x => x.UpdatesCount, Times.Once);
        }

        [Test]
        public void UpdatesController_GetUpdatesList_Test()
        {
            var context = new TestContext();
            var target = CreateTarget(context);

            var result = target.GetUpdatesList();

            Assert.That(GetOkValue<IEnumerable<InfoBaseUpdate>>(result), Is.EquivalentTo(context.Updates));
        }

        [Test]
        public void UpdatesController_GetUpdatesList_InteractionWithRepository_Test()
        {
            var context = new TestContext();
            var target = CreateTarget(context);

            target.GetUpdatesList();

            context.MockRepository.Verify(x => x.Updates, Times.Once);
        }

        [Test]
        public void UpdatesController_ContainsUpdate_PositiveResult_Test()
        {
            var context = new TestContext();
            context.MockRepository
                .Setup(x => x.ContainsUpdate(It.IsAny<InfobaseConfig>()))
                .Returns(true);

            var target = CreateTarget(context);

            var result = target.ContainsUpdate(context.ExistingConfig);

            Assert.That(GetOkValue<ContainsUpdateResponse>(result).Contains, Is.True);
        }

        [Test]
        public void UpdatesController_ContainsUpdate_NegativeResult_Test()
        {
            var context = new TestContext();
            var target = CreateTarget(context);

            var result = target.ContainsUpdate(context.UnknownConfig);

            Assert.That(GetOkValue<ContainsUpdateResponse>(result).Contains, Is.False);
        }

        [Test]
        public void UpdatesController_ContainsUpdate_InteractionWithRepository_Test()
        {
            var context = new TestContext();
            var target = CreateTarget(context);

            target.ContainsUpdate(context.ExistingConfig);

            context.MockRepository.Verify(x => x.ContainsUpdate(context.ExistingConfig), Times.Once);
        }

        [Test]
        public void UpdatesController_GetUpdate_PositiveResult_Test()
        {
            var context = new TestContext();
            context.MockRepository
                .Setup(x => x.GetUpdate(It.IsAny<InfobaseConfig>(), It.IsAny<bool>()))
                .Returns(context.ExistingUpdate);

            var target = CreateTarget(context);

            var result = target.GetUpdate(context.ExistingConfig, false);

            Assert.That(GetOkValue<InfoBaseUpdate>(result), Is.EqualTo(context.ExistingUpdate));
        }

        [Test]
        public void UpdatesController_GetUpdate_NegativeResult_Test()
        {
            var context = new TestContext();
            context.MockRepository
                .Setup(x => x.GetUpdate(It.IsAny<InfobaseConfig>(), It.IsAny<bool>()))
                .Returns((InfoBaseUpdate?)null);

            var target = CreateTarget(context);

            var result = target.GetUpdate(context.UnknownConfig, false);

            Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public void UpdatesController_GetUpdate_InteractionWithRepository_Test()
        {
            var context = new TestContext();
            context.MockRepository
                .Setup(x => x.GetUpdate(It.IsAny<InfobaseConfig>(), It.IsAny<bool>()))
                .Returns(context.ExistingUpdate);

            var target = CreateTarget(context);

            target.GetUpdate(context.ExistingConfig, true);

            context.MockRepository.Verify(x => x.GetUpdate(context.ExistingConfig, true), Times.Once);
        }

        [Test]
        public void UpdatesController_GetPotentialUpdates_Test()
        {
            var context = new TestContext();
            context.MockRepository
                .Setup(x => x.GetPotentialUpdates(It.IsAny<InfobaseConfig>()))
                .Returns(context.Updates);

            var target = CreateTarget(context);

            var result = target.GetPotentialUpdates(context.ExistingConfig);

            Assert.That(GetOkValue<IEnumerable<InfoBaseUpdate>>(result), Is.EquivalentTo(context.Updates));
        }

        [Test]
        public void UpdatesController_GetPotentialUpdates_InteractionWithRepository_Test()
        {
            var context = new TestContext();
            var target = CreateTarget(context);

            target.GetPotentialUpdates(context.ExistingConfig);

            context.MockRepository.Verify(x => x.GetPotentialUpdates(context.ExistingConfig), Times.Once);
        }

        [Test]
        public void UpdatesController_GetLocation_PositiveResult_Test()
        {
            var context = new TestContext();
            context.MockRepository
                .Setup(x => x.GetLocation(It.IsAny<string>()))
                .Returns(context.Location);

            var target = CreateTarget(context);

            var result = target.GetLocation(context.ExistingUpdate.Id!);

            Assert.That(GetOkValue<UpdateLocationResponse>(result).Location, Is.EqualTo(context.Location));
        }

        [Test]
        public void UpdatesController_GetLocation_NegativeResult_Test()
        {
            var context = new TestContext();
            context.MockRepository
                .Setup(x => x.GetLocation(It.IsAny<string>()))
                .Returns((string)null!);

            var target = CreateTarget(context);

            var result = target.GetLocation("missing");

            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public void UpdatesController_GetLocation_InteractionWithRepository_Test()
        {
            var context = new TestContext();
            context.MockRepository
                .Setup(x => x.GetLocation(It.IsAny<string>()))
                .Returns(context.Location);

            var target = CreateTarget(context);

            target.GetLocation(context.ExistingUpdate.Id!);

            context.MockRepository.Verify(x => x.GetLocation(context.ExistingUpdate.Id), Times.Once);
        }

        private static T GetOkValue<T>(IActionResult result)
        {
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            return GetOkValue<T>((OkObjectResult)result);
        }

        private static T GetOkValue<T>(ActionResult<T> result)
        {
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            return GetOkValue<T>((OkObjectResult)result.Result!);
        }

        private static T GetOkValue<T>(OkObjectResult result)
        {
            Assert.That(result.Value, Is.InstanceOf<T>());
            return (T)result.Value!;
        }
    }
}
