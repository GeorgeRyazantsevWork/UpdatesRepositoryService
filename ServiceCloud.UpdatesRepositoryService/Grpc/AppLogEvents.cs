using Microsoft.Extensions.Logging;

namespace ServiceCloud.UpdatesRepositoryService.Grpc
{
    /// <summary>
    /// Идентификаторы журналируемых событий gRPC-интерфейса
    /// </summary>
    public static class AppLogEvents
    {
        /// <summary>
        /// Запрос общего количества обновлений в репозитории
        /// </summary>
        public static readonly EventId UpdatesCount = new(100, nameof(UpdatesRepositoryGrpcService.GetUpdatesCount));

        /// <summary>
        /// Запрос коллекции имеющихся в репозитории обновлений
        /// </summary>
        public static readonly EventId UpdatesList = new(110, nameof(UpdatesRepositoryGrpcService.GetUpdatesList));

        /// <summary>
        /// Поиск обновления для заданной конфигурации информационной базы 1С
        /// </summary>
        public static readonly EventId UpdateSearch = new(120, nameof(UpdatesRepositoryGrpcService.ContainsUpdate));

        /// <summary>
        /// Запрос информации об обновлении конфигурации информационной базы 1С
        /// </summary>
        public static readonly EventId UpdateInfo = new(130, nameof(UpdatesRepositoryGrpcService.GetUpdate));

        /// <summary>
        /// Запрос списка обновлений, которые можно установить на указанную конфигурацию базы
        /// </summary>
        public static readonly EventId PotentialUpdatesList = new(140, nameof(UpdatesRepositoryGrpcService.GetPotentialUpdatesList));

        /// <summary>
        /// Запрос информации о размещении бинарных данных обновления
        /// </summary>
        public static readonly EventId UpdateLocation = new(150, nameof(UpdatesRepositoryGrpcService.GetLocation));
    }
}
