using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Shared.Models;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;

namespace Controllers
{
    /// <summary>
    /// Контроллер для управления отзывами пользователей о товарах.
    /// </summary>
    /// <remarks>
    /// Этот контроллер предоставляет REST API для выполнения CRUD-операций над сущностью <see cref="Review"/>.
    ///
    /// Поддерживаемые операции:
    /// - Получение списка всех отзывов;
    /// - Получение конкретного отзыва по ID;
    /// - Получение всех отзывов конкретного товара;
    /// - Создание нового отзыва;
    /// - Обновление существующего отзыва;
    /// - Удаление отзыва.
    ///
    /// ### Общие правила:
    /// - Отзыв содержит рейтинг от 1 до 5;
    /// - У отзыва должен быть автор (AuthorId) — ссылка на пользователя;
    /// - У отзыва должен быть товар (ProductId) — ссылка на продукт;
    /// - Дата создания задаётся автоматически;
    ///
    /// Данный контроллер использует сервисный слой <see cref="IReviewService"/>,
    /// что обеспечивает изоляцию бизнес-логики от контроллера и репозитория.
    ///
    /// Пример объекта:
    ///
    /// ```json
    /// {
    ///   "id": 1,
    ///   "rating": 5,
    ///   "comment": "Отличный товар!",
    ///   "createdAt": "2025-01-10T09:20:00Z",
    ///   "authorId": 12,
    ///   "productId": 3
    /// }
    /// ```
    ///
    /// </remarks>
    [ApiController]
    [Route("review")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _service;

        /// <summary>
        /// Инициализирует новый экземпляр контроллера <see cref="ReviewController"/>.
        /// </summary>
        /// <param name="service">Сервис для работы с отзывами.</param>
        public ReviewController(IReviewService service)
        {
            _service = service;
        }

        // --------------------------------------------------------------------
        // GET /review
        // --------------------------------------------------------------------

        /// <summary>
        /// Получает список всех отзывов.
        /// </summary>
        /// <remarks>
        /// Этот метод возвращает полный список отзывов в системе.
        ///
        /// Пример запроса:
        ///
        /// ```
        /// GET /review
        /// ```
        ///
        /// Пример ответа:
        /// ```json
        /// [
        ///   {
        ///     "id": 1,
        ///     "rating": 4,
        ///     "comment": "Хороший товар",
        ///     "createdAt": "2025-01-10T10:00:00Z",
        ///     "authorId": 5,
        ///     "productId": 3
        ///   }
        /// ]
        /// ```
        /// </remarks>
        /// <response code="200">Возвращает массив всех отзывов.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Review>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        {
            var reviews = await _service.GetAllAsync();
            return Ok(reviews);
        }

        // --------------------------------------------------------------------
        // GET /review/{id}
        // --------------------------------------------------------------------

        /// <summary>
        /// Возвращает отзыв по заданному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор отзыва.</param>
        /// <remarks>
        /// Пример запроса:
        ///
        /// ```
        /// GET /review/10
        /// ```
        ///
        /// Если отзыв не найден, возвращается <c>404 Not Found</c>.
        /// </remarks>
        /// <response code="200">Возвращает найденный отзыв.</response>
        /// <response code="404">Отзыв с указанным ID не найден.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Review), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Review>> GetReview(int id)
        {
            var review = await _service.GetByIdAsync(id);
            if (review == null)
                return NotFound($"Review with id {id} not found");

            return Ok(review);
        }

        // --------------------------------------------------------------------
        // GET /review/product/{productId}
        // --------------------------------------------------------------------

        /// <summary>
        /// Получает все отзывы, относящиеся к указанному товару.
        /// </summary>
        /// <param name="productId">ID товара.</param>
        /// <remarks>
        /// Метод полезен для отображения отзывов на странице товара.
        ///
        /// Пример запроса:
        ///
        /// ```
        /// GET /review/product/5
        /// ```
        ///
        /// Пример ответа:
        /// ```json
        /// [
        ///   {
        ///     "id": 1,
        ///     "rating": 5,
        ///     "comment": "Товар супер!",
        ///     "authorId": 2,
        ///     "productId": 5
        ///   }
        /// ]
        /// ```
        /// </remarks>
        /// <response code="200">Возвращает список отзывов для товара.</response>
        [HttpGet("product/{productId}")]
        [ProducesResponseType(typeof(IEnumerable<Review>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Review>>> GetByProduct(int productId)
        {
            var reviews = await _service.GetByProductIdAsync(productId);
            return Ok(reviews);
        }

        // --------------------------------------------------------------------
        // POST /review
        // --------------------------------------------------------------------

        /// <summary>
        /// Создаёт новый отзыв.
        /// </summary>
        /// <param name="review">Данные нового отзыва.</param>
        /// <remarks>
        /// Требования:
        /// - Рейтинг от 1 до 5;
        /// - Автор должен существовать (AuthorId);
        /// - Продукт должен существовать (ProductId);
        ///
        /// Пример запроса:
        /// ```json
        /// {
        ///   "rating": 5,
        ///   "comment": "Прекрасный товар!",
        ///   "authorId": 1,
        ///   "productId": 4
        /// }
        /// ```
        ///
        /// Пример ответа:
        /// ```json
        /// {
        ///   "id": 12,
        ///   "rating": 5,
        ///   "comment": "Прекрасный товар!",
        ///   "authorId": 1,
        ///   "productId": 4,
        ///   "createdAt": "2025-01-18T14:20:00Z"
        /// }
        /// ```
        /// </remarks>
        /// <response code="201">Отзыв успешно создан.</response>
        /// <response code="400">Некорректные данные отзыва.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Review), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Review>> CreateReview(Review review)
        {
            if (review == null)
                return BadRequest("Review object cannot be null.");

            if (review.Rating < 1 || review.Rating > 5)
                return BadRequest("Rating must be from 1 to 5.");

            await _service.AddAsync(review);

            return CreatedAtAction(nameof(GetReview), new { id = review.Id }, review);
        }

        // --------------------------------------------------------------------
        // PUT /review/{id}
        // --------------------------------------------------------------------

        /// <summary>
        /// Обновляет существующий отзыв.
        /// </summary>
        /// <param name="id">ID обновляемого отзыва.</param>
        /// <param name="review">Обновлённые данные.</param>
        /// <remarks>
        /// Пример запроса:
        ///
        /// ```json
        /// {
        ///   "id": 3,
        ///   "rating": 4,
        ///   "comment": "Исправленный отзыв",
        ///   "authorId": 5,
        ///   "productId": 2
        /// }
        /// ```
        ///
        /// Если отзыв отсутствует — возвращается <c>404 Not Found</c>.
        /// </remarks>
        /// <response code="204">Обновление выполнено успешно.</response>
        /// <response code="400">Ошибка валидации данных.</response>
        /// <response code="404">Отзыв не найден.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateReview(int id, Review review)
        {
            if (review == null || id != review.Id)
                return BadRequest("ID mismatch.");

            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
                return NotFound($"Review with id {id} not found");

            await _service.UpdateAsync(id, review);
            return NoContent();
        }

        // --------------------------------------------------------------------
        // DELETE /review/{id}
        // --------------------------------------------------------------------

        /// <summary>
        /// Удаляет отзыв по идентификатору.
        /// </summary>
        /// <param name="id">ID удаляемого отзыва.</param>
        /// <remarks>
        /// Пример запроса:
        ///
        /// ```
        /// DELETE /review/5
        /// ```
        ///
        /// Если отзыв отсутствует — возвращается <c>404 Not Found</c>.
        /// </remarks>
        /// <response code="204">Отзыв успешно удалён.</response>
        /// <response code="404">Отзыв не найден.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
                return NotFound($"Review with id {id} not found");

            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
