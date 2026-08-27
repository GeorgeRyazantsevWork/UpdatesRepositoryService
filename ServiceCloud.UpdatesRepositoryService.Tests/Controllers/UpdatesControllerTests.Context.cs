using Moq;
using ServiceCloud.UpdatesRepository.BusinessLogic.Data;
using ServiceCloud.UpdatesRepository.BusinessLogic.Repository;

namespace ServiceCloud.UpdatesRepositoryService.Tests.Controllers
{
    public partial class UpdatesControllerTests
    {
        class TestContext
        {
            public TestContext()
            {
                ExistingConfig = new InfobaseConfig
                {
                    Publisher = "Фирма \"1С\"",
                    Name = "БухгалтерияПредприятия",
                    Version = new InfoBaseConfigVersion("3.0.58.41"),
                    Edition = InfoBaseConfigEdition.PROF
                };

                UnknownConfig = new InfobaseConfig
                {
                    Publisher = "Рарус",
                    Name = "Микрофининсы",
                    Version = new InfoBaseConfigVersion("1.0.5.186"),
                    Edition = InfoBaseConfigEdition.PROF
                };

                ExistingUpdate = new InfoBaseUpdate
                {
                    Id = "103EB5F83D0EE3AAB321E667FE3641771874AD6A6AB3FDAF871639EFFA6A6FC7",
                    DestinationConfig = ExistingConfig,
                    SourseConfigs =
                    [
                        new InfobaseConfig
                        {
                            Publisher = "Фирма \"1С\"",
                            Name = "БухгалтерияПредприятия",
                            Version = new InfoBaseConfigVersion("3.0.58.26"),
                            Edition = InfoBaseConfigEdition.PROF
                        }
                    ]
                };

                Updates = [ExistingUpdate];
                UpdatesCount = Updates.Count;
                Location = Path.Combine("DataSet", "TestRepo", "1c", "Accounting", "3.0.58.41");

                MockRepository = new Mock<IUpdatesRepository>();
                MockRepository.Setup(x => x.Updates).Returns(Updates);
                MockRepository.Setup(x => x.UpdatesCount).Returns(UpdatesCount);
            }

            public Mock<IUpdatesRepository> MockRepository { get; }

            public InfobaseConfig ExistingConfig { get; }

            public InfobaseConfig UnknownConfig { get; }

            public InfoBaseUpdate ExistingUpdate { get; }

            public List<InfoBaseUpdate> Updates { get; }

            public int UpdatesCount { get; }

            public string Location { get; }
        }
    }
}
