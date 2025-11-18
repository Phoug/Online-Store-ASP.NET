using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Services.Interfaces;
using System.Net.Mime;

namespace Controllers
{
    /// <summary>
    /// Контроллер для управления элементами корзины (<see cref="CartItem"/>).
    /// </summary>
    /// <remarks>
    /// Этот контроллер предоставляет RESTful API для выполнения CRUD-операций над элементами корзины пользователя.
    /// 
    /// Каждый элемент корзины связывает товар (<see cref="Product"/>) с конкретной корзиной (<see cref="Cart"/>)
    /// и содержит количество добавленного товара.
    /// 
    /// Возможные сценарии использования:
    /// - Получение всех элементов корзины для всех пользователей (администратор);
    /// - Получение элементов корзины по конкретному CartId (конкретный пользователь);
    /// - Добавление товара в корзину;
    /// - Обновление количества товара;
    /// - Удаление товара из корзины.
    /// 
    /// Все операции выполняются через сервисный слой (<see cref="ICartItemService"/>),
    /// который инкапсулирует бизнес-логику и обращение к репозиторию.
    /// </remarks>
    [ApiController]
    [Route("cart-items")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemService _service;

        /// <summary>
        /// Инициализирует новый экземпляр контроллера <see cref="CartItemController"/>.
        /// </summary>
        /// <param name="service">Сервисный слой для работы с элементами корзины.</param>
        public CartItemController(ICartItemService service)
        {
            _service = service;
        }

        /// <summary>
        /// Получает список всех элементов корзины.
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET /cart-items
        /// 
        /// Этот метод возвращает все элементы всех корзин, включая связанные данные о товарах и пользователях.
        /// Используется, как правило, в административных интерфейсах.
        /// </remarks>
        /// <returns>Список всех элементов корзины.</returns>
        /// <response code="200">Возвращает список всех элементов корзины.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CartItem>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CartItem>>> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        /// <summary>
        /// Получает элемент корзины по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор элемента корзины.</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET /cart-items/7
        /// 
        /// Если элемент с указанным ID не найден, возвращается код <c>404 Not Found</c>.
        /// </remarks>
        /// <returns>Информация об элементе корзины.</returns>
        /// <response code="200">Элемент найден и возвращён.</response>
        /// <response code="404">Элемент с указанным ID не найден.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CartItem), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CartItem>> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null)
                return NotFound($"Cart item with ID={id} not found.");

            return Ok(item);
        }

        /// <summary>
        /// Получает все элементы корзины по идентификатору корзины.
        /// </summary>
        /// <param name="cartId">Идентификатор корзины.</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET /cart-items/cart/3
        /// 
        /// Возвращает все товары, добавленные пользователем с корзиной <c>cartId = 3</c>.
        /// </remarks>
        /// <returns>Коллекция элементов корзины.</returns>
        /// <response code="200">Элементы корзины успешно получены.</response>
        /// <response code="404">Элементы корзины для указанного CartId не найдены.</response>
        [HttpGet("cart/{cartId}")]
        [ProducesResponseType(typeof(IEnumerable<CartItem>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<CartItem>>> GetByCartId(int cartId)
        {
            var items = await _service.GetByCartIdAsync(cartId);
            if (items == null || !items.Any())
                return NotFound($"No items found for CartId={cartId}.");

            return Ok(items);
        }

        /// <summary>
        /// Добавляет новый элемент в корзину.
        /// </summary>
        /// <param name="item">Объект <see cref="CartItem"/> с указанием идентификаторов корзины и товара, а также количества.</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     POST /cart-items
        ///     {
        ///         "cartId": 3,
        ///         "productId": 12,
        ///         "quantity": 2
        ///     }
        /// 
        /// При успешном создании возвращается код <c>201 Created</c> и URI созданного ресурса.
        /// </remarks>
        /// <returns>Созданный элемент корзины.</returns>
        /// <response code="201">Элемент успешно добавлен в корзину.</response>
        /// <response code="400">Переданы некорректные данные (например, отсутствует ProductId или CartId).</response>
        [HttpPost]
        [ProducesResponseType(typeof(CartItem), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CartItem>> Create([FromBody] CartItem item)
        {
            if (item == null)
                return BadRequest("CartItem object cannot be null.");

            if (item.CartId <= 0 || item.ProductId <= 0)
                return BadRequest("Both CartId and ProductId must be specified.");

            await _service.AddAsync(item);
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        /// <summary>
        /// Обновляет информацию об элементе корзины (например, количество товара).
        /// </summary>
        /// <param name="id">Идентификатор обновляемого элемента корзины.</param>
        /// <param name="item">Обновлённые данные элемента корзины.</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     PUT /cart-items/5
        ///     {
        ///         "id": 5,
        ///         "cartId": 3,
        ///         "productId": 12,
        ///         "quantity": 4
        ///     }
        /// 
        /// Если элемент с указанным ID отсутствует, возвращается <c>404 Not Found</c>.
        /// </remarks>
        /// <response code="204">Элемент корзины успешно обновлён.</response>
        /// <response code="400">Переданы некорректные данные (например, ID не совпадают).</response>
        /// <response code="404">Элемент корзины не найден.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] CartItem item)
        {
            if (item == null || id != item.Id)
                return BadRequest("CartItem ID mismatch or body is null.");

            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
                return NotFound($"Cart item with ID={id} not found.");

            await _service.UpdateAsync(id, item);
            return NoContent();
        }

        /// <summary>
        /// Удаляет элемент корзины по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор удаляемого элемента корзины.</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     DELETE /cart-items/5
        /// 
        /// Если элемент отсутствует, возвращается <c>404 Not Found</c>.
        /// </remarks>
        /// <response code="204">Элемент корзины успешно удалён.</response>
        /// <response code="404">Элемент с указанным ID не найден.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
                return NotFound($"Cart item with ID={id} not found.");

            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
