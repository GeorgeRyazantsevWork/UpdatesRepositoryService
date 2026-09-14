using Grpc.Core;
using Moq;
using System.Threading;

namespace ServiceCloud.UpdatesRepositoryService.Tests.Grpc
{
    public class TestServerCallContext : ServerCallContext
    {
        protected override string MethodCore => string.Empty;

        protected override string HostCore => string.Empty;

        protected override string PeerCore => string.Empty;

        protected override DateTime DeadlineCore => DateTime.MaxValue;

        protected override Metadata RequestHeadersCore => new();

        protected override CancellationToken CancellationTokenCore => CancellationToken.None;

        protected override Metadata ResponseTrailersCore => new();

        protected override Status StatusCore { get; set; }

        protected override WriteOptions? WriteOptionsCore { get; set; }

        protected override AuthContext AuthContextCore => new(string.Empty, new Dictionary<string, List<AuthProperty>>());

        public static ServerCallContext Create()
        {
            return new TestServerCallContext();
        }

        protected override ContextPropagationToken CreatePropagationTokenCore(ContextPropagationOptions? options)
        {
            return Mock.Of<ContextPropagationToken>();
        }

        protected override Task WriteResponseHeadersAsyncCore(Metadata responseHeaders)
        {
            return Task.CompletedTask;
        }
    }
}
