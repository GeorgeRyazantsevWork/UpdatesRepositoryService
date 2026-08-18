using ServiceCloud.UpdatesRepositoryService.Properties;

namespace ServiceCloud.UpdatesRepositoryService.Middleware
{
    /// <summary>
    /// Middleware для централизованной обработки исключений,
    /// возникающих при обработке HTTP-запросов
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionHandlingMiddleware> logger;

        /// <summary>
        /// Конструктор экземпляров класса. Инициализирует объект начальными данными
        /// </summary>
        /// <param name="next">Делегат следующего компонента в конвейере обработки HTTP-запроса</param>
        /// <param name="logger">Регистратор событий для записи информации о возникших исключениях</param>
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        /// <summary>
        /// Перехватывает исключения, возникающие в последующих компонентах конвейера
        /// </summary>
        /// <param name="context">Контекст текущего HTTP-запроса</param>
        /// <returns>Асинхронная задача, представляющая обработку HTTP-запроса</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка при обработке HTTP-запроса");

                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (statusCode, message) = exception switch
            {
                ArgumentNullException =>
                    (StatusCodes.Status400BadRequest, string.Format(Resource.ReplyErrorMessageTemplate_RequestFieldNotDefined, exception.Message)),

                ArgumentException =>
                    (StatusCodes.Status400BadRequest, string.Format(Resource.ReplyMessageTemplate_IncorrectRequestData, exception.Message)),

                _ =>
                    (StatusCodes.Status500InternalServerError,
                     "Ошибка при обработке запроса")
            };

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new
            {
                Message = message
            });
        }
    }
}
