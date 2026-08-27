using ServiceCloud.UpdatesRepository.BusinessLogic.Data;
using ServiceCloud.UpdatesRepository.BusinessLogic.Repository;

namespace ServiceCloud.UpdatesRepository.BusinessLogic.Tests.Repository
{
    [TestFixture]
    public abstract class IRepositoryIndexTests
    {
        protected abstract IRepositoryIndex CreateTargetObject();

        [Test]
        public void IRepositoryIndex_Count_DefaultValue_Test()
        {
            var target = CreateTargetObject();

            Assert.That(target.Count, Is.EqualTo(0));
        }

        [Test]
        public void IRepositoryIndex_GetUpdates_DefaultValue_Test()
        {
            var target = CreateTargetObject();

            Assert.That(target.GetUpdates(), Is.Empty);
        }
    }
}
