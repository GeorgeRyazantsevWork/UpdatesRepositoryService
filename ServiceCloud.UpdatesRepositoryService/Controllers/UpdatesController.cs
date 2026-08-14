using Microsoft.AspNetCore.Mvc;
using ServiceCloud.UpdatesRepositoryService.Models;
using ServiceCloud.UpdatesRepositoryService.Repository;

namespace ServiceCloud.UpdatesRepositoryService.Controllers
{
    [ApiController]
    [Route("api/updates")]
    public class UpdatesController(IUpdatesRepository repositoryService) : ControllerBase
    {
        private readonly IUpdatesRepository repository = repositoryService;

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

        /// <summary>
        /// Проверить наличие обновления
        /// POST api/updates/contains
        /// </summary>
        [HttpGet("contains")]
        public IActionResult ContainsUpdate(ConfigurationModel config)
        {
            var result = repository.ContainsUpdate(config);

            return Ok(new
            {
                Contains = result
            });
        }

        /// <summary>
        /// Получить конкретное обновление
        /// POST api/updates/get
        /// </summary>
        [HttpGet("get")]
        public IActionResult GetUpdate(ConfigurationModel request)
        {
            var update = repository.GetUpdate(request, transitive: false); // TODO добавить булевый параметр в запрос

            if (update == null)
            {
                return NotFound();
            }

            return Ok(update);
        }


        /// <summary>
        /// Получить потенциальные обновления
        /// POST api/updates/potential
        /// </summary>
        [HttpGet("potential")]
        public IActionResult GetPotentialUpdates(ConfigurationModel config)
        {
            var updates =
                repository.GetPotentialUpdates(config);

            return Ok(updates);
        }

        /// <summary>
        /// Получить расположение обновления
        /// POST api/updates/location
        /// </summary>
        [HttpGet("location")]
        public IActionResult GetLocation(string updateId)
        {
            var location = repository.GetLocation(updateId);

            if (location == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                Location = location
            });
        }
    }
}
