using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using ServiceCloud.UpdatesRepositoryService.Middleware;

namespace ServiceCloud.UpdatesRepositoryService.Tests.Middleware
{
    [TestFixture]
    public class ExceptionHandlingMiddlewareTests
    {
        [Test]
        public async Task ExceptionHandlingMiddleware_InvokeAsync_NoException_Test()
        {
            var invoked = false;
            var httpContext = new DefaultHttpContext();
            var target = new ExceptionHandlingMiddleware(_ =>
            {
                invoked = true;
                return Task.CompletedTask;
            }, NullLogger<ExceptionHandlingMiddleware>.Instance);

            await target.InvokeAsync(httpContext);

            Assert.That(invoked, Is.True);
            Assert.That(httpContext.Response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        }

        [Test]
        public async Task ExceptionHandlingMiddleware_InvokeAsync_ArgumentNullException_Test()
        {
            var httpContext = CreateHttpContext();
            var target = new ExceptionHandlingMiddleware(
                _ => throw new ArgumentNullException("config"),
                NullLogger<ExceptionHandlingMiddleware>.Instance);

            await target.InvokeAsync(httpContext);

            Assert.That(httpContext.Response.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
            Assert.That(await ReadMessage(httpContext), Is.Not.Empty);
        }

        [Test]
        public async Task ExceptionHandlingMiddleware_InvokeAsync_ArgumentException_Test()
        {
            var httpContext = CreateHttpContext();
            var target = new ExceptionHandlingMiddleware(
                _ => throw new ArgumentException("incorrect"),
                NullLogger<ExceptionHandlingMiddleware>.Instance);

            await target.InvokeAsync(httpContext);

            Assert.That(httpContext.Response.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
            Assert.That(await ReadMessage(httpContext), Is.Not.Empty);
        }

        [Test]
        public void ExceptionHandlingMiddleware_InvokeAsync_GrpcRequest_DoesNotHandleException_Test()
        {
            var httpContext = new DefaultHttpContext
            {
                Request = { ContentType = "application/grpc" }
            };
            var target = new ExceptionHandlingMiddleware(
                _ => throw new InvalidOperationException("fail"),
                NullLogger<ExceptionHandlingMiddleware>.Instance);

            Assert.That(
                async () => await target.InvokeAsync(httpContext),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public async Task ExceptionHandlingMiddleware_InvokeAsync_UnhandledException_Test()
        {
            var httpContext = CreateHttpContext();
            var target = new ExceptionHandlingMiddleware(
                _ => throw new InvalidOperationException("fail"),
                NullLogger<ExceptionHandlingMiddleware>.Instance);

            await target.InvokeAsync(httpContext);

            Assert.That(httpContext.Response.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
            Assert.That(await ReadMessage(httpContext), Is.EqualTo("Ошибка при обработке запроса"));
        }

        private static DefaultHttpContext CreateHttpContext()
        {
            return new DefaultHttpContext
            {
                Response = { Body = new MemoryStream() }
            };
        }

        private static async Task<string> ReadMessage(HttpContext httpContext)
        {
            httpContext.Response.Body.Position = 0;
            var payload = await JsonSerializer.DeserializeAsync<JsonElement>(httpContext.Response.Body);
            if (payload.TryGetProperty("Message", out var message) ||
                payload.TryGetProperty("message", out message))
            {
                return message.GetString() ?? string.Empty;
            }

            return string.Empty;
        }
    }
}
