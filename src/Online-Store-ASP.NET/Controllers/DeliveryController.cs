using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Services.Interfaces;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;

namespace Controllers
{
    /// <summary>
    /// Контроллер для управления доставками.
    /// Предоставляет методы для получения, создания, обновления и удаления записей о доставке.
    /// </summary>
    [ApiController]
    [Route("deliveries")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryService _service;

        /// <summary>
        /// Инициализирует новый экземпляр контроллера DeliveryController.
        /// </summary>
        /// <param name="service">Сервис для работы с доставками.</param>
        public DeliveryController(IDeliveryService service)
        {
            _service = service;
        }

        /// <summary>
        /// Возвращает список всех доставок.
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET /deliveries
        ///     
        /// </remarks>
        /// <response code="200">Список доставок успешно получен.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Delivery>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Delivery>>> GetAll()
        {
            var deliveries = await _service.GetAllAsync();
            return Ok(deliveries);
        }

        /// <summary>
        /// Возвращает информацию о доставке по её идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор доставки.</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET /deliveries/5
        ///     
        /// </remarks>
        /// <response code="200">Доставка найдена и возвращена.</response>
        /// <response code="404">Доставка с указанным ID не найдена.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Delivery), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Delivery>> GetById(int id)
        {
            var delivery = await _service.GetByIdAsync(id);
            if (delivery == null)
                return NotFound($"Доставка с идентификатором {id} не найдена.");

            return Ok(delivery);
        }

        /// <summary>
        /// Возвращает все доставки, связанные с пользователем.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET /deliveries/user/3
        ///     
        /// </remarks>
        /// <response code="200">Список доставок пользователя успешно получен.</response>
        /// <response code="404">Доставки для данного пользователя не найдены.</response>
        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<Delivery>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Delivery>>> GetByUserId(int userId)
        {
            var deliveries = await _service.GetByUserIdAsync(userId);
            if (deliveries == null || !deliveries.Any())
                return NotFound($"Для пользователя с ID = {userId} доставки не найдены.");

            return Ok(deliveries);
        }

        /// <summary>
        /// Создаёт новую запись о доставке.
        /// </summary>
        /// <param name="delivery">Объект Delivery для добавления.</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     POST /deliveries
        ///     {
        ///        "address": "ул. Ленина, д. 10",
        ///        "deliveryMethod": "Курьер",
        ///        "deliveryCost": 250,
        ///        "startDate": "2025-11-06T12:00:00",
        ///        "endDate": "2025-11-07T18:00:00",
        ///        "orderId": 5,
        ///        "userId": 3
        ///     }
        ///     
        /// </remarks>
        /// <response code="201">Доставка успешно создана.</response>
        /// <response code="400">Некорректные входные данные.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Delivery), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Delivery>> Create(Delivery delivery)
        {
            if (delivery == null)
                return BadRequest("Объект доставки не может быть пустым.");

            await _service.AddAsync(delivery);
            return CreatedAtAction(nameof(GetById), new { id = delivery.Id }, delivery);
        }

        /// <summary>
        /// Обновляет существующую запись о доставке.
        /// </summary>
        /// <param name="id">Идентификатор доставки.</param>
        /// <param name="delivery">Обновлённые данные доставки.</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     PUT /deliveries/5
        ///     {
        ///        "id": 5,
        ///        "address": "ул. Пушкина, д. 15",
        ///        "deliveryMethod": "Пункт выдачи",
        ///        "deliveryCost": 150,
        ///        "startDate": "2025-11-06T10:00:00",
        ///        "endDate": "2025-11-06T18:00:00",
        ///        "orderId": 5,
        ///        "userId": 3
        ///     }
        ///     
        /// </remarks>
        /// <response code="204">Доставка успешно обновлена.</response>
        /// <response code="400">Ошибка валидации данных или несовпадение ID.</response>
        /// <response code="404">Доставка с указанным ID не найдена.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, Delivery delivery)
        {
            if (delivery == null || id != delivery.Id)
                return BadRequest("Ошибка: ID не совпадает или объект недействителен.");

            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
                return NotFound($"Доставка с ID = {id} не найдена.");

            await _service.UpdateAsync(id, delivery);
            return NoContent();
        }

        /// <summary>
        /// Удаляет запись о доставке по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор доставки.</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     DELETE /deliveries/5
        ///     
        /// </remarks>
        /// <response code="204">Доставка успешно удалена.</response>
        /// <response code="404">Доставка с указанным ID не найдена.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
                return NotFound($"Доставка с ID = {id} не найдена.");

            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
