using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Services.Interfaces;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;

namespace Controllers
{
    /// <summary>
    /// Контроллер для управления товарами в интернет-магазине.
    /// </summary>
    /// <remarks>
    /// Этот контроллер предоставляет RESTful API для выполнения CRUD-операций над сущностью <see cref="Product"/>.
    /// 
    /// Методы позволяют:
    /// - Получать список всех товаров;
    /// - Получать конкретный товар по идентификатору;
    /// - Создавать новый товар;ф
    /// - Обновлять существующий товар;
    /// - Удалять товар из базы данных.
    /// 
    /// Все методы взаимодействуют через сервисный слой (<see cref="IProductService"/>),
    /// обеспечивающий бизнес-логику и изоляцию от уровня данных.
    /// </remarks>
    [ApiController]
    [Route("product")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;

        /// <summary>
        /// Инициализирует новый экземпляр контроллера <see cref="ProductController"/>.
        /// </summary>
        /// <param name="service">Сервис для работы с товарами.</param>
        public ProductController(IProductService service)
        {
            _service = service;
        }

        /// <summary>
        /// Получает список всех товаров.
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET /product
        /// 
        /// Возвращает коллекцию объектов <see cref="Product"/> со всей доступной информацией (название, цена, описание и т.д.).
        /// </remarks>
        /// <returns>Список всех товаров.</returns>
        /// <response code="200">Возвращает список товаров.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _service.GetAllAsync();
            return Ok(products);
        }

        /// <summary>
        /// Получает товар по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор товара.</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET product/5
        /// 
        /// Если товар с указанным ID не найден, возвращается код <c>404 Not Found</c>.
        /// </remarks>
        /// <returns>Информация о найденном товаре.</returns>
        /// <response code="200">Возвращает найденный товар.</response>
        /// <response code="404">Товар с указанным ID не найден.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _service.GetByIdAsync(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        /// <summary>
        /// Создаёт новый товар.
        /// </summary>
        /// <param name="product">Объект <see cref="Product"/>, содержащий данные нового товара.</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     POST product
        ///     {
        ///        "name": "Apple iPhone 16",
        ///        "price": 1299.99,
        ///        "description": "Новый флагманский смартфон Apple.",
        ///        "categoryId": 1
        ///     }
        /// 
        /// При успешном создании возвращается код <c>201 Created</c> и URI нового ресурса.
        /// </remarks>
        /// <returns>Созданный товар.</returns>
        /// <response code="201">Товар успешно создан.</response>
        /// <response code="400">Некорректные данные запроса.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            if (product == null)
                return BadRequest("Product object cannot be null.");

            await _service.AddAsync(product);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        /// <summary>
        /// Обновляет информацию о существующем товаре.
        /// </summary>
        /// <param name="id">Идентификатор обновляемого товара.</param>
        /// <param name="product">Объект <see cref="Product"/> с обновлёнными данными.</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     PUT product/5
        ///     {
        ///        "id": 5,
        ///        "name": "Apple iPhone 16 Pro",
        ///        "price": 1399.99,
        ///        "description": "Улучшенная версия с камерой Pro."
        ///     }
        /// 
        /// Если товар не найден — возвращается <c>404 Not Found</c>.
        /// </remarks>
        /// <response code="204">Обновление выполнено успешно.</response>
        /// <response code="400">Переданы некорректные данные.</response>
        /// <response code="404">Товар с указанным ID не найден.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (product == null || id != product.Id)
                return BadRequest("Product ID mismatch.");

            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _service.UpdateAsync(id, product);
            return NoContent();
        }

        /// <summary>
        /// Удаляет товар по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор удаляемого товара.</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     DELETE product/5
        /// 
        /// Если товар не найден — возвращается <c>404 Not Found</c>.
        /// </remarks>
        /// <response code="204">Товар успешно удалён.</response>
        /// <response code="404">Товар с указанным ID не найден.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
