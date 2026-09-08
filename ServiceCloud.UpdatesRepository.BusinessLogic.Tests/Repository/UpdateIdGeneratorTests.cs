using ServiceCloud.UpdatesRepository.BusinessLogic.Data;

namespace ServiceCloud.UpdatesRepository.BusinessLogic.Repository.Tests
{
    [TestFixture, NonParallelizable]
    public partial class UpdateIdGeneratorTests
    {
        UpdateIdGenerator? target;

        [SetUp]
        public void TestSetup()
        {
            target = new UpdateIdGenerator();
        }

        [TearDown]
        public void TestTearDown()
        {
            target?.Dispose();
        }

        [Test]
        public void UpdateIdGenerator_Constructor_Test()
        {
            Assert.That(() => target = new UpdateIdGenerator(), Throws.Nothing);
        }

        [Test]
        public void UpdateIdGenerator_NewId_WithoutAgrument_Test()
        {
            Assert.That(() => target!.NewId(null), Throws.ArgumentNullException);
        }

        [Test]
        [TestCaseSource(typeof(Context), nameof(Context.InvalidUpdateCases))]
        public void UpdateIdGenerator_NewId_WithInvalidAgrument_Test(InfoBaseUpdate invalidUpdate)
        {
            Assert.That(() => target!.NewId(invalidUpdate),
                        Throws.ArgumentException);
        }

        [Test]
        [TestCaseSource(typeof(Context), nameof(Context.IdCases))]
        public void UpdateIdGenerator_NewId_Test(Tuple<InfoBaseUpdate, string> sample)
        {
            Assert.That(() => target!.NewId(sample.Item1),
                        Is.EqualTo(sample.Item2));
        }

        [Test]
        public void UpdateIdGenerator_NewId_AfterDisposeWithValidArgument_Test()
        {
            var context = new Context();
            target!.Dispose();

            Assert.That(() => target!.NewId(context.ValidUpdate), Throws.TypeOf<ObjectDisposedException>());
        }

        [Test]
        public void UpdateIdGenerator_NewId_AfterDisposeWithNullArgument_Test()
        {
            target!.Dispose();

            Assert.That(() => target!.NewId(null), Throws.TypeOf<ObjectDisposedException>());
        }

        [Test]
        public void UpdateIdGenerator_NewId_AfterDisposeWithInvalidArgument_Test()
        {
            var context = new Context();
            target!.Dispose();

            Assert.That(() => target!.NewId(null), Throws.TypeOf<ObjectDisposedException>());
        }

        [Test]
        public void UpdateIdGenerator_NewId_DoubleDisposing_Test()
        {
            target!.Dispose();
            Assert.That(() => target!.Dispose(), Throws.Nothing);
        }
    }
}