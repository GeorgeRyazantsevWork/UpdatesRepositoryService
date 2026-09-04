using Grpc.Core;
using ServiceCloud.UpdatesRepository.BusinessLogic.Properties;
using ServiceCloud.UpdatesRepository.BusinessLogic.Repository;

namespace ServiceCloud.UpdatesRepositoryService.Grpc
{
    /// <summary>
    /// gRPC-адаптер старого контракта поверх текущего репозитория обновлений
    /// </summary>
    public class UpdatesRepositoryGrpcService : UpdatesRepository.UpdatesRepositoryBase
    {
        private readonly IUpdatesRepository repository;
        private readonly ILogger<UpdatesRepositoryGrpcService> logger;

        /// <summary>
        /// Конструктор объектов класса. Инициализирует объект начальными данными
        /// </summary>
        /// <param name="repository">Репозиторий обновлений 1С</param>
        /// <param name="logger">Объект журналирования событий</param>
        /// <exception cref="ArgumentNullException">Не задан обязательный аргумент метода</exception>
        public UpdatesRepositoryGrpcService(IUpdatesRepository? repository, ILogger<UpdatesRepositoryGrpcService>? logger)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public override Task<UpdatesCountReply> GetUpdatesCount(UpdatesCountRequest request, ServerCallContext context)
        {
            using (logger.BeginScope(AppLogEvents.UpdatesCount))
            {
                try
                {
                    int count = repository.UpdatesCount;
                    return Task.FromResult(new UpdatesCountReply { Count = count });
                }
                catch (Exception exception)
                {
                    throw ToRpcException(AppLogEvents.UpdatesCount, exception);
                }
            }
        }

        /// <inheritdoc />
        public override async Task GetUpdatesList(
            UpdatesListRequest request,
            IServerStreamWriter<InfoBaseUpdate> responseStream,
            ServerCallContext context)
        {
            using (logger.BeginScope(AppLogEvents.UpdatesList))
            {
                try
                {
                    foreach (var item in repository.Updates)
                    {
                        await responseStream.WriteAsync(item.ToGrpcMessage());
                    }
                }
                catch (Exception exception)
                {
                    throw ToRpcException(AppLogEvents.UpdatesList, exception);
                }
            }
        }

        /// <inheritdoc />
        public override Task<ContainsUpdateReply> ContainsUpdate(InfoBaseConfig request, ServerCallContext context)
        {
            using (logger.BeginScope(AppLogEvents.UpdateSearch))
            {
                try
                {
                    bool found = repository.ContainsUpdate(request.ToPocoObject());
                    return Task.FromResult(new ContainsUpdateReply { Contains = found });
                }
                catch (Exception exception)
                {
                    throw ToRpcException(AppLogEvents.UpdateSearch, exception);
                }
            }
        }

        /// <inheritdoc />
        public override Task<InfoBaseUpdate> GetUpdate(InfoBaseUpdateRequest request, ServerCallContext context)
        {
            using (logger.BeginScope(AppLogEvents.UpdateInfo))
            {
                try
                {
                    if (request.Config is null)
                    {
                        throw new ArgumentNullException(nameof(request.Config), Resource.Message_TargetConfigNotSpecified);
                    }

                    var update = repository.GetUpdate(request.Config.ToPocoObject(), request.Transitive);
                    if (update == null)
                    {
                        logger.LogInformation(AppLogEvents.UpdateInfo, Resource.Message_UpdateWasNotFound);
                        throw new RpcException(new Status(StatusCode.NotFound, Resource.Message_UpdateWasNotFound));
                    }

                    return Task.FromResult(update.ToGrpcMessage());
                }
                catch (Exception exception)
                {
                    throw ToRpcException(AppLogEvents.UpdateInfo, exception);
                }
            }
        }

        /// <inheritdoc />
        public override async Task GetPotentialUpdatesList(
            InfoBaseConfig request,
            IServerStreamWriter<InfoBaseUpdate> responseStream,
            ServerCallContext context)
        {
            using (logger.BeginScope(AppLogEvents.PotentialUpdatesList))
            {
                try
                {
                    foreach (var item in repository.GetPotentialUpdates(request.ToPocoObject()))
                    {
                        await responseStream.WriteAsync(item.ToGrpcMessage());
                    }
                }
                catch (Exception exception)
                {
                    throw ToRpcException(AppLogEvents.PotentialUpdatesList, exception);
                }
            }
        }

        /// <inheritdoc />
        public override Task<InfoBaseUpdateLocationReply> GetLocation(
            InfoBaseUpdateLocationRequest request,
            ServerCallContext context)
        {
            using (logger.BeginScope(AppLogEvents.UpdateLocation))
            {
                try
                {
                    var location = repository.GetLocation(request.UpdateId);
                    if (string.IsNullOrEmpty(location))
                    {
                        throw new KeyNotFoundException(Resource.Message_UpdateWasNotFound);
                    }

                    return Task.FromResult(new InfoBaseUpdateLocationReply { Location = location });
                }
                catch (Exception exception)
                {
                    throw ToRpcException(AppLogEvents.UpdateLocation, exception);
                }
            }
        }

        private RpcException ToRpcException(EventId eventId, Exception exception)
        {
            if (exception is RpcException rpcException)
            {
                return rpcException;
            }

            if (exception is ArgumentException)
            {
                logger.LogWarning(eventId, exception.Message);
                var message = eventId.Id == AppLogEvents.UpdateLocation.Id
                    ? Resource.Message_UpdateIdentifierIsNotSet
                    : Resource.Message_ConfigurationIsInvalid;
                return new RpcException(new Status(StatusCode.InvalidArgument, message));
            }

            if (exception is KeyNotFoundException)
            {
                logger.LogWarning(eventId, Resource.Message_UpdateWasNotFound);
                return new RpcException(new Status(StatusCode.NotFound, Resource.Message_UpdateWasNotFound));
            }

            logger.LogError(eventId, exception, Resource.Message_RequestProcessingError);
            return new RpcException(new Status(StatusCode.Internal, Resource.Message_RequestProcessingError));
        }
    }
}
