using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ServiceCloud.UpdatesRepository.BusinessLogic.Data;
using ServiceCloud.UpdatesRepository.BusinessLogic.Repository;
using static ServiceCloud.UpdatesRepositoryService.ApiResponses;

namespace ServiceCloud.UpdatesRepositoryService.Controllers
{
    /// <summary>
    /// Предоставляет API-методы для работы с обновлениями через репозиторий 
    /// </summary>
    /// <param name="repositoryService">Интерфейс репозитория с обновлениями конфигураций ИБ</param>

    [ApiController]
    [ApiVersion(1.0)]
    [Route("urs/v{version:apiVersion}/updates")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public class UpdatesController(IUpdatesRepository repositoryService) : ControllerBase
    {
        private readonly IUpdatesRepository repository = repositoryService;

        /// <summary>
        /// Получить общее количество обновлений
        /// </summary>
        /// <returns>Количество доступных обновлений</returns>
        /// <response code="200">Количество обновлений успешно получено</response>
        [HttpGet("count")]
        [ProducesResponseType(typeof(UpdatesCountResponse), StatusCodes.Status200OK)]
        public IActionResult GetUpdatesCount()
        {
            return Ok(new UpdatesCountResponse(repository.UpdatesCount));
        }

        /// <summary>
        /// Получить список всех обновлений
        /// </summary>
        /// <returns>Коллекция описаний обновлений</returns>
        /// <response code="200">Список обновлений успешно получен</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<InfoBaseUpdate>), StatusCodes.Status200OK)]
        public IActionResult GetUpdatesList()
        {
            var updates = repository.Updates;

            return Ok(updates);
        }

        /// <summary>
        /// Проверить наличие обновления
        /// </summary>
        /// <returns>Признак наличия обновления в репозитории</returns>
        /// <response code="200">Проверка выполнена</response>
        /// <response code="400">Конфигурация не задана или задана не полностью</response>
        [HttpGet("contains")]
        [ProducesResponseType(typeof(ContainsUpdateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public ActionResult<ContainsUpdateResponse> ContainsUpdate([FromQuery] InfobaseConfig config)
        {
            var result = repository.ContainsUpdate(config);
            return Ok(new ContainsUpdateResponse(result));
        }

        /// <summary>
        /// Получить конкретное обновление
        /// </summary>
        /// <param name="request">Целевая конфигурация информационной базы</param>
        /// <param name="transitive">Признак, указывающи является ли обновление переходным</param>
        /// <returns>Описание найденного обновления</returns>
        /// <response code="200">Обновление найдено</response>
        /// <response code="400">Конфигурация не задана или задана не полностью</response>
        /// <response code="404">Обновление для указанной конфигурации отсутствует</response>
        [HttpGet("concrete")]
        [ProducesResponseType(typeof(InfoBaseUpdate), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<InfoBaseUpdate> GetUpdate([FromQuery] InfobaseConfig request, [FromQuery] bool transitive = false)
        {
            var update = repository.GetUpdate(request, transitive);

            if (update == null)
            {
                return NotFound();
            }

            return Ok(update);
        }

        /// <summary>
        /// Получить потенциальные обновления для заданной конфигурации
        /// </summary>
        /// <param name="config">Конфигурация информационной базы, для которой подбираются обновления</param>
        /// <returns>Коллекция подходящих обновлений</returns>
        /// <response code="200">Список потенциальных обновлений успешно получен</response>
        /// <response code="400">Конфигурация не задана или задана не полностью</response>
        [HttpGet("potential")]
        [ProducesResponseType(typeof(IEnumerable<InfoBaseUpdate>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<InfoBaseUpdate>> GetPotentialUpdates([FromQuery] InfobaseConfig config)
        {
            var updates = repository.GetPotentialUpdates(config);
            return Ok(updates);
        }

        /// <summary>
        /// Получить расположение файлов обновления в репозитории
        /// </summary>
        /// <param name="updateId">Идентификатор обновления в репозитории</param>
        /// <returns>Путь к каталогу с файлами обновления</returns>
        /// <response code="200">Расположение обновления успешно получено</response>
        /// <response code="400">Идентификатор обновления не задан</response>
        /// <response code="404">Обновление с указанным идентификатором отсутствует</response>
        [HttpGet("location")]
        [ProducesResponseType(typeof(UpdateLocationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UpdateLocationResponse> GetLocation([FromQuery] string updateId)
        {
            var location = repository.GetLocation(updateId);

            if (location == null)
            {
                return NotFound();
            }

            return Ok(new UpdateLocationResponse(location));
        }
    }
}
