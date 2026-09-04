using Moq;
using ServiceCloud.UpdatesRepository.BusinessLogic.Data;
using ServiceCloud.UpdatesRepository.BusinessLogic.Repository;
using ServiceCloud.UpdatesRepositoryService.Grpc;
using DataUpdate = ServiceCloud.UpdatesRepository.BusinessLogic.Data.InfoBaseUpdate;
using GrpcUpdate = ServiceCloud.UpdatesRepositoryService.Grpc.InfoBaseUpdate;

namespace ServiceCloud.UpdatesRepositoryService.Tests.Grpc
{
    public partial class UpdatesRepositoryGrpcServiceTests
    {
        private class TestContext
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

                ExistingUpdate = new DataUpdate
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
                Location = Path.Combine("DataSet", "TestRepo", "1c", "Accounting", "3.0.58.41");

                MockRepository = new Mock<IUpdatesRepository>();
                MockRepository.Setup(x => x.Updates).Returns(Updates);
                MockRepository.Setup(x => x.UpdatesCount).Returns(Updates.Count);
            }

            public Mock<IUpdatesRepository> MockRepository { get; }

            public InfobaseConfig ExistingConfig { get; }

            public InfobaseConfig UnknownConfig { get; }

            public DataUpdate ExistingUpdate { get; }

            public List<DataUpdate> Updates { get; }

            public string Location { get; }

            public InfoBaseConfig ExistingConfigMessage => ExistingConfig.ToGrpcMessage();

            public GrpcUpdate ExistingUpdateMessage => ExistingUpdate.ToGrpcMessage();
        }
    }
}
