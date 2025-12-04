using System.Net.Http.Json;
using Online_Store_ASP_NET.Shared.DTO.User;

namespace Online_Store_ASP_NET.Client.Services
{
    public interface IAuthService
    {
        Task<(bool Success, string? ErrorMessage, UserReadDto? User)> RegisterAsync(UserCreateDto dto);
        Task<(bool Success, string? ErrorMessage, UserReadDto? User)> LoginAsync(UserLoginDto dto);
    }

    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Регистрация: POST /user
        public async Task<(bool Success, string? ErrorMessage, UserReadDto? User)> RegisterAsync(UserCreateDto dto)
        {
            try
            {
                // Базовый адрес уже задан в Program.cs, здесь используем относительный путь
                var response = await _httpClient.PostAsJsonAsync("user", dto);

                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<UserReadDto>();
                    return (true, null, user);
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                return (false, errorContent, null);
            }
            catch (Exception ex)
            {
                return (false, $"Ошибка подключения: {ex.Message}", null);
            }
        }

        // Логин: POST /user/login
        public async Task<(bool Success, string? ErrorMessage, UserReadDto? User)> LoginAsync(UserLoginDto dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("user/login", dto);

                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<UserReadDto>();
                    return (true, null, user);
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                return (false, errorContent, null);
            }
            catch (Exception ex)
            {
                return (false, $"Ошибка подключения: {ex.Message}", null);
            }
        }
    }
}
