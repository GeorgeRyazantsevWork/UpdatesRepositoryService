using ServiceCloud.UpdatesRepository.BusinessLogic.Data;

namespace ServiceCloud.UpdatesRepository.BusinessLogic.Repository.Tests
{
    [TestFixture]
    public class UpdatesRepositoryTests : IUpdatesRepositoryTests
    {
        protected override IUpdatesRepository CreateTargetObject()
        {
            return new UpdatesRepository(new RepositoryIndex(), ".");
        }

        protected UpdatesRepository CreateTargetObject(UpdatesRepositoryTestContext context)
        {
            return new UpdatesRepositoryStub(new RepositoryIndex(), context);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void UpdatesRepository_Constructor_Test(string? value)
        {
            Assert.That(() => new UpdatesRepository(new RepositoryIndex(), value!), Throws.InstanceOf<ArgumentException>());
        }

        [TestCase(@"C:\Repo")]
        [TestCase(@"\\fs\Repo")]
        [TestCase("TestRepo")]
        public void UpdatesRepository_RootDirectoryPath_InitializeByConstructor_Test(string expected)
        {
            var target = new UpdatesRepository(new RepositoryIndex(), expected);

            Assert.AreEqual(expected, target.RootDirectoryPath);
        }

        [TestCase(@"C:\repo")]
        [TestCase(@"C:\repo\")]
        [TestCase(@"\\host\repo")]
        [TestCase("TestRepo")]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void UpdatesRepository_RootDirectoryPath_SetCorrectValue_Test(string? expectedValue)
        {
            var target = (UpdatesRepository)CreateTargetObject();

            target.RootDirectoryPath = expectedValue;

            Assert.AreEqual(expectedValue, target.RootDirectoryPath);
        }

        [Test]
        public void UpdatesRepository_ContainsUpdate_NegativeResult_Test()
        {
            var context = new UpdatesRepositoryTestContext();
            var target = CreateTargetObject(context);
            var targetConfig = context.UnknownConfiguration;

            bool isContains = target.ContainsUpdate(targetConfig);

            Assert.IsFalse(isContains);
        }

        [Test]
        public void UpdatesRepository_ContainsUpdate_PositiveResult_Test()
        {
            var context = new UpdatesRepositoryTestContext();
            var target = CreateTargetObject(context);
            var targetConfig = context.ExistingConfigs[0];

            bool isContains = target.ContainsUpdate(targetConfig);

            Assert.IsTrue(isContains);
        }

        [Test]
        public void UpdatesRepository_GetUpdate_NotTransitiveUpdateFound_Test()
        {
            var context = new UpdatesRepositoryTestContext();
            var target = CreateTargetObject(context);
            var targetConfig = context.ExistingConfigs[0];

            var result = target.GetUpdate(targetConfig, false);

            Assert.AreEqual(result, context.ExistingUpdates[0]);
        }

        [Test]
        public void UpdatesRepository_GetUpdate_NotTransitiveUpdateNotFound_Test()
        {
            var context = new UpdatesRepositoryTestContext();
            var target = CreateTargetObject(context);
            var targetConfig = context.UnknownConfiguration;

            var result = target.GetUpdate(targetConfig, false);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void UpdatesRepository_GetUpdate_TransitiveUpdateFound_Test()
        {
            var context = new UpdatesRepositoryTestContext();
            var target = CreateTargetObject(context);
            var targetConfig = context.ExistingConfigs[4];

            InfoBaseUpdate? result = target.GetUpdate(targetConfig, true);

            Assert.AreEqual(result, context.ExistingUpdates[4]);
        }

        [Test]
        public void UpdatesRepository_GetUpdate_TransitiveUpdateNotFound_Test()
        {
            var context = new UpdatesRepositoryTestContext();
            var target = CreateTargetObject(context);
            var targetConfig = context.ExistingConfigs[0];

            var result = target.GetUpdate(targetConfig, true);

            Assert.That(result, Is.Null);
        }

        [Test]
        [TestCaseSource(typeof(UpdatesRepositoryTestContext), nameof(UpdatesRepositoryTestContext.InsufficientConfigs))]
        public void UpdatesRepository_GetUpdate_InsufficientData_Test(InfobaseConfig targetConfig)
        {
            var context = new UpdatesRepositoryTestContext();
            var target = CreateTargetObject(context);

            Assert.That(() => target.GetUpdate(targetConfig, false), Throws.ArgumentException);
        }

        [TestCaseSource(typeof(UpdatesRepositoryTestContext), nameof(UpdatesRepositoryTestContext.PotentialUpdatesCorrectCases))]
        public void UpdatesRepository_GetPotentialUpdates_Result_Test(List<InfoBaseUpdate> updates,
                                                                         InfobaseConfig config,
                                                                         IEnumerable<InfoBaseUpdate> expected)
        {
            var context = new UpdatesRepositoryTestContext()
            {
                ExistingUpdates = updates,
                ExistingLocations = new List<string>(new string[updates.Count])
            };
            var targetObject = CreateTargetObject(context);

            var actual = targetObject.GetPotentialUpdates(config);

            Assert.That(actual, Is.EquivalentTo(expected));
        }

        [Test]
        public void UpdatesRepository_GetLocation_NoSuchUpdate_Test()
        {
            var context = new UpdatesRepositoryTestContext();
            var target = CreateTargetObject(context);
            string targetUpdateId = "key";

            Assert.Catch<KeyNotFoundException>(() => target.GetLocation(targetUpdateId));
        }

        [Test]
        public void UpdatesRepository_GetLocation_ExistingUpdate_Test()
        {
            var context = new UpdatesRepositoryTestContext();
            var target = CreateTargetObject(context);
            var targetUpdate = context.ExistingUpdates[0];
            var expected = context.ExistingLocations[0];

            var location = target.GetLocation(targetUpdate.Id);

            Assert.That(location, Is.EqualTo(expected));
        }

        [Test]
        public void UpdatesRepository_Refresh_Test()
        {
            var context = new UpdatesRepositoryTestContext();
            var target = (UpdatesRepository)CreateTargetObject();
            target.RootDirectoryPath = UpdatesRepositoryTestContext.TestRepositoryPath;
            var expected = context.ExistingUpdates;

            target.Refresh();

            Assert.That(target.Updates, Is.EquivalentTo(expected));
        }

        [Test]
        public void UpdatesRepository_Refresh_ConstantIdOfUpdate_Test()
        {
            var context = new UpdatesRepositoryTestContext();
            var target = (UpdatesRepository)CreateTargetObject();
            target.RootDirectoryPath = UpdatesRepositoryTestContext.TestRepositoryPath;
            var expected = context.ExistingUpdates;

            target.Refresh();
            target.Refresh();

            Assert.That(target.Updates, Is.EquivalentTo(expected));
        }

        [Test]
        public void UpdatesRepository_Refresh_Fail_Test()
        {
            var context = new UpdatesRepositoryTestContext();
            var target = CreateTargetObject(context);
            target.RootDirectoryPath = @"very//bad//path";

            try
            {
                target.Refresh();
            }
            catch { }

            Assert.That(target.Updates, Is.EquivalentTo(context.ExistingUpdates));
        }
    }
}
