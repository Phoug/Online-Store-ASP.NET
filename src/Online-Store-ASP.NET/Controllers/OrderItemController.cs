using Microsoft.AspNetCore.Mvc;
using Online_Store_ASP_NET.Shared.Models;
using Services.Interfaces;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;

namespace Controllers
{
    /// <summary>
    /// Контроллер для управления позициями заказа (OrderItem) в интернет-магазине.
    /// </summary>
    /// <remarks>
    /// Этот контроллер предоставляет RESTful API для выполнения CRUD-операций
    /// над сущностью <see cref="OrderItem"/>.
    ///
    /// Методы позволяют:
    /// - Получать список всех позиций заказа;
    /// - Получать конкретную позицию по идентификатору;
    /// - Создавать новую позицию заказа;
    /// - Обновлять существующую позицию;
    /// - Удалять позицию из заказа.
    ///
    /// Все методы используют сервис <see cref="IOrderItemService"/>,
    /// обеспечивающий бизнес-логику и работу с репозиторием.
    /// </remarks>
    [ApiController]
    [Route("order-item")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemService _service;

        /// <summary>
        /// Создаёт экземпляр контроллера <see cref="OrderItemController"/>.
        /// </summary>
        /// <param name="service">Сервис для работы с позициями заказа.</param>
        public OrderItemController(IOrderItemService service)
        {
            _service = service;
        }

        /// <summary>
        /// Возвращает список всех позиций заказа.
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET /order-item
        /// 
        /// Возвращает коллекцию <see cref="OrderItem"/> с включёнными данными
        /// о товаре и заказе.
        /// </remarks>
        /// <response code="200">Список позиций успешно получен.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OrderItem>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderItems()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        /// <summary>
        /// Возвращает позицию заказа по ID.
        /// </summary>
        /// <param name="id">Идентификатор позиции заказа.</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET /order-item/5
        /// </remarks>
        /// <response code="200">Позиция найдена.</response>
        /// <response code="404">Позиция не найдена.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrderItem), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderItem>> GetOrderItem(int id)
        {
            var item = await _service.GetAsync(id);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        /// <summary>
        /// Создаёт новую позицию заказа.
        /// </summary>
        /// <param name="item">Данные создаваемой позиции.</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     POST /order-item
        ///     {
        ///         "quantity": 2,
        ///         "orderId": 1,
        ///         "productId": 3
        ///     }
        ///
        /// При успешном создании возвращается <c>201 Created</c>.
        /// </remarks>
        /// <response code="201">Позиция успешно создана.</response>
        /// <response code="400">Некорректные данные запроса.</response>
        [HttpPost]
        [ProducesResponseType(typeof(OrderItem), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderItem>> CreateOrderItem(OrderItem item)
        {
            if (item == null)
                return BadRequest("OrderItem cannot be null.");

            var created = await _service.CreateAsync(item);

            return CreatedAtAction(nameof(GetOrderItem), new { id = created.Id }, created);
        }

        /// <summary>
        /// Обновляет существующую позицию заказа.
        /// </summary>
        /// <param name="id">Идентификатор позиции заказа.</param>
        /// <param name="item">Обновлённые данные позиции.</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     PUT /order-item/5
        ///     {
        ///         "id": 5,
        ///         "quantity": 3,
        ///         "orderId": 1,
        ///         "productId": 3
        ///     }
        /// </remarks>
        /// <response code="204">Позиция успешно обновлена.</response>
        /// <response code="400">Ошибка соответствия ID.</response>
        /// <response code="404">Позиция не найдена.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateOrderItem(int id, OrderItem item)
        {
            if (item == null || id != item.Id)
                return BadRequest("ID mismatch.");

            var existing = await _service.GetAsync(id);
            if (existing == null)
                return NotFound();

            await _service.UpdateAsync(id, item);

            return NoContent();
        }

        /// <summary>
        /// Удаляет позицию заказа по ID.
        /// </summary>
        /// <param name="id">Идентификатор позиции заказа.</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     DELETE /order-item/5
        /// </remarks>
        /// <response code="204">Позиция удалена.</response>
        /// <response code="404">Позиция не найдена.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            var existing = await _service.GetAsync(id);
            if (existing == null)
                return NotFound();

            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
