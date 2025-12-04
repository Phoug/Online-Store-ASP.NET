using Microsoft.EntityFrameworkCore;
using Online_Store_ASP_NET.Shared.DTO.User;
using Online_Store_ASP_NET.Shared.Models;
using Shared.Repositories;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.UserService
{
    public class UserServiceImpl : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly AppDbContext _context;

        public UserServiceImpl(IUserRepository repository, AppDbContext context)
        {
            _repository = repository;
            _context = context;
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

        public async Task<UserReadDto?> LoginAsync(string email, string password)
        {
            var users = await _repository.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Email == email);

            if (user == null || user.Password != password)
                return null;

            return MapToReadDto(user);
        }

        public async Task<UserReadDto?> GetByUsernameAsync(string username)
        {
            var user = await _repository.GetByUsernameAsync(username);
            return user == null ? null : MapToReadDto(user);
        }

        public async Task<UserReadDto> CreateAsync(UserCreateDto dto)
        {
            var usersSet = _context.Set<User>();

            if (await usersSet.AnyAsync(u => u.Email == dto.Email))
            {
                throw new Exception("Пользователь с таким Email уже существует.");
            }

            var user = new User
            {
                Username = dto.Username,
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                BirthDate = dto.BirthDate,
                Password = dto.Password,
                Role = UserRole.Customer,
                RegistrationDate = DateTime.UtcNow,
                ReviewsList = new List<Review>(),
                OrdersList = new List<Order>(),
                DeliveriesList = new List<Delivery>()
            };

            user.Cart = new Cart { User = user };
            user.Wishlist = new Wishlist { User = user };

            usersSet.Add(user);
            await _context.SaveChangesAsync();

            return MapToReadDto(user);
        }

        public async Task<UserReadDto?> UpdateAsync(int id, UserUpdateDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return null;

            if (!string.IsNullOrEmpty(dto.Username)) existing.Username = dto.Username;
            if (!string.IsNullOrEmpty(dto.Name)) existing.Name = dto.Name;
            if (!string.IsNullOrEmpty(dto.Password)) existing.Password = dto.Password;
            if (!string.IsNullOrEmpty(dto.Email)) existing.Email = dto.Email;
            if (!string.IsNullOrEmpty(dto.Phone)) existing.Phone = dto.Phone;

            if (!string.IsNullOrEmpty(dto.Role) && Enum.TryParse<UserRole>(dto.Role, true, out var role))
                existing.Role = role;

            if (dto.BirthDate.HasValue)
            {
                existing.BirthDate = dto.BirthDate.Value;
            }

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
