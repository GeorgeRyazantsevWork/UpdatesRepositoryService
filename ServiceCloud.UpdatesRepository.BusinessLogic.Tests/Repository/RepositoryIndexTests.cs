using ServiceCloud.UpdatesRepository.BusinessLogic.Data;
using ServiceCloud.UpdatesRepository.BusinessLogic.Repository;

namespace ServiceCloud.UpdatesRepository.BusinessLogic.Tests.Repository
{
    [TestFixture]
    public class RepositoryIndexTests : IRepositoryIndexTests
    {
        protected override IRepositoryIndex CreateTargetObject()
        {
            return new RepositoryIndex();
        }

        protected IRepositoryIndex CreateTargetObject(UpdatesRepositoryTestContext context)
        {
            var target = new RepositoryIndex();
            foreach (var update in context.ExistingUpdates)
            {
                target.Add(update);
            }

            return target;
        }

        [Test]
        public void RepositoryIndex_Add_IncreasesCount_Test()
        {
            var context = new UpdatesRepositoryTestContext();
            var target = CreateTargetObject();

            target.Add(context.ExistingUpdates[0]);

            Assert.That(target.Count, Is.EqualTo(1));
            Assert.That(target.GetUpdates(), Is.EquivalentTo(new[] { context.ExistingUpdates[0] }));
        }

        [Test]
        public void RepositoryIndex_Add_DuplicateUpdate_Ignored_Test()
        {
            var context = new UpdatesRepositoryTestContext();
            var target = CreateTargetObject();
            var update = context.ExistingUpdates[0];

            target.Add(update);
            target.Add(update);

            Assert.That(target.Count, Is.EqualTo(1));
        }

        [Test]
        public void RepositoryIndex_Contains_PositiveResult_Test()
        {
            var context = new UpdatesRepositoryTestContext();
            var target = CreateTargetObject(context);

            Assert.That(target.Contains(context.ExistingConfigs[0]), Is.True);
        }

        [Test]
        public void RepositoryIndex_Contains_NegativeResult_Test()
        {
            var context = new UpdatesRepositoryTestContext();
            var target = CreateTargetObject(context);

            Assert.That(target.Contains(context.UnknownConfiguration), Is.False);
        }

        [Test]
        [TestCaseSource(typeof(UpdatesRepositoryTestContext), nameof(UpdatesRepositoryTestContext.InsufficientConfigs))]
        public void RepositoryIndex_Contains_InsufficientData_Test(InfobaseConfig config)
        {
            var target = CreateTargetObject();

            Assert.That(() => target.Contains(config), Throws.ArgumentException);
        }

        [Test]
        public void RepositoryIndex_GetUpdate_NotTransitiveUpdateFound_Test()
        {
            var context = new UpdatesRepositoryTestContext();
            var target = CreateTargetObject(context);

            var result = target.GetUpdate(context.ExistingConfigs[0], false);

            Assert.That(result, Is.EqualTo(context.ExistingUpdates[0]));
        }

        [Test]
        public void RepositoryIndex_GetUpdate_NotTransitiveUpdateNotFound_Test()
        {
            var context = new UpdatesRepositoryTestContext();
            var target = CreateTargetObject(context);

            var result = target.GetUpdate(context.UnknownConfiguration, false);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void RepositoryIndex_GetUpdate_TransitiveUpdateFound_Test()
        {
            var context = new UpdatesRepositoryTestContext();
            var target = CreateTargetObject(context);

            var result = target.GetUpdate(context.ExistingConfigs[4], true);

            Assert.That(result, Is.EqualTo(context.ExistingUpdates[4]));
        }

        [Test]
        public void RepositoryIndex_GetUpdate_TransitiveUpdateNotFound_Test()
        {
            var context = new UpdatesRepositoryTestContext();
            var target = CreateTargetObject(context);

            var result = target.GetUpdate(context.ExistingConfigs[0], true);

            Assert.That(result, Is.Null);
        }

        [Test]
        [TestCaseSource(typeof(UpdatesRepositoryTestContext), nameof(UpdatesRepositoryTestContext.InsufficientConfigs))]
        public void RepositoryIndex_GetUpdate_InsufficientData_Test(InfobaseConfig config)
        {
            var target = CreateTargetObject();

            Assert.That(() => target.GetUpdate(config, false), Throws.ArgumentException);
        }

        [TestCaseSource(typeof(UpdatesRepositoryTestContext), nameof(UpdatesRepositoryTestContext.PotentialUpdatesCorrectCases))]
        public void RepositoryIndex_GetPotentialUpdates_Result_Test(List<InfoBaseUpdate> updates,
                                                                   InfobaseConfig config,
                                                                   IEnumerable<InfoBaseUpdate> expected)
        {
            var target = new RepositoryIndex();
            foreach (var update in updates)
            {
                target.Add(update);
            }

            var actual = target.GetPotentialUpdates(config);

            Assert.That(actual, Is.EquivalentTo(expected));
        }

        [Test]
        [TestCaseSource(typeof(UpdatesRepositoryTestContext), nameof(UpdatesRepositoryTestContext.InsufficientConfigs))]
        public void RepositoryIndex_GetPotentialUpdates_InsufficientData_Test(InfobaseConfig config)
        {
            var target = CreateTargetObject();

            Assert.That(() => target.GetPotentialUpdates(config), Throws.ArgumentException);
        }

        [Test]
        public void RepositoryIndex_Clear_Test()
        {
            var context = new UpdatesRepositoryTestContext();
            var target = CreateTargetObject(context);

            target.Clear();

            Assert.That(target.Count, Is.EqualTo(0));
            Assert.That(target.GetUpdates(), Is.Empty);
            Assert.That(target.Contains(context.ExistingConfigs[0]), Is.False);
        }
    }
}
