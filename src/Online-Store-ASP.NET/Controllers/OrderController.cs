using Microsoft.AspNetCore.Mvc;
using Online_Store_ASP_NET.Shared.Models;
using Services.Interfaces;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;

namespace Controllers
{
    /// <summary>
    /// Контроллер для управления заказами в интернет-магазине.
    /// </summary>
    /// <remarks>
    /// Этот контроллер предоставляет RESTful API для выполнения CRUD-операций над сущностью <see cref="Order"/>.
    /// 
    /// Методы позволяют:
    /// - Получать список всех заказов;
    /// - Получать конкретный заказ по идентификатору;
    /// - Получать все заказы конкретного пользователя;
    /// - Создавать новый заказ;
    /// - Обновлять существующий заказ;
    /// - Удалять заказ из базы данных.
    /// 
    /// Все методы взаимодействуют через сервисный слой (<see cref="IOrderService"/>),
    /// обеспечивающий бизнес-логику и изоляцию от уровня данных.
    /// </remarks>
    [ApiController]
    [Route("orders")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;

        /// <summary>
        /// Инициализирует новый экземпляр контроллера <see cref="OrderController"/>.
        /// </summary>
        /// <param name="service">Сервис для работы с заказами.</param>
        public OrderController(IOrderService service)
        {
            _service = service;
        }

        /// <summary>
        /// Получает список всех заказов.
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET /orders
        /// 
        /// Возвращает коллекцию объектов <see cref="Order"/> с полной информацией о заказе, включая пользователя, доставку и позиции заказа.
        /// </remarks>
        /// <returns>Список всех заказов.</returns>
        /// <response code="200">Список заказов успешно получен.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Order>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await _service.GetAllAsync();
            return Ok(orders);
        }

        /// <summary>
        /// Получает заказ по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор заказа.</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET /orders/5
        /// 
        /// Если заказ с указанным ID не найден, возвращается код <c>404 Not Found</c>.
        /// </remarks>
        /// <returns>Информация о найденном заказе.</returns>
        /// <response code="200">Возвращает найденный заказ.</response>
        /// <response code="404">Заказ с указанным ID не найден.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            var order = await _service.GetByIdAsync(id);
            if (order == null)
                return NotFound();
            return Ok(order);
        }

        /// <summary>
        /// Получает все заказы конкретного пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET /orders/user/3
        /// 
        /// Возвращает коллекцию объектов <see cref="Order"/> для указанного пользователя.
        /// </remarks>
        /// <returns>Список заказов пользователя.</returns>
        /// <response code="200">Список заказов пользователя успешно получен.</response>
        /// <response code="404">Заказы для данного пользователя не найдены.</response>
        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<Order>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByUserId(int userId)
        {
            var orders = await _service.GetByUserIdAsync(userId);
            if (orders == null || !orders.Any())
                return NotFound();
            return Ok(orders);
        }

        /// <summary>
        /// Создаёт новый заказ.
        /// </summary>
        /// <param name="order">Объект <see cref="Order"/>, содержащий данные нового заказа.</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     POST /orders
        ///     {
        ///        "status": "Pending",
        ///        "userId": 3,
        ///        "deliveryId": 2,
        ///        "orderItems": [
        ///           {"productId": 1, "quantity": 2},
        ///           {"productId": 5, "quantity": 1}
        ///        ]
        ///     }
        /// 
        /// При успешном создании возвращается код <c>201 Created</c> и URI нового ресурса.
        /// </remarks>
        /// <returns>Созданный заказ.</returns>
        /// <response code="201">Заказ успешно создан.</response>
        /// <response code="400">Некорректные данные запроса.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Order), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Order>> CreateOrder(Order order)
        {
            if (order == null)
                return BadRequest("Order object cannot be null.");

            await _service.AddAsync(order);
            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }

        /// <summary>
        /// Обновляет информацию о существующем заказе.
        /// </summary>
        /// <param name="id">Идентификатор обновляемого заказа.</param>
        /// <param name="order">Объект <see cref="Order"/> с обновлёнными данными.</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     PUT /orders/5
        ///     {
        ///        "id": 5,
        ///        "status": "Shipped",
        ///        "userId": 3,
        ///        "deliveryId": 2
        ///     }
        /// 
        /// Если заказ не найден — возвращается <c>404 Not Found</c>.
        /// </remarks>
        /// <response code="204">Обновление выполнено успешно.</response>
        /// <response code="400">Переданы некорректные данные.</response>
        /// <response code="404">Заказ с указанным ID не найден.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateOrder(int id, Order order)
        {
            if (order == null || id != order.Id)
                return BadRequest("Order ID mismatch.");

            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _service.UpdateAsync(id, order);
            return NoContent();
        }

        /// <summary>
        /// Удаляет заказ по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор удаляемого заказа.</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     DELETE /orders/5
        /// 
        /// Если заказ не найден — возвращается <c>404 Not Found</c>.
        /// </remarks>
        /// <response code="204">Заказ успешно удалён.</response>
        /// <response code="404">Заказ с указанным ID не найден.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
