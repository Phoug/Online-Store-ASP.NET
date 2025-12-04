using Microsoft.AspNetCore.Mvc;
using Online_Store_ASP_NET.Shared.DTO.User;
using Services.UserService;
using System.Net.Mime;

namespace Controllers
{
    /// <summary>
    /// Контроллер для управления пользователями интернет-магазина.
    /// </summary>
    /// <remarks>
    /// Предоставляет API для регистрации, авторизации, управления профилями пользователей.
    /// Поддерживает CRUD операции и специализированные методы для аутентификации.
    /// </remarks>
    [ApiController]
    [Route("[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Инициализирует новый экземпляр контроллера UserController.
        /// </summary>
        /// <param name="userService">Сервис для работы с пользователями</param>
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Получает список всех пользователей.
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET /user
        /// 
        /// Возвращает коллекцию всех зарегистрированных пользователей с базовой информацией.
        /// Не включает чувствительные данные (пароли).
        /// </remarks>
        /// <returns>Список пользователей</returns>
        /// <response code="200">Успешно возвращен список пользователей</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<UserReadDto>>> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при получении списка пользователей", error = ex.Message });
            }
        }

        /// <summary>
        /// Получает пользователя по идентификатору.
        /// </summary>
        /// <param name="id">Уникальный идентификатор пользователя</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET /user/5
        /// 
        /// Возвращает детальную информацию о конкретном пользователе.
        /// </remarks>
        /// <returns>Информация о пользователе</returns>
        /// <response code="200">Пользователь успешно найден</response>
        /// <response code="404">Пользователь с указанным ID не найден</response>
        /// <response code="400">Некорректный формат ID</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserReadDto>> GetUserById(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "ID должен быть положительным числом" });

            var user = await _userService.GetByIdAsync(id);

            if (user == null)
                return NotFound(new { message = $"Пользователь с ID {id} не найден" });

            return Ok(user);
        }

        /// <summary>
        /// Получает пользователя по имени пользователя (username).
        /// </summary>
        /// <param name="username">Уникальное имя пользователя</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET /user/by-username/john_doe
        /// 
        /// Используется для поиска пользователя по логину.
        /// </remarks>
        /// <returns>Информация о пользователе</returns>
        /// <response code="200">Пользователь успешно найден</response>
        /// <response code="404">Пользователь с указанным username не найден</response>
        /// <response code="400">Пустое или некорректное имя пользователя</response>
        [HttpGet("by-username/{username}")]
        [ProducesResponseType(typeof(UserReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserReadDto>> GetUserByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return BadRequest(new { message = "Имя пользователя не может быть пустым" });

            var user = await _userService.GetByUsernameAsync(username);

            if (user == null)
                return NotFound(new { message = $"Пользователь '{username}' не найден" });

            return Ok(user);
        }

        /// <summary>
        /// Регистрация нового пользователя.
        /// </summary>
        /// <param name="dto">Данные для создания нового пользователя</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     POST /user
        ///     {
        ///         "username": "john_doe",
        ///         "name": "Иван Иванов",
        ///         "email": "ivan@example.com",
        ///         "phone": "+7 (999) 123-45-67",
        ///         "birthDate": "1990-05-15",
        ///         "password": "SecurePassword123"
        ///     }
        /// 
        /// При регистрации автоматически создаются:
        /// - Корзина (Cart) для пользователя
        /// - Список желаемого (Wishlist)
        /// - Устанавливается роль Customer
        /// 
        /// Требования к полям:
        /// - Email должен быть уникальным
        /// - Возраст должен быть старше 14 лет
        /// - Пароль минимум 6 символов (рекомендуется использовать хеширование)
        /// </remarks>
        /// <returns>Данные созданного пользователя</returns>
        /// <response code="201">Пользователь успешно зарегистрирован</response>
        /// <response code="400">Некорректные данные или email уже существует</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(UserReadDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserReadDto>> RegisterUser([FromBody] UserCreateDto dto)
        {
            if (dto == null)
                return BadRequest(new { message = "Данные пользователя не могут быть пустыми" });

            if (string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest(new { message = "Email обязателен для регистрации" });

            if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 6)
                return BadRequest(new { message = "Пароль должен содержать минимум 6 символов" });

            try
            {
                var user = await _userService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Авторизация пользователя.
        /// </summary>
        /// <param name="loginDto">Данные для входа (email и пароль)</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     POST /user/login
        ///     {
        ///         "email": "ivan@example.com",
        ///         "password": "SecurePassword123"
        ///     }
        /// 
        /// ВАЖНО: В продакшен версии рекомендуется:
        /// - Использовать JWT токены
        /// - Хешировать пароли (BCrypt, Argon2)
        /// - Добавить защиту от брутфорса
        /// - Логировать попытки входа
        /// </remarks>
        /// <returns>Данные пользователя при успешной авторизации</returns>
        /// <response code="200">Авторизация успешна</response>
        /// <response code="401">Неверный email или пароль</response>
        /// <response code="400">Некорректные данные запроса</response>
        [HttpPost("login")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(UserReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserReadDto>> Login([FromBody] UserLoginDto loginDto)
        {
            if (loginDto == null)
                return BadRequest(new { message = "Данные для входа не могут быть пустыми" });

            if (string.IsNullOrWhiteSpace(loginDto.Email) || string.IsNullOrWhiteSpace(loginDto.Password))
                return BadRequest(new { message = "Email и пароль обязательны" });

            var user = await _userService.LoginAsync(loginDto.Email, loginDto.Password);

            if (user == null)
                return Unauthorized(new { message = "Неверный email или пароль" });

            return Ok(user);
        }

        /// <summary>
        /// Обновление данных пользователя.
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <param name="dto">Обновленные данные пользователя</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     PUT /user/5
        ///     {
        ///         "username": "john_doe_updated",
        ///         "name": "Иван Петрович Иванов",
        ///         "email": "ivan.new@example.com",
        ///         "phone": "+7 (999) 999-99-99",
        ///         "birthDate": "1990-05-15",
        ///         "password": "NewPassword123",
        ///         "role": "Manager"
        ///     }
        /// 
        /// Все поля опциональны. Обновляются только переданные поля.
        /// Роли: Guest, Customer, Manager, Admin
        /// </remarks>
        /// <returns>Обновленные данные пользователя</returns>
        /// <response code="200">Данные успешно обновлены</response>
        /// <response code="404">Пользователь не найден</response>
        /// <response code="400">Некорректные данные или ID не совпадает</response>
        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(UserReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserReadDto>> UpdateUser(int id, [FromBody] UserUpdateDto dto)
        {
            if (id <= 0)
                return BadRequest(new { message = "Некорректный ID пользователя" });

            if (dto == null)
                return BadRequest(new { message = "Данные для обновления не могут быть пустыми" });

            try
            {
                var user = await _userService.UpdateAsync(id, dto);

                if (user == null)
                    return NotFound(new { message = $"Пользователь с ID {id} не найден" });

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Удаление пользователя.
        /// </summary>
        /// <param name="id">Идентификатор пользователя для удаления</param>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     DELETE /user/5
        /// 
        /// ВНИМАНИЕ: Операция необратима!
        /// Удаляются все связанные данные:
        /// - Корзина
        /// - Список желаемого
        /// - Заказы (в зависимости от настроек каскадного удаления)
        /// - Отзывы
        /// 
        /// Рекомендуется использовать soft delete (мягкое удаление) в продакшене.
        /// </remarks>
        /// <response code="204">Пользователь успешно удален</response>
        /// <response code="404">Пользователь не найден</response>
        /// <response code="400">Некорректный ID</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Некорректный ID пользователя" });

            var result = await _userService.DeleteAsync(id);

            if (!result)
                return NotFound(new { message = $"Пользователь с ID {id} не найден" });

            return NoContent();
        }
    }
}
