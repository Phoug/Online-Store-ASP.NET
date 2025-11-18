using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Services.Interfaces;
using System.Net.Mime;

namespace Controllers
{
    /// <summary>
    /// Контроллер для управления категориями товаров.
    /// </summary>
    /// <remarks>
    /// Этот контроллер предоставляет RESTful API для работы с сущностью <see cref="Category"/>.
    /// 
    /// Методы контроллера позволяют:
    /// - Получить список всех категорий;
    /// - Получить категорию по идентификатору;
    /// - Создать новую категорию;
    /// - Обновить существующую категорию;
    /// - Удалить категорию из базы данных.
    /// 
    /// Все методы обращаются к сервисному слою (<see cref="ICategoryService"/>),
    /// обеспечивая разделение бизнес-логики и уровня данных.
    /// </remarks>
    [ApiController]
    [Route("category")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        /// <summary>
        /// Инициализирует новый экземпляр контроллера <see cref="CategoryController"/>.
        /// </summary>
        /// <param name="service">Сервис для работы с категориями.</param>
        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        /// <summary>
        /// Получает список всех категорий.
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET /category
        /// 
        /// Возвращает список всех категорий с их описаниями и при необходимости — вложенными товарами.
        /// </remarks>
        /// <returns>Список всех категорий.</returns>
        /// <response code="200">Успешно возвращает список категорий.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Category>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _service.GetAllAsync();
            return Ok(categories);
        }

        /// <summary>
        /// Получает категорию по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор категории.</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET /category/3
        /// 
        /// Если категория с указанным ID не найдена, возвращается код <c>404 Not Found</c>.
        /// </remarks>
        /// <returns>Информация о найденной категории.</returns>
        /// <response code="200">Категория найдена.</response>
        /// <response code="404">Категория не найдена.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Category), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Category>> GetCategoryById(int id)
        {
            var category = await _service.GetByIdAsync(id);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        /// <summary>
        /// Создаёт новую категорию.
        /// </summary>
        /// <param name="category">Объект <see cref="Category"/>, содержащий данные новой категории.</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     POST /category
        ///     {
        ///         "name": "Электроника",
        ///         "description": "Товары, связанные с электроникой и гаджетами."
        ///     }
        /// 
        /// Возвращает код <c>201 Created</c> и ссылку на созданный ресурс.
        /// </remarks>
        /// <returns>Созданная категория.</returns>
        /// <response code="201">Категория успешно создана.</response>
        /// <response code="400">Некорректные данные запроса.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Category), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Category>> CreateCategory(Category category)
        {
            if (category == null)
                return BadRequest("Category cannot be null.");

            await _service.AddAsync(category);
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }

        /// <summary>
        /// Обновляет существующую категорию.
        /// </summary>
        /// <param name="id">Идентификатор обновляемой категории.</param>
        /// <param name="category">Обновлённый объект <see cref="Category"/>.</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     PUT /category/2
        ///     {
        ///         "id": 2,
        ///         "name": "Бытовая техника",
        ///         "description": "Категория товаров для дома и кухни."
        ///     }
        /// 
        /// Если категория не найдена, возвращается <c>404 Not Found</c>.
        /// </remarks>
        /// <response code="204">Категория успешно обновлена.</response>
        /// <response code="400">Ошибка в данных запроса.</response>
        /// <response code="404">Категория не найдена.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCategory(int id, Category category)
        {
            if (category == null || id != category.Id)
                return BadRequest("Category ID mismatch.");

            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _service.UpdateAsync(id, category);
            return NoContent();
        }

        /// <summary>
        /// Удаляет категорию по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор удаляемой категории.</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     DELETE /category/4
        /// 
        /// При успешном удалении возвращает <c>204 No Content</c>.
        /// </remarks>
        /// <response code="204">Категория успешно удалена.</response>
        /// <response code="404">Категория не найдена.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
