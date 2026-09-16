// Ignore Spelling: Grpc

using ServiceCloud.UpdatesRepository.BusinessLogic.Data;
using DataUpdate = ServiceCloud.UpdatesRepository.BusinessLogic.Data.InfoBaseUpdate;

namespace ServiceCloud.UpdatesRepositoryService.Grpc
{
    /// <summary>
    /// Преобразование protobuf-сообщений gRPC-контракта в модели бизнес-логики и обратно
    /// </summary>
    public static class GrpcMessageExtensions
    {
        /// <summary>
        /// Создать POCO-объект конфигурации по protobuf-сообщению
        /// </summary>
        public static InfobaseConfig ToPocoObject(this InfoBaseConfig message)
        {
            return new InfobaseConfig
            {
                Name = message.Name,
                Publisher = message.Publisher,
                VersionString = message.Version,
                Edition = (InfoBaseConfigEdition)(int)message.Edition
            };
        }

        /// <summary>
        /// Создать protobuf-сообщение по POCO-объекту конфигурации
        /// </summary>
        public static InfoBaseConfig ToGrpcMessage(this InfobaseConfig data)
        {
            return new InfoBaseConfig
            {
                Name = data.Name ?? string.Empty,
                Publisher = data.Publisher ?? string.Empty,
                Version = data.VersionString ?? string.Empty,
                Edition = (InfoBaseConfig.Types.InfoBaseEdition)(int)data.Edition
            };
        }

        /// <summary>
        /// Создать protobuf-сообщение по POCO-объекту обновления
        /// </summary>
        public static InfoBaseUpdate ToGrpcMessage(this DataUpdate data)
        {
            var update = new InfoBaseUpdate
            {
                Id = data.Id ?? string.Empty,
                IsTransitive = data.IsTransitive,
                DestinationConfig = data.DestinationConfig?.ToGrpcMessage()
            };

            if (data.SourseConfigs != null)
            {
                update.SourceConfigs.AddRange(data.SourseConfigs.Select(config => config.ToGrpcMessage()));
            }

            return update;
        }
    }
}
