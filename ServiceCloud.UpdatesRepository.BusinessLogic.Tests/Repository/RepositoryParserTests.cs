using ServiceCloud.UpdatesRepository.BusinessLogic.Repository;

namespace ServiceCloud.UpdatesRepository.BusinessLogic.Tests.Repository
{
    [TestFixture]
    public class RepositoryParserTests
    {
        [Test]
        public void RepositoryParser_ValidateUpdateFiles_ExistingUpdateDirectory_Test()
        {
            foreach (var entry in UpdatesRepositoryTestContext.TestRepositoryEntries)
            {
                Assert.That(RepositoryParser.ValidateUpdateFiles(entry.Location), Is.True, entry.Location);
            }
        }

        [Test]
        public void RepositoryParser_ValidateUpdateFiles_RootDirectory_Test()
        {
            Assert.That(RepositoryParser.ValidateUpdateFiles(UpdatesRepositoryTestContext.TestRepositoryPath), Is.False);
        }

        [Test]
        public void RepositoryParser_ValidateUpdateFiles_MissingDirectory_Test()
        {
            var missingPath = Path.Combine(UpdatesRepositoryTestContext.TestRepositoryPath, "missing");

            Assert.That(RepositoryParser.ValidateUpdateFiles(missingPath), Is.False);
        }

        [Test]
        public void RepositoryParser_TryParseСUCFile_TestRepoEntries_Test()
        {
            foreach (var entry in UpdatesRepositoryTestContext.TestRepositoryEntries)
            {
                var cucPath = Directory.EnumerateFiles(entry.Location, "*.cuc").Single();

                Assert.That(RepositoryParser.TryParseСUCFile(cucPath, out var update), Is.True, cucPath);
                Assert.That(update, Is.Not.Null);
                Assert.That(update!.DestinationConfig, Is.EqualTo(entry.Update.DestinationConfig));
                Assert.That(update.IsTransitive, Is.EqualTo(entry.Update.IsTransitive));
                Assert.That(update.SourceConfigs, Is.EquivalentTo(entry.Update.SourceConfigs!));
            }
        }

        [Test]
        public void RepositoryParser_TryParseСUCFile_MissingFile_Test()
        {
            var parsed = RepositoryParser.TryParseСUCFile("not-a-file.cuc", out var update);

            Assert.That(parsed, Is.False);
            Assert.That(update, Is.Null);
        }
    }
}
