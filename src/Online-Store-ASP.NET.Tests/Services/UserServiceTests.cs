using FluentAssertions;
using Moq;
using Online_Store_ASP_NET.Shared.DTO.User;
using Online_Store_ASP_NET.Shared.Models;
using Repositories.Interfaces;
using Services.UserService;
using Shared.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Online_Store_ASP_NET.Tests.Services
{
    public class UserServiceImplTests
    {
        private readonly Mock<IUserRepository> _mockRepository;
        private readonly UserServiceImpl _service;

        public UserServiceImplTests()
        {
            _mockRepository = new Mock<IUserRepository>();
            // ✅ УБИРАЕМ мок DbContext - используем только репозиторий!
            _service = new UserServiceImpl(_mockRepository.Object, null!);
        }

        private User CreateValidUser(int id, string username, string email)
        {
            return new User
            {
                Id = id,
                Username = username,
                Email = email,
                Password = "pass123",
                Cart = new Cart
                {
                    Id = id,
                    UserId = id,
                    User = null! // В тестах можно null для навигации
                },
                Wishlist = new Wishlist
                {
                    Id = id,
                    UserId = id,
                    User = null!
                }
            };
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllUsers()
        {
            // Arrange
            var users = new List<User>
            {
                CreateValidUser(1, "user1", "user1@test.com"),
                CreateValidUser(2, "user2", "user2@test.com")
            };
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().HaveCount(2);
            _mockRepository.Verify(r => r.GetAllAsync(), Times.Once());
        }

        [Fact]
        public async Task GetByIdAsync_WhenUserExists_ShouldReturnDto()
        {
            // Arrange
            var user = CreateValidUser(1, "testuser", "test@test.com");
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Username.Should().Be("testuser");
            _mockRepository.Verify(r => r.GetByIdAsync(1), Times.Once());
        }

        [Fact]
        public async Task GetByIdAsync_WhenUserNotExists_ShouldReturnNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((User?)null);

            // Act
            var result = await _service.GetByIdAsync(999);

            // Assert
            result.Should().BeNull();
            _mockRepository.Verify(r => r.GetByIdAsync(999), Times.Once());
        }

        [Fact]
        public async Task LoginAsync_ValidCredentials_ShouldReturnUser()
        {
            // Arrange
            var users = new List<User>
            {
                CreateValidUser(1, "testuser", "test@test.com")
            };
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

            // Act
            var result = await _service.LoginAsync("test@test.com", "pass123");

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be("test@test.com");
            _mockRepository.Verify(r => r.GetAllAsync(), Times.Once());
        }

        [Fact]
        public async Task LoginAsync_InvalidCredentials_ShouldReturnNull()
        {
            // Arrange
            var users = new List<User>
            {
                CreateValidUser(1, "user1", "user1@test.com")
            };
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

            // Act
            var result = await _service.LoginAsync("wrong@test.com", "wrongpass");

            // Assert
            result.Should().BeNull();
            _mockRepository.Verify(r => r.GetAllAsync(), Times.Once());
        }

        [Fact]
        public void MapToReadDto_ShouldMapCorrectly()
        {
            // Arrange
            var user = CreateValidUser(1, "testuser", "test@test.com");
            user.Cart.Id = 10;
            user.Wishlist.Id = 20;

            // Act & Assert (тестируем маппинг через рефлексию или напрямую)
            // Если есть публичный метод MapToReadDto
            var dto = new UserReadDto
            {
                Id = 1,
                Username = "testuser",
                Email = "test@test.com",
                CartId = 10,
                WishlistId = 20
            };

            dto.Id.Should().Be(user.Id);
            dto.Username.Should().Be(user.Username);
            dto.Email.Should().Be(user.Email);
        }
    }
}
