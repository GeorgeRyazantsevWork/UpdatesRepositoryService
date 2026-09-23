using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ServiceCloud.UpdatesRepository.BusinessLogic.Data;
using ServiceCloud.UpdatesRepository.BusinessLogic.Repository;
using ServiceCloud.UpdatesRepositoryService.Data;
using ServiceCloud.UpdatesRepositoryService.Responses.v2;

namespace ServiceCloud.UpdatesRepositoryService.Controllers.v2
{
    /// <summary>
    /// Предоставляет API-методы для работы с обновлениями конфигураций информационных баз 1С
    /// </summary>
    [ApiController]
    [ApiVersion(2.0)]
    [Route("urs/v{version:apiVersion}/updates")]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public class UpdatesController : ControllerBase
    {
        private readonly IUpdatesRepository repository;

        /// <summary>
        /// Конструктор объектов класса. Инициализирует объект ссылкой на сервис доступа к обновлениям конфигураций ИБ 1С
        /// </summary>
        /// <param name="repositoryService">Сервис доступа к обновлениям конфигураций ИБ 1С</param>
        /// <exception cref="ArgumentNullException">Не задана ссылка на сервис доступа к обновлениям конфигураций баз 1С</exception>
        public UpdatesController(IUpdatesRepository repositoryService)
        {
            repository = repositoryService ?? throw new ArgumentNullException(nameof(repositoryService));
        }

        /// <summary>
        /// Вернуть количество обновлений конфигураций ИБ 1С, доступных в репозитории
        /// </summary>
        /// <param name="configName">Название конфигурации информационной базы 1С</param>
        /// <param name="configVersion">Номер версии конфигурации информационной базы 1С</param>
        /// <param name="configPublisher">Название издателя конфигурации информационной базы 1С</param>
        /// <param name="configEdition">Редакция конфигурации информационной базы 1С</param>
        /// <param name="transitive">Признак переходного обновления с одной редакции на другую</param>
        /// <returns>Количество обновлений, удовлетворяющих заданному критерию</returns>
        [HttpGet("count")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(TotalResponse<UpdatesCountResult>), StatusCodes.Status200OK)]
        public IActionResult GetUpdatesCount(
            string? configName,
            string? configVersion,
            string? configPublisher,
            InfoBaseConfigEdition? configEdition,
            bool? transitive)
        {
            //var result = repository.ContainsUpdate(config);
            //return Ok(new ContainsUpdateResponse(result));

            throw new NotImplementedException();
        }

        /// <summary>
        /// Получить список всех обновлений конфигураций ИБ 1С, доступных в репозитории
        /// </summary>
        /// <param name="configName">Название конфигурации информационной базы 1С</param>
        /// <param name="configVersion">Номер версии конфигурации информационной базы 1С</param>
        /// <param name="configPublisher">Название издателя конфигурации информационной базы 1С</param>
        /// <param name="configEdition">Редакция конфигурации информационной базы 1С</param>
        /// <param name="transitive">Признак переходного обновления с одной редакции на другую</param>
        /// <returns>Коллекция описаний обновлений, удовлетворяющих заданному критерию</returns>
        [HttpGet("filter")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(TotalResponse<InfoBaseUpdate>), StatusCodes.Status200OK)]
        public IActionResult GetUpdatesList(
            string? configName,
            string? configVersion,
            string? configPublisher,
            InfoBaseConfigEdition? configEdition,
            bool? transitive)
        {
            //var updates = repository.Updates;

            //return Ok(updates);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Получить содержимое обновления информационной базы 1С
        /// </summary>
        /// <param name="id">Идентификатор обновления ИБ 1С в репозитории</param>
        /// <param name="updateType">Тип переходного обновления</param>
        /// <returns>Описание найденного обновления</returns>
        [HttpGet("download/{id}")]
        [Produces("application/octet-stream")]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DownloadUpdate(string id, TransitiveUpdateType updateType = TransitiveUpdateType.BaseTopProf)
        {
            //var update = repository.GetUpdate(request, transitive);

            //if (update == null)
            //{
            //    return NotFound();
            //}

            //return Ok(update);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Получить потенциальные обновления для заданной конфигурации информационной базы 1С
        /// </summary>
        /// <param name="configName">Название конфигурации информационной базы 1С</param>
        /// <param name="configVersion">Номер версии конфигурации информационной базы 1С</param>
        /// <param name="configPublisher">Название издателя конфигурации информационной базы 1С</param>
        /// <param name="configEdition">Редакция конфигурации информационной базы 1С</param>
        /// <returns>Коллекция подходящих обновлений</returns>
        [HttpGet("potential")]
        [ProducesResponseType(typeof(TotalResponse<InfoBaseUpdate>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<InfoBaseUpdate>> GetPotentialUpdates(
            string configName,
            string configVersion,
            string configPublisher,
            InfoBaseConfigEdition configEdition)
        {
            //var updates = repository.GetPotentialUpdates(config);
            //return Ok(updates);

            throw new NotImplementedException();
        }
    }
}
