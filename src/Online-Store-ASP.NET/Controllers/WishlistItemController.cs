using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;
using Online_Store_ASP_NET.Shared.Models;
using Shared.Services;

namespace Controllers
{
    /// <summary>
    /// Контроллер для управления элементами списка избранного (WishlistItem).
    /// </summary>
    /// <remarks>
    /// Предоставляет CRUD-операции для сущности <see cref="WishlistItem"/>:
    /// - Получение всех элементов;
    /// - Получение элемента по идентификатору;
    /// - Получение элементов по идентификатору вишлиста;
    /// - Создание нового элемента в вишлист;
    /// - Обновление существующего элемента;
    /// - Удаление элемента.
    ///
    /// Все операции делегируются сервису <see cref="IWishlistItemService"/>.
    /// </remarks>
    [ApiController]
    [Route("wishlist-item")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class WishlistItemController : ControllerBase
    {
        private readonly IWishlistItemService _service;

        /// <summary>
        /// Инициализирует новый экземпляр контроллера <see cref="WishlistItemController"/>.
        /// </summary>
        /// <param name="service">Сервис для работы с элементами вишлиста.</param>
        public WishlistItemController(IWishlistItemService service)
        {
            _service = service;
        }

        /// <summary>
        /// Возвращает все элементы вишлистов.
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET /wishlist-item
        ///
        /// Возвращает коллекцию <see cref="WishlistItem"/> с загруженными навигационными свойствами (Product, Wishlist).
        /// </remarks>
        /// <response code="200">Список элементов успешно получен.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<WishlistItem>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<WishlistItem>>> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        /// <summary>
        /// Возвращает элемент вишлиста по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор элемента.</param>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET /wishlist-item/5
        ///
        /// Если элемент не найден — возвращается 404.
        /// </remarks>
        /// <response code="200">Элемент найден.</response>
        /// <response code="404">Элемент не найден.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(WishlistItem), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WishlistItem>> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        /// <summary>
        /// Возвращает элементы вишлиста по идентификатору вишлиста.
        /// </summary>
        /// <param name="wishlistId">Идентификатор вишлиста.</param>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET /wishlist-item/wishlist/10
        ///
        /// Возвращает все элементы, принадлежащие указанному вишлисту.
        /// </remarks>
        /// <response code="200">Список элементов найден.</response>
        [HttpGet("wishlist/{wishlistId}")]
        [ProducesResponseType(typeof(IEnumerable<WishlistItem>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<WishlistItem>>> GetByWishlistId(int wishlistId)
        {
            var items = await _service.GetByWishlistIdAsync(wishlistId);
            return Ok(items);
        }

        /// <summary>
        /// Создаёт новый элемент вишлиста.
        /// </summary>
        /// <param name="item">Данные создаваемого элемента (<see cref="WishlistItem"/>).</param>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST /wishlist-item
        ///     {
        ///         "wishlistId": 1,
        ///         "productId": 5,
        ///         "addedAt": "2025-11-17T12:00:00Z"
        ///     }
        ///
        /// При успешном создании возвращается 201 Created и URI нового ресурса.
        /// </remarks>
        /// <response code="201">Элемент успешно создан.</response>
        /// <response code="400">Некорректные входные данные.</response>
        [HttpPost]
        [ProducesResponseType(typeof(WishlistItem), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<WishlistItem>> Create(WishlistItem item)
        {
            if (item == null) return BadRequest("WishlistItem cannot be null.");

            var created = await _service.AddAsync(item);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Обновляет существующий элемент вишлиста.
        /// </summary>
        /// <param name="id">Идентификатор обновляемого элемента.</param>
        /// <param name="item">Обновлённые данные элемента (<see cref="WishlistItem"/>).</param>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     PUT /wishlist-item/5
        ///     {
        ///         "id": 5,
        ///         "wishlistId": 1,
        ///         "productId": 7,
        ///         "addedAt": "2025-11-17T12:30:00Z"
        ///     }
        ///
        /// Возвращает 204 No Content при успешном обновлении.
        /// </remarks>
        /// <response code="204">Элемент успешно обновлён.</response>
        /// <response code="400">Ошибка соответствия ID.</response>
        /// <response code="404">Элемент не найден.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, WishlistItem item)
        {
            if (item == null || id != item.Id) return BadRequest("ID mismatch.");

            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            await _service.UpdateAsync(id, item);
            return NoContent();
        }

        /// <summary>
        /// Удаляет элемент вишлиста по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор удаляемого элемента.</param>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     DELETE /wishlist-item/5
        ///
        /// Если элемент не найден — возвращается 404.
        /// </remarks>
        /// <response code="204">Элемент удалён.</response>
        /// <response code="404">Элемент не найден.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}