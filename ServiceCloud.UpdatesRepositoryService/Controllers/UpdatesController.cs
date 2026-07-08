using Microsoft.AspNetCore.Mvc;

namespace ServiceCloud.UpdatesRepositoryService.Controllers
{
    [ApiController]
    public class UpdatesController : ControllerBase
    {
        private readonly IUpdatesRepository repository;

        public UpdatesController(IUpdatesRepository repositoryService)
        {
            repository = repositoryService;
        }

        /// <summary>
        /// Получить количество обновлений
        /// GET api/updates/count
        /// </summary>
        [HttpGet("count")]
        public IActionResult GetUpdatesCount()
        {
            var count = repository.UpdatesCount;

            return Ok(new
            {
                Count = count
            });
        }

        /// <summary>
        /// Получить список всех обновлений
        /// GET api/updates
        /// </summary>
        [HttpGet]
        public IActionResult GetUpdatesList()
        {
            var updates = repository.Updates;

            return Ok(updates);
        }

    }
}
