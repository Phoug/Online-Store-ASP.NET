using Microsoft.AspNetCore.Mvc;
using Online_Store_ASP_NET.Shared.Models;
using Services.Interfaces;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;

namespace Controllers
{
    /// <summary>
    /// Контроллер для управления корзинами пользователей.
    /// </summary>
    /// <remarks>
    /// Этот контроллер предоставляет RESTful API для выполнения CRUD-операций над сущностью <see cref="Cart"/>.
    /// 
    /// Методы позволяют:
    /// - Получить все корзины пользователей;
    /// - Получить корзину по её идентификатору;
    /// - Получить корзину по идентификатору пользователя;
    /// - Создать новую корзину;
    /// - Обновить существующую корзину;
    /// - Удалить корзину.
    /// 
    /// Все операции проходят через сервисный слой (<see cref="ICartService"/>),
    /// обеспечивая изоляцию бизнес-логики от уровня данных.
    /// </remarks>
    [ApiController]
    [Route("cart")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;

        /// <summary>
        /// Инициализирует новый экземпляр контроллера <see cref="CartController"/>.
        /// </summary>
        /// <param name="service">Сервис для работы с корзинами.</param>
        public CartController(ICartService service)
        {
            _service = service;
        }

        /// <summary>
        /// Получает список всех корзин.
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET /cart
        /// 
        /// Возвращает список всех корзин с пользователями и товарами.
        /// </remarks>
        /// <response code="200">Успешно возвращает список корзин.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Cart>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCarts()
        {
            var carts = await _service.GetAllAsync();
            return Ok(carts);
        }

        /// <summary>
        /// Получает корзину по её идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор корзины.</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET /cart/5
        /// </remarks>
        /// <response code="200">Корзина найдена.</response>
        /// <response code="404">Корзина не найдена.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Cart), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Cart>> GetCartById(int id)
        {
            var cart = await _service.GetByIdAsync(id);
            if (cart == null)
                return NotFound();

            return Ok(cart);
        }

        /// <summary>
        /// Получает корзину по идентификатору пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET /cart/user/3
        /// 
        /// Если корзина для пользователя не найдена, возвращается <c>404 Not Found</c>.
        /// </remarks>
        /// <response code="200">Корзина пользователя найдена.</response>
        /// <response code="404">Корзина пользователя не найдена.</response>
        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(Cart), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Cart>> GetCartByUserId(int userId)
        {
            var cart = await _service.GetByUserIdAsync(userId);
            if (cart == null)
                return NotFound();

            return Ok(cart);
        }

        /// <summary>
        /// Создаёт новую корзину.
        /// </summary>
        /// <param name="cart">Объект <see cref="Cart"/>, содержащий данные новой корзины.</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     POST /cart
        ///     {
        ///         "userId": 1,
        ///         "cartItems": [
        ///             { "productId": 5, "quantity": 2 },
        ///             { "productId": 8, "quantity": 1 }
        ///         ]
        ///     }
        /// 
        /// Возвращает код <c>201 Created</c> и ссылку на созданный ресурс.
        /// </remarks>
        /// <response code="201">Корзина успешно создана.</response>
        /// <response code="400">Некорректные данные.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Cart), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Cart>> CreateCart(Cart cart)
        {
            if (cart == null)
                return BadRequest("Cart cannot be null.");

            await _service.AddAsync(cart);
            return CreatedAtAction(nameof(GetCartById), new { id = cart.Id }, cart);
        }

        /// <summary>
        /// Обновляет существующую корзину.
        /// </summary>
        /// <param name="id">Идентификатор корзины.</param>
        /// <param name="cart">Обновлённые данные корзины.</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     PUT /cart/1
        ///     {
        ///         "id": 1,
        ///         "userId": 1,
        ///         "cartItems": [
        ///             { "productId": 2, "quantity": 4 }
        ///         ]
        ///     }
        /// </remarks>
        /// <response code="204">Корзина успешно обновлена.</response>
        /// <response code="400">Ошибка в данных запроса.</response>
        /// <response code="404">Корзина не найдена.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCart(int id, Cart cart)
        {
            if (cart == null || id != cart.Id)
                return BadRequest("Cart ID mismatch.");

            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _service.UpdateAsync(id, cart);
            return NoContent();
        }

        /// <summary>
        /// Удаляет корзину по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор корзины.</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     DELETE /cart/4
        /// </remarks>
        /// <response code="204">Корзина успешно удалена.</response>
        /// <response code="404">Корзина не найдена.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCart(int id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
