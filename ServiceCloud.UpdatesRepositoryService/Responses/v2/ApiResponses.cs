namespace ServiceCloud.UpdatesRepositoryService.Responses.v2
{
    /// <summary>
    /// Общий ответ на запросы к сервису
    /// </summary>
    /// <typeparam name="TValue">Тип объектов, возвращаемых запросом</typeparam>
    /// <param name="Code">Код результата выполнения запроса</param>
    /// <param name="Details">Дополнительные сведения о результате выполнения запроса</param>
    /// <param name="Result">Массив объектов, являющихся результатом выполнения запроса</param>
    public record TotalResponse<TValue>(int Code, string Details, IEnumerable<TValue>? Result);

    /// <summary>
    /// Ответ с описанием ошибки выполнения запроса к сервису
    /// </summary>
    /// <param name="Code">Числовой код ошибки</param>
    /// <param name="Details">Дополнительные сведения об ошибке</param>
    public sealed record ErrorResponse(int Code, string Details)
        : TotalResponse<object>(Code, Details, Array.Empty<object>());

    /// <summary>
    /// Количество обновлений ИБ 1С, доступных в репозитории по заданному критерию запроса
    /// </summary>
    /// <param name="Count">Количество обновлений</param>
    public sealed record UpdatesCountResult(int Count);
}
