using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;
using Shared.DTO.Wishlist;
using Shared.Services;

namespace Server.Controllers
{
    /// <summary>
    /// Контроллер для управления списками избранного (wishlist) пользователей.
    /// </summary>
    /// <remarks>
    /// Этот контроллер предоставляет RESTful API для работы с сущностью "Wishlist" — получение, создание,
    /// обновление и удаление вишлистов, а также получение подробной информации и получения вишлиста по пользователю.
    /// 
    /// Методы:
    /// - GET /wishlist — получить все вишлисты (короткая форма);
    /// - GET /wishlist/{id} — получить вишлист по ID (короткая форма);
    /// - GET /wishlist/{id}/detailed — получить вишлист по ID (полная детализированная форма);
    /// - GET /wishlist/user/{userId} — получить вишлист по идентификатору пользователя;
    /// - POST /wishlist — создать новый вишлист;
    /// - PUT /wishlist/{id} — обновить существующий вишлист;
    /// - DELETE /wishlist/{id} — удалить вишлист.
    /// 
    /// Все операции делегируются сервисному слою <see cref="IWishlistService"/>.
    /// </remarks>
    [ApiController]
    [Route("wishlist")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _service;

        /// <summary>
        /// Инициализирует новый экземпляр контроллера <see cref="WishlistController"/>.
        /// </summary>
        /// <param name="service">Сервис для работы с вишлистами.</param>
        public WishlistController(IWishlistService service)
        {
            _service = service;
        }

        /// <summary>
        /// Получает вишлист по его идентификатору (короткая форма).
        /// </summary>
        /// <param name="id">Идентификатор вишлиста.</param>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET /wishlist/5
        ///
        /// Возвращает короткую информацию о вишлисте (id, userId, itemsCount, список элементов в коротком формате).
        /// </remarks>
        /// <returns>Информация о вишлисте в виде <see cref="WishlistReadDto"/>.</returns>
        /// <response code="200">Возвращает найденный вишлист.</response>
        /// <response code="404">Вишлист с указанным ID не найден.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(WishlistReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WishlistReadDto>> Get(int id)
        {
            var data = await _service.GetByIdAsync(id);
            return data == null ? NotFound() : Ok(data);
        }

        /// <summary>
        /// Получает подробный вишлист по идентификатору (детализированная форма).
        /// </summary>
        /// <param name="id">Идентификатор вишлиста.</param>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET /wishlist/5/detailed
        ///
        /// Возвращает расширенную информацию: пользователь (полный профиль), элементы с деталями продуктов,
        /// средний рейтинг и отзывы (если доступны), а также упрощённый список элементов для маппинга.
        /// </remarks>
        /// <returns>Детализированный вишлист в виде <see cref="WishlistDetailedDto"/>.</returns>
        /// <response code="200">Возвращает подробный вишлист.</response>
        /// <response code="404">Вишлист с указанным ID не найден.</response>
        [HttpGet("{id}/detailed")]
        [ProducesResponseType(typeof(WishlistDetailedDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WishlistDetailedDto>> GetDetailed(int id)
        {
            var data = await _service.GetDetailedAsync(id);
            return data == null ? NotFound() : Ok(data);
        }

        /// <summary>
        /// Получает вишлист по идентификатору пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET /wishlist/user/10
        ///
        /// Возвращает вишлист, принадлежащий указанному пользователю (короткая форма).
        /// </remarks>
        /// <returns>Информация о вишлисте в виде <see cref="WishlistReadDto"/>.</returns>
        /// <response code="200">Возвращает найденный вишлист.</response>
        /// <response code="404">Вишлист для указанного пользователя не найден.</response>
        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(WishlistReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WishlistReadDto>> GetByUserId(int userId)
        {
            var data = await _service.GetByUserIdAsync(userId);
            return data == null ? NotFound() : Ok(data);
        }

        /// <summary>
        /// Получает все вишлисты (короткая форма).
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET /wishlist
        ///
        /// Возвращает список всех вишлистов в системе в краткой форме (<see cref="WishlistShortDto"/>).
        /// </remarks>
        /// <returns>Коллекция вишлистов.</returns>
        /// <response code="200">Возвращает список вишлистов.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<WishlistShortDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<WishlistShortDto>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        /// <summary>
        /// Создаёт новый вишлист.
        /// </summary>
        /// <param name="dto">DTO для создания вишлиста (<see cref="WishlistCreateDto"/>).</param>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST /wishlist
        ///     {
        ///         "userId": 10
        ///     }
        ///
        /// При успешном создании возвращается код <c>201 Created</c> и URI нового ресурса.
        /// </remarks>
        /// <returns>Созданный вишлист.</returns>
        /// <response code="201">Вишлист успешно создан.</response>
        /// <response code="400">Некорректные входные данные.</response>
        [HttpPost]
        [ProducesResponseType(typeof(WishlistReadDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<WishlistReadDto>> Create(WishlistCreateDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        /// <summary>
        /// Обновляет существующий вишлист.
        /// </summary>
        /// <param name="id">Идентификатор обновляемого вишлиста.</param>
        /// <param name="dto">DTO с изменениями (<see cref="WishlistUpdateDto"/>).</param>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     PUT /wishlist/5
        ///     {
        ///        "userId": 11
        ///     }
        ///
        /// Если вишлист не найден — возвращается <c>404 Not Found</c>.
        /// </remarks>
        /// <returns>Обновлённый вишлист.</returns>
        /// <response code="200">Возвращает обновлённый вишлист.</response>
        /// <response code="404">Вишлист с указанным ID не найден.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(WishlistReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WishlistReadDto>> Update(int id, WishlistUpdateDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            return updated == null ? NotFound() : Ok(updated);
        }

        /// <summary>
        /// Удаляет вишлист по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор удаляемого вишлиста.</param>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     DELETE /wishlist/5
        ///
        /// Если вишлист не найден — возвращается <c>404 Not Found</c>.
        /// </remarks>
        /// <response code="204">Вишлист успешно удалён.</response>
        /// <response code="404">Вишлист с указанным ID не найден.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            bool deleted = await _service.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}