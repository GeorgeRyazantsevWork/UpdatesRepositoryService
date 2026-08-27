namespace ServiceCloud.UpdatesRepository.BusinessLogic.Repository.Tests
{
    [TestFixture]
    public abstract class IUpdatesRepositoryTests
    {
        protected abstract IUpdatesRepository CreateTargetObject();

        [Test]
        public void IUpdateRepository_Updates_DefaultValue_Test()
        {
            var targetObject = CreateTargetObject();

            Assert.That(targetObject.Updates, Is.Empty);
        }

        [Test]
        public void UpdatesRepository_UpdatesCount_DefaultValue_Test()
        {
            var target = CreateTargetObject();

            Assert.AreEqual(0, target.UpdatesCount);
        }

        [Test]
        public void IUpdateRepository_ContainsUpdate_NullArgument_Test()
        {
            var targetObject = CreateTargetObject();

            Assert.That(() => targetObject.ContainsUpdate(null), Throws.ArgumentNullException);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void IUpdateRepository_GetUpdate_NullArgument_Test(bool transitive)
        {
            var targetObject = CreateTargetObject();

            Assert.Catch<ArgumentException>(() => targetObject.GetUpdate(null, transitive));
        }

        [Test]
        public void IUpdateRepository_GetPotentialUpdates_NullArgument_Test()
        {
            var targetObject = CreateTargetObject();

            Assert.That(() => targetObject.GetPotentialUpdates(null), Throws.ArgumentNullException);
        }

        [Test]
        public void IUpdateRepository_GetLocation_NullArgument_Test()
        {
            var targetObject = CreateTargetObject();

            Assert.Catch<ArgumentException>(() => targetObject.GetLocation(null));
        }
    }
}
