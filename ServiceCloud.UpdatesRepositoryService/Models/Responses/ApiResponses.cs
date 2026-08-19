namespace ServiceCloud.UpdatesRepositoryService.Models.Responses
{
    public class ApiResponses
    {
        /// <summary>
        /// Количество обновлений, доступных в репозитории
        /// </summary>
        /// <param name="Count">Общее количество обновлений</param>
        public sealed record UpdatesCountResponse(int Count);

        /// <summary>
        /// Результат проверки наличия обновления для заданной конфигурации
        /// </summary>
        /// <param name="Contains">TRUE — обновление найдено в репозитории, FALSE — в противном случае</param>
        public sealed record ContainsUpdateResponse(bool Contains);

        /// <summary>
        /// Расположение файлов обновления в репозитории
        /// </summary>
        /// <param name="Location">Путь к каталогу с файлами обновления</param>
        public sealed record UpdateLocationResponse(string Location);

        /// <summary>
        /// Сообщение об ошибке обработки HTTP-запроса
        /// </summary>
        /// <param name="Message">Текст ошибки</param>
        public sealed record ErrorResponse(string Message);
    }
}