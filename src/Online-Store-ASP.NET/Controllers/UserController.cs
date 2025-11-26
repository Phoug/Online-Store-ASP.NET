using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;
using Shared.DTO.User;
using Services.UserService;

namespace Server.Controllers
{
    /// <summary>
    /// Контроллер для управления пользователями.
    /// </summary>
    /// <remarks>
    /// Предоставляет CRUD-эндпоинты для пользователей:
    /// - GET /user — список пользователей;
    /// - GET /user/{id} — получение пользователя по id;
    /// - GET /user/by-username/{username} — получить пользователя по логину;
    /// - POST /user — создать пользователя;
    /// - PUT /user/{id} — обновить пользователя;
    /// - DELETE /user/{id} — удалить пользователя.
    /// </remarks>
    [ApiController]
    [Route("user")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        /// <summary>
        /// Получает всех пользователей.
        /// </summary>
        /// <response code="200">Возвращает список пользователей.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserReadDto>>> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(data);
        }

        /// <summary>
        /// Получает пользователя по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <response code="200">Пользователь найден.</response>
        /// <response code="404">Пользователь не найден.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserReadDto>> GetById(int id)
        {
            var user = await _service.GetByIdAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        /// <summary>
        /// Получает пользователя по логину (username).
        /// </summary>
        /// <param name="username">Логин пользователя.</param>
        /// <response code="200">Пользователь найден.</response>
        /// <response code="404">Пользователь не найден.</response>
        [HttpGet("by-username/{username}")]
        [ProducesResponseType(typeof(UserReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserReadDto>> GetByUsername(string username)
        {
            var user = await _service.GetByUsernameAsync(username);
            return user == null ? NotFound() : Ok(user);
        }

        /// <summary>
        /// Создаёт нового пользователя.
        /// </summary>
        /// <param name="dto">Данные для создания пользователя.</param>
        /// <response code="201">Пользователь создан.</response>
        /// <response code="400">Некорректные данные.</response>
        [HttpPost]
        [ProducesResponseType(typeof(UserReadDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserReadDto>> Create(UserCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Обновляет пользователя.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <param name="dto">Данные для обновления.</param>
        /// <response code="200">Возвращает обновлённого пользователя.</response>
        /// <response code="404">Пользователь не найден.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UserReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserReadDto>> Update(int id, UserUpdateDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            return updated == null ? NotFound() : Ok(updated);
        }

        /// <summary>
        /// Удаляет пользователя по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <response code="204">Пользователь удалён.</response>
        /// <response code="404">Пользователь не найден.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}