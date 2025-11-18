using Shared.Models;
using Shared.Repositories;
using Shared.DTO.User;
using System.Linq;

namespace Services.UserService
{
    public class UserServiceImpl : IUserService
    {
        private readonly IUserRepository _repository;

        public UserServiceImpl(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<UserReadDto>> GetAllAsync()
        {
            var users = await _repository.GetAllAsync();
            return users.Select(MapToReadDto).ToList();
        }

        public async Task<UserReadDto?> GetByIdAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            return user == null ? null : MapToReadDto(user);
        }

        public async Task<UserReadDto?> GetByUsernameAsync(string username)
        {
            var user = await _repository.GetByUsernameAsync(username);
            return user == null ? null : MapToReadDto(user);
        }

        public async Task<UserReadDto> CreateAsync(UserCreateDto dto)
        {
            var user = new User
            {
                Username = dto.Username,
                Name = dto.Name,
                Password = dto.Password, // TODO: hash in real app
                Email = dto.Email,
                Phone = dto.Phone ?? string.Empty,
                Role = Enum.TryParse<UserRole>(dto.Role, true, out var r) ? r : UserRole.Guest,
                RegistrationDate = DateTime.UtcNow,
                BirthDate = dto.BirthDate,
                // Не инициализируем навигации глубоко, EF заполнит при необходимости
                Cart = null!,
                Wishlist = null!
            };

            var created = await _repository.CreateAsync(user);
            await _repository.SaveChangesAsync();
            return MapToReadDto(created);
        }

        public async Task<UserReadDto?> UpdateAsync(int id, UserUpdateDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return null;

            if (!string.IsNullOrEmpty(dto.Name)) existing.Name = dto.Name;
            if (!string.IsNullOrEmpty(dto.Password)) existing.Password = dto.Password; // TODO: re-hash
            if (!string.IsNullOrEmpty(dto.Email)) existing.Email = dto.Email;
            if (!string.IsNullOrEmpty(dto.Phone)) existing.Phone = dto.Phone;
            if (!string.IsNullOrEmpty(dto.Role) && Enum.TryParse<UserRole>(dto.Role, true, out var role)) existing.Role = role;
            if (dto.BirthDate.HasValue) existing.BirthDate = dto.BirthDate.Value;

            await _repository.UpdateAsync(existing);
            await _repository.SaveChangesAsync();
            return MapToReadDto(existing);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            await _repository.DeleteAsync(id);
            await _repository.SaveChangesAsync();
            return true;
        }

        private static UserReadDto MapToReadDto(User u) =>
            new UserReadDto
            {
                Id = u.Id,
                Username = u.Username,
                Name = u.Name,
                Email = u.Email,
                Phone = u.Phone,
                Role = u.Role.ToString(),
                RegistrationDate = u.RegistrationDate,
                BirthDate = u.BirthDate,
                CartId = u.Cart?.Id ?? 0,
                WishlistId = u.Wishlist?.Id ?? 0,
                ReviewsCount = u.ReviewsList?.Count ?? 0,
                OrdersCount = u.OrdersList?.Count ?? 0,
                DeliveriesCount = u.DeliveriesList?.Count ?? 0
            };
    }
}